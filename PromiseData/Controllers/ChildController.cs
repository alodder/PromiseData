﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PromiseData.Models;
using PromiseData.ViewModels;
using System.Net;
using System.Security.Claims;

namespace PromiseData.Controllers
{
    public class ChildController : Controller
    {
        private ApplicationDbContext _context;
        private Dictionary<int, bool> RaceBoolDictionary;

        public ChildController()
        {
            _context = new ApplicationDbContext();
            RaceBoolDictionary = new Dictionary<int, bool>();
            var raceList = _context.RaceEthnic.ToList();
            foreach (RaceEthnicity race in raceList)
            {
                RaceBoolDictionary.Add(race.Id, false);
            }
        }

        [Authorize]
        [HttpGet]
        public ActionResult Add()
        {
            var viewModel = new ChildFormViewModel
            {
                Genders = _context.CodeGender.ToList(),
                Languages = _context.CodeLanguage.ToList(),
                RaceEthnicityList = _context.RaceEthnic.ToList(),
                Generations = _context.Code_GenerationCode.ToList()
            };
            return View(viewModel);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(ChildFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Genders = _context.CodeGender.ToList();
                viewModel.Languages = _context.CodeLanguage.ToList();
                viewModel.RaceEthnicityList = _context.RaceEthnic.ToList();
                viewModel.Generations = _context.Code_GenerationCode.ToList();
                return View("Add", viewModel);
            }
                

            var child = new Child
            {
                LastName = viewModel.LastName,
                OtherLastName = viewModel.OtherLastName,
                FirstName = viewModel.FirstName,
                MiddleName = viewModel.MiddleName,
                OtherMiddleName = viewModel.OtherMiddleName,
                Birthdate = viewModel.Date,
                GenerationCode_ID = viewModel.GenerationCodeID,
                Language_ID = viewModel.LanguageID,
                Gender_ID = viewModel.GenderID.ToString()
            };

            String studentId = viewModel.FirstName.Substring(0, 2);
            studentId += "-";
            studentId += viewModel.LastName.Substring(0, 3);
            studentId += "-";
            studentId += viewModel.Date.ToString("yyyyMMdd");

            child.ELD_ID = studentId;

            var returnChild = _context.Children.Add(child);
            _context.SaveChanges();

            return RedirectToAction("LangRace", new { id = returnChild.ID});
        }

        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var child = _context.Children.Single(a => a.ID == id);
            if (child == null)
            {
                return HttpNotFound();
            }

            var viewModel = new ChildDetailsViewModel();
            viewModel.ID = child.ID;
            viewModel.LastName = child.LastName;
            viewModel.FirstName = child.FirstName;
            viewModel.GenerationCodeID = child.GenerationCode_ID.GetValueOrDefault();
            viewModel.MiddleName = child.MiddleName;
            viewModel.OtherMiddleName = child.OtherMiddleName;
            viewModel.OtherLastName = child.OtherLastName;
            viewModel.Birthdate = child.Birthdate.GetValueOrDefault();
            viewModel.GenderID = child.Gender_ID[0];

            viewModel.Child_IFSP = _context.Child_IFSPs.Where(c => c.ChildID == child.ID).ToList();
            viewModel.Child_Special_Needs = _context.Child_Special_Needs.Where(c => c.ChildID == id).ToList();

            viewModel.Languages = _context.CodeLanguage.ToList();
            viewModel.RaceEthnicityList = _context.RaceEthnic.ToList();
            viewModel.Generations = _context.Code_GenerationCode.ToList();

            viewModel.CanEdit = UserCanUpdateChild( child.ID);

            return View(viewModel);
        }

        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var child = _context.Children.Single(a => a.ID == id);
            if (child == null)
            {
                return HttpNotFound();
            }

            var viewModel = new ChildFormViewModel
            {
                LastName = child.LastName,
                OtherLastName = child.OtherLastName,
                FirstName = child.FirstName,
                MiddleName = child.MiddleName,
                OtherMiddleName = child.OtherMiddleName,
                Date = child.Birthdate.GetValueOrDefault(),
                GenerationCodeID = child.GenerationCode_ID.GetValueOrDefault(),
                LanguageID = child.Language_ID.GetValueOrDefault(),
                GenderID = child.Gender_ID.ToCharArray()[0],

                Genders = _context.CodeGender.ToList(),
                Languages = _context.CodeLanguage.ToList(),
                RaceEthnicityList = _context.RaceEthnic.ToList(),
                Generations = _context.Code_GenerationCode.ToList()
            };

            return View(viewModel);
        }

        [Authorize]
        [HttpGet]
        public ActionResult UpdateIFSP(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var child = _context.Children.Single(a => a.ID == id);
            if (child == null)
            {
                return HttpNotFound();
            }

            var viewModel = new ChildSpecialViewModel
            {
                ChildID = child.ID,
                Child = child,

                IFSPs = _context.Code_IFSPs.ToList(),
                Special_Needs = _context.SpecialNeeds.ToList()
            };

            viewModel.MyIFSP = new Dictionary<int, bool>();
            var IFSPset = _context.Child_IFSPs.Where(c => c.ChildID == child.ID);
            foreach (Code_IFSP IFSP in viewModel.IFSPs)
            {
                if (IFSPset.Select(t => t.IFSP_Code).Contains(IFSP.Code))
                    viewModel.MyIFSP.Add(IFSP.Code, true);
                else
                    viewModel.MyIFSP.Add(IFSP.Code, false);
            }

            viewModel.MySpecialNeeds = new Dictionary<int, bool>();
            var specialSet = _context.Child_Special_Needs.Where(c => c.ChildID == child.ID);
            foreach (Special_Needs SpecialNeed in viewModel.Special_Needs)
            {
                if (specialSet.Select(t => t.SpecialNeedsCode).Contains( SpecialNeed.Code))
                    viewModel.MySpecialNeeds.Add(SpecialNeed.Code, true);
                else
                    viewModel.MySpecialNeeds.Add(SpecialNeed.Code, false);
            }

            return View(viewModel);
        }

        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var child = _context.Children.Single(a => a.ID == id);
            if (child == null)
            {
                return HttpNotFound();
            }
            return View(child);
        }

        [Authorize]
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult ConfirmDelete(int id)
        {
            var child = _context.Children.Single(a => a.ID == id);
            _context.Children.Remove(child);
            _context.SaveChanges();
            return RedirectToAction("Index", "Child");
        }

        [HttpGet]
        public ActionResult LangRace(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var child = _context.Children.Single(a => a.ID == id);
            if (child == null)
            {
                return HttpNotFound();
            }
            var viewModel = new ChildFormViewModel
            {
                ID = id,
                FirstName = child.FirstName,
                LastName = child.LastName,
                Languages = _context.CodeLanguage.ToList(),
                RaceEthnicityList = _context.RaceEthnic.ToList(),
                RaceDictionary = RaceBoolDictionary
            };
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult LangRace(ChildFormViewModel viewModel)
        {
            foreach (var raceId in viewModel.RaceDictionary.Keys)
            {
                //var truth = false;
                //viewModel.RaceDictionary.TryGetValue(raceId, out truth);
                if (viewModel.RaceDictionary[raceId])
                {
                    var ChildRace = new ChildRace
                    {
                        ChildID = viewModel.ID,
                        RaceID = raceId
                    };
                    _context.ChildRaces.Add(ChildRace);
                }
            }
            
            _context.SaveChanges();
            return RedirectToAction("Create", "Adult");
        }

        //[HttpGet]
        public JsonResult getClassrooms(int id)
        {
            var classrooms = _context.Classrooms
                .Where(c => c.Facility_ID == id)
                .Select(c => new {
                    ID =c.ID,
                    Description = c.Description
                })
                .ToList();
            return Json(classrooms, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Authorize]
        public ActionResult Enroll(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var child = _context.Children.Single(a => a.ID == id);
            if (child == null)
            {
                return HttpNotFound();
            }

            var viewModel = new ChildEnrollViewModel
            {
                Child = child,
                ChildID = child.ID
            };

            viewModel.Children = GetUserChildren();
            viewModel.Services = GetUserServices();
            viewModel.Sites = GetUserSites();
            viewModel.Classrooms = _context.Classrooms;

            return View(viewModel);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Enroll(ChildFormViewModel viewModel)
        {
            foreach (var raceId in viewModel.RaceDictionary.Keys)
            {
                //var truth = false;
                //viewModel.RaceDictionary.TryGetValue(raceId, out truth);
                if (viewModel.RaceDictionary[raceId])
                {
                    var ChildRace = new ChildRace
                    {
                        ChildID = viewModel.ID,
                        RaceID = raceId
                    };
                    _context.ChildRaces.Add(ChildRace);
                }
            }

            _context.SaveChanges();
            return RedirectToAction("Index", "Child");
        }

        [HttpPost]
        [Authorize]
        public ActionResult Search(ChildrenListViewModel viewModel)
        {
            return RedirectToAction("Index", "Child", new { query = viewModel.SearchTerm });
        }

        [HttpGet]
        [Authorize]
        public ActionResult Index(string query = null)
        {
            var viewModel = new ChildrenListViewModel();

            viewModel.Children = GetUserChildren();

            if (!String.IsNullOrWhiteSpace(query))
            {
                var querylow = query.ToLower();
                var queryFilter = viewModel.Children.Where(i =>
                                            (i.FirstName.ToLower() ?? "").Contains(querylow) ||
                                            (i.LastName.ToLower() ?? "").Contains(querylow) ||
                                            ((i.FirstName + " " +i.LastName).ToLower() ?? "").Contains(querylow) ||
                                            (i.ELD_ID ?? "").Contains(querylow)
                                            );

                viewModel.Children = queryFilter.ToList();
                viewModel.SearchTerm = query;
            }

            if (User.IsInRole("Administrator") || User.IsInRole("System Administrator"))
            {
                viewModel.CanAdd = true;
                viewModel.CanEdit = true;
                viewModel.CanDelete = true;
            }
            return View(viewModel);
        }

        /**
         * Return true if Child belongsd to facility/HUB for User or User is admin
         **/
        private bool UserCanUpdateChild(int childID)
        {
            if ( (User.IsInRole("System Administrator") || User.IsInRole("Administrator")))
            {
                return true;
            }
            var children = GetUserChildren();
            if (children.Select(c => c.ID).Contains(childID))
            {
                return true;
            }
            return false;
        }


        /**
         * Retrieve Identity Claim for user for 'Institution' which holds an ID in its value field
         **/
        private int GetUserInstitutionID()
        {
            ClaimsIdentity identity = (ClaimsIdentity)User.Identity;

            var claims = (from c in identity.Claims
                          where c.Type == "Institution"
                          select c);
            int institutionId = Int32.Parse(claims.FirstOrDefault().Value);

            return institutionId;
        }


        /**
         * Return an IQueryable of Children that the User has rights to see.
         * If the user is an administrator, return all children.
         * Otherwise return the children at sites that belong to the user's Provider or HUB
         * 
         * IQueryable is lighter on memory than IEnumerable using this strategy
         * https://stackoverflow.com/questions/2876616/returning-ienumerablet-vs-iqueryablet
         **/
        private IQueryable<Child> GetUserChildren()
        {
            var children = _context.Children.AsQueryable();

            if (!(User.IsInRole("System Administrator") || User.IsInRole("Administrator")))
            {
                int institutionId = GetUserInstitutionID();
                var institution = _context.Institutions.SingleOrDefault(i => i.Id == institutionId);

                if (institution.IsHub)
                {
                    var providers = _context.Institutions.Where(i => i.ParentHubId == institutionId).Select(p => p.Id);
                    
                    //should assume children not enrolled, retrieve by column facility_id in child table
                    var childFacilities = _context.ChildFacilities.Where(f => providers.Contains(f.Facility.ProviderID)).Select(c => c.ChildID);
                    children = children.Where(c => childFacilities.Contains(c.ID));
                }
                if (institution.IsProvider)
                {
                    var childFacilities = _context.ChildFacilities.Where(f => f.Facility.ProviderID == institutionId).Select(c => c.ChildID);
                    children = children.Where(c => childFacilities.Contains(c.ID));
                }
            }
            return children;
        }


        /**
         * Return 'Services' (the Class session or school year model) that a user can see/administrate in order to enroll a child
         **/
        private IQueryable<Service> GetUserServices()
        {
            var services = _context.Services.AsQueryable();

            if (!(User.IsInRole("System Administrator") || User.IsInRole("Administrator")))
            {
                int institutionId = GetUserInstitutionID();
                var institution = _context.Institutions.SingleOrDefault(i => i.Id == institutionId);

                if (institution.IsHub)
                {
                    var providerids = _context.Institutions.Where(i => i.ParentHubId == institutionId).Select(p => p.Id);

                    var facilityids = _context.Facilities.Where(f => providerids.Contains(f.ProviderID)).Select(f => f.ID);
                    var classrooms = _context.Classrooms.Where(c => facilityids.Contains( c.Facility_ID.GetValueOrDefault())).Select(c => c.ID);
                    services = services.Where(s => classrooms.Contains(s.ID));
                }
                if (institution.IsProvider)
                {
                    var childFacilities = _context.ChildFacilities.Where(f => f.Facility.ProviderID == institutionId).Select(c => c.ChildID);
                    services = services.Where(s => childFacilities.Contains(s.ID));
                }
            }
            return services;
        }

        /**
         * Return Sites (Facility) that a user can see details about or see the Children enrolled
         **/
        private IQueryable<Facility> GetUserSites()
        {
            var sites = _context.Facilities.AsQueryable();

            if (!(User.IsInRole("System Administrator") || User.IsInRole("Administrator")))
            {
                int institutionId = GetUserInstitutionID();
                var institution = _context.Institutions.SingleOrDefault(i => i.Id == institutionId);

                if (institution.IsHub)
                {
                    var providerids = _context.Institutions.Where(i => i.ParentHubId == institutionId).Select(p => p.Id);

                    sites = _context.Facilities.Where(f => providerids.Contains(f.ProviderID));
                }
                if (institution.IsProvider)
                {
                    sites = _context.Facilities.Where(f => f.ProviderID == institutionId);
                }
            }
            return sites;
        }
    }
}