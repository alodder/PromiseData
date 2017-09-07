using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PromiseData.Models;
using PromiseData.ViewModels;
using System.Net;

namespace PromiseData.Controllers
{
    public class AdultController : Controller
    {
        private ApplicationDbContext _context;
        private List<String> types;
        private List<String> adultTypes;
        private Dictionary<int, bool> RaceBoolDictionary;

        public AdultController()
        {
            _context = new ApplicationDbContext();

            types = new List<string>();
            types.Add("Full-time");
            types.Add("Part-time");
            types.Add("None");

            adultTypes = new List<string>();
            adultTypes.Add("Parent");
            adultTypes.Add("Legal-Guardian");
            adultTypes.Add("Foster Parent");
            adultTypes.Add("Other");

            RaceBoolDictionary = new Dictionary<int, bool>();
            var raceList = _context.RaceEthnic.ToList();
            foreach (RaceEthnicity race in raceList)
            {
                RaceBoolDictionary.Add(race.Id, false);
            }
        }


        //Take FamilyID passed from Child?
        [Authorize]
        public ActionResult Create()
        {
            var viewModel = new AdultFormViewModel
            {
                ParentTypes = adultTypes,
                Genders = _context.CodeGender.ToList(),
                EducationTypes = _context.Code_Education.ToList(),
                TimeTypes = types
            };
            return View(viewModel);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Create( AdultFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Genders = _context.CodeGender.ToList();
                viewModel.EducationTypes = _context.Code_Education.ToList();
                viewModel.TimeTypes = types;
                return View("Create", viewModel);
            }


            var adult = new Adult
            {
                Age = viewModel.Age,
                ResidentialTime = viewModel.ResidentialTime,
                Education_ID = viewModel.Education_ID,
                Employment = viewModel.Employment,
                Gender_ID = viewModel.GenderID
            };

            _context.Adults.Add(adult);
            _context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var adult = _context.Adults.Single(a => a.ID == id);
            if (adult == null)
            {
                return HttpNotFound();
            }
            return View(adult);
        }

        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var adult = _context.Adults.Single(a => a.ID == id);
            if (adult == null)
            {
                return HttpNotFound();
            }
            return View( adult);
        }

        [Authorize]
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult ConfirmDelete(int id)
        {
            var adult = _context.Adults.Single(a => a.ID == id);
            _context.Adults.Remove(adult);
            _context.SaveChanges();
            return RedirectToAction("Index", "Adult");
        }

        [HttpGet]
        public ActionResult LangRace(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var adult = _context.Adults.Single(a => a.ID == id);
            if (adult == null)
            {
                return HttpNotFound();
            }

            var viewModel = new AdultFormViewModel
            {
                id = adult.ID,
                NameFirst = "Anon",
                NameLast = "Jones",
                RaceEthnicityList = _context.RaceEthnic.ToList(),
                RaceDictionary = RaceBoolDictionary
            };
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult LangRace(AdultFormViewModel viewModel)
        {
            foreach (var raceId in viewModel.RaceDictionary.Keys)
            {
                if (viewModel.RaceDictionary[raceId])
                {
                    var adultRace = new AdultRace()
                    {
                        AdultID = viewModel.id,
                        RaceID = raceId
                    };
                    _context.AdultRaces.Add(adultRace);
                }
            }

            _context.SaveChanges();
            return RedirectToAction("Index", "Adult");
        }

        [Authorize]
        // GET: Adult
        public ActionResult Index()
        {
            var viewModel = _context.Adults;
            return View( viewModel);
        }
    }
}