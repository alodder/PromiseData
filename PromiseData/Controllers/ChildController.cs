using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PromiseData.Models;
using PromiseData.ViewModels;
using System.Net;

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
                Gender_ID = viewModel.GenderID
            };

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

        [Authorize]
        // GET: Address
        public ActionResult Index()
        {
            var viewModel = _context.Children;
            return View(viewModel);
        }
    }
}