using System;
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
            
            return View(child);
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

            ClaimsIdentity identity = (ClaimsIdentity)User.Identity;

            var claims = (from c in identity.Claims
                          where c.Type == "Institution"
                          select c);

            viewModel.Children = _context.Children.ToList();

            if (!(User.IsInRole("System Administrator") || User.IsInRole("Administrator")))
            {
                int institutionId = Int32.Parse(claims.FirstOrDefault().Value);
                var institution = _context.Institutions.SingleOrDefault(i => i.Id == institutionId);

                if (institution.IsHub)
                {
                    var providers = _context.Institutions.Where(i => i.ParentHubId == institutionId).Select(p => p.Id);
                    
                    var childFacilities = _context.ChildFacilities.Where(f => providers.Contains(f.Facility.ProviderID)).Select(c => c.ChildID);
                    viewModel.Children = viewModel.Children.Where(c => childFacilities.Contains( c.ID));
                }
                if (institution.IsProvider)
                {
                    var childFacilities = _context.ChildFacilities.Where(f => f.Facility.ProviderID == institutionId).Select(c => c.ChildID);
                    viewModel.Children = viewModel.Children.Where(c => childFacilities.Contains(c.ID));
                }
            }

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