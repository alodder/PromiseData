using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PromiseData.Models;
using PromiseData.ViewModels;
using System.Net;
using Microsoft.AspNet.Identity;
using System.Security.Claims;
using System.Web.Services;
using PromiseData.Repositories;

namespace PromiseData.Controllers
{
    public class ChildController : Controller
    {
        private ApplicationDbContext _context;
        private ChildRepository _childRepository;
        private ServicesRepository _servicesRepository;
        private SitesRepository _sitesRepository;
        private ClassroomRepository _classroomRepository;
        private Dictionary<int, bool> RaceBoolDictionary;

        public ChildController()
        {
            _context = new ApplicationDbContext();
            _childRepository = new ChildRepository( _context);
            _servicesRepository = new ServicesRepository(_context);
            _sitesRepository = new SitesRepository(_context);
            _classroomRepository = new ClassroomRepository(_context);

            RaceBoolDictionary = new Dictionary<int, bool>();
            var raceList = _context.RaceEthnic.ToList();
            foreach (RaceEthnicity race in raceList)
            {
                RaceBoolDictionary.Add(race.Id, false);
            }
        }


        /**
         * Create Child form
         * Possibbly add Classroom here?
         */
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

            return RedirectToAction("Race", new { id = returnChild.ID});
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
            viewModel.Address_ID = child.Address_ID;

            viewModel.Address = _context.Addresses.SingleOrDefault( a => a.ID == child.Address_ID);
            viewModel.Generation = _context.Code_GenerationCode.SingleOrDefault(a => a.Code == child.GenerationCode_ID);
            viewModel.OtherNameType = child.OtherNameType;
            viewModel.Gender = _context.CodeGender.SingleOrDefault(a => a.Code == child.Gender_ID);
            viewModel.Language = _context.CodeLanguage.SingleOrDefault(a => a.Code == child.Language_ID);
            viewModel.ProgramSessionType = _context.Code_ProgramSessionType.SingleOrDefault(a => a.Code == child.Program_ID);
            viewModel.ExitReason = _context.Code_ExitReason.SingleOrDefault(a => a.Code == child.Language_ID);

            viewModel.Child_IFSP = _context.Child_IFSPs.Where(c => c.ChildID == child.ID).ToList();
            viewModel.Child_Special_Needs = _context.Child_Special_Needs.Where(c => c.ChildID == child.ID).ToList();

            viewModel.Family = _context.Families.SingleOrDefault( f => f.ID == child.FamilyID);
            viewModel.Adults = _context.Adults.Where( a => a.FamilyID == child.FamilyID);

            viewModel.ChildRaces = _context.ChildRaces.Where( r => r.ChildID == child.ID);

            viewModel.Languages = _context.CodeLanguage.ToList();
            viewModel.RaceEthnicityList = _context.RaceEthnic.ToList();
            viewModel.Generations = _context.Code_GenerationCode.ToList();

            viewModel.CanEdit = _childRepository.UserCanUpdateChild( (ClaimsPrincipal)User, child.ID);

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
            };

            viewModel.MyIFSP = new Dictionary<int, bool>();
            var IFSPset = _context.Child_IFSPs.Where(c => c.ChildID == child.ID).ToList();
            foreach (Code_IFSP IFSP in viewModel.IFSPs)
            {
                if ( IFSPset.Select(t => t.IFSP_Code).Contains(IFSP.Code))
                    viewModel.MyIFSP.Add(IFSP.Code, true);
                else
                    viewModel.MyIFSP.Add(IFSP.Code, false);
            }

            return View(viewModel);
        }

        [Authorize]
        [HttpPost]
        public ActionResult UpdateIFSP(ChildSpecialViewModel viewModel)
        {
            var IFSPset = _context.Child_IFSPs.Where(c => c.ChildID == viewModel.ChildID).ToList();
            foreach (var IFSP in viewModel.MyIFSP.Keys)
            {
                //Create TeacherLanguageClassroom for Teacher and Language pair
                var childsIFSP = new Child_IFSP
                {
                    ChildID = viewModel.ChildID,
                    IFSP_Code = IFSP
                };

                /**
                 * If the language wasn't checked, remove from table,
                 * else if it was both checked and doesn't yet exist, add it
                 */
                if (!viewModel.MyIFSP[IFSP])
                {
                    _context.Child_IFSPs.RemoveRange( IFSPset.Where( tlc => tlc.IFSP_Code == childsIFSP.IFSP_Code));
                }
                else if (viewModel.MyIFSP[IFSP] &&
                        !IFSPset.Select(t => t.IFSP_Code).Contains( childsIFSP.IFSP_Code))//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                {
                    _context.Child_IFSPs.Add( childsIFSP);
                }
            }

            _context.SaveChanges();

            return RedirectToAction("Details", new { id = viewModel.ChildID });
        }

        [Authorize]
        [HttpGet]
        public ActionResult UpdateSpecialNeeds(int? id)
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

                Special_Needs = _context.SpecialNeeds.ToList()
            };

            viewModel.MySpecialNeeds = new Dictionary<int, bool>();
            var specialSet = _context.Child_Special_Needs.Where(c => c.ChildID == child.ID).ToList();
            foreach (Special_Needs SpecialNeed in viewModel.Special_Needs)
            {
                if (specialSet.Select(t => t.SpecialNeedsCode).Contains(SpecialNeed.Code))
                    viewModel.MySpecialNeeds.Add(SpecialNeed.Code, true);
                else
                    viewModel.MySpecialNeeds.Add(SpecialNeed.Code, false);
            }

            return View(viewModel);
        }

        [Authorize]
        [HttpPost]
        public ActionResult UpdateSpecialNeeds(ChildSpecialViewModel viewModel)
        {
            var SpecialNeedsSet = _context.Child_Special_Needs.Where(c => c.ChildID == viewModel.ChildID).ToList();
            foreach (var SpecialNeedCode in viewModel.MySpecialNeeds.Keys)
            {
                //Create TeacherLanguageClassroom for Teacher and Language pair
                var childsSpecial = new Child_Special_Needs
                {
                    ChildID = viewModel.ChildID,
                    SpecialNeedsCode = SpecialNeedCode
                };

                /**
                 * If the language wasn't checked, remove from table,
                 * else if it was both checked and doesn't yet exist, add it
                 */
                if (!viewModel.MySpecialNeeds[SpecialNeedCode])
                {
                    _context.Child_Special_Needs.RemoveRange( SpecialNeedsSet.Where(tlc => tlc.SpecialNeedsCode == childsSpecial.SpecialNeedsCode));
                }
                else if (viewModel.MySpecialNeeds[SpecialNeedCode] &&
                        !SpecialNeedsSet.Select(t => t.SpecialNeedsCode).Contains( childsSpecial.SpecialNeedsCode))
                {
                    _context.Child_Special_Needs.Add( childsSpecial);
                }
            }

            _context.SaveChanges();

            return RedirectToAction("Details", new { id = viewModel.ChildID });
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
        public ActionResult Race(int id)
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
            var viewModel = new ChildRaceViewModel
            {
                ChildID = id,
                Child = child,
                RaceEthnicityList = _context.RaceEthnic.ToList(),
                RaceDictionary = RaceBoolDictionary
            };
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Race(ChildRaceViewModel viewModel)
        {
            foreach (var raceId in viewModel.RaceDictionary.Keys)
            {
                //var truth = false;
                //viewModel.RaceDictionary.TryGetValue(raceId, out truth);
                if (viewModel.RaceDictionary[raceId])
                {
                    var ChildRace = new ChildRace
                    {
                        ChildID = viewModel.ChildID,
                        RaceID = raceId
                    };
                    _context.ChildRaces.Add(ChildRace);
                }
            }
            
            _context.SaveChanges();
            return RedirectToAction("Create", "Adult");
        }

        [HttpGet]
        public ActionResult EditRace(int id)
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
            var viewModel = new ChildRaceViewModel
            {
                ChildID = id,
                Child = child,
                RaceEthnicityList = _context.RaceEthnic.ToList(),
                RaceDictionary = RaceBoolDictionary,
                Update = true
            };

            var childRaces = _context.ChildRaces.Where(r => r.ChildID == child.ID).Select(r => r.RaceID);

            foreach(var def in viewModel.RaceDictionary.ToList())
            {
                if (childRaces.Contains(def.Key))
                    viewModel.RaceDictionary[def.Key] = true;
            }

            return View("Race", viewModel);
        }

        [HttpPost]
        public ActionResult UpdateRace(ChildRaceViewModel viewModel)
        {
            var childRaces = _context.ChildRaces.Where(r => r.ChildID == viewModel.ChildID);
            foreach (var raceId in viewModel.RaceDictionary.Keys)
            {
                var ChildRace = new ChildRace
                    {
                        ChildID = viewModel.ChildID,
                        RaceID = raceId
                    };

                if (viewModel.RaceDictionary[raceId] && !childRaces.Select(r => r.RaceID).Contains(raceId))
                {
                    _context.ChildRaces.Add( ChildRace);
                }
                if (!viewModel.RaceDictionary[raceId] && childRaces.Select(r => r.RaceID).Contains(raceId))
                {
                    _context.ChildRaces.RemoveRange(childRaces.Where( r=> r.RaceID == raceId));
                }

            }

            _context.SaveChanges();
            return RedirectToAction("Details", "Child", new { id = viewModel.ChildID });
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

        [WebMethod]
        public string confirmRemoveAdult(int id)
        {
            var adult = _context.Adults.Single(a => a.ID == id);
            string response = "Are you sure you want to remove ";
            response += adult.NameFirst + " " + adult.NameLast;
            response += " from this child's family?";
            return response;
        }

        [WebMethod]
        public int removeAdult(int id)
        {
            var adult = _context.Adults.Single(a => a.ID == id);
            adult.FamilyID = null;
            var success = _context.SaveChanges();
            return success;
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

            viewModel.Children = _childRepository.GetUserChildren( (ClaimsPrincipal)User);
            viewModel.Services = _servicesRepository.GetUserServices( (ClaimsPrincipal)User);
            viewModel.Sites = _sitesRepository.GetUserSites( (ClaimsPrincipal)User);
            viewModel.Classrooms = _classroomRepository.GetUserClassrooms((ClaimsPrincipal)User);

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

            viewModel.Children = _childRepository.GetUserChildren( (ClaimsPrincipal)User);

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
    }
}