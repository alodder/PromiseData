using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PromiseData.Models;
using PromiseData.ViewModels;

namespace PromiseData.Controllers
{
    public class ChildController : Controller
    {
        private ApplicationDbContext _context;
        private Dictionary<RaceEthnicity, bool> raceDictionary;

        public ChildController()
        {
            _context = new ApplicationDbContext();
            raceDictionary = new Dictionary<RaceEthnicity, bool>();
            var raceList = _context.RaceEthnic.ToList();
            foreach (RaceEthnicity race in raceList)
            {
                raceDictionary.Add(race, false);
            }
        }

        [Authorize]
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
                Birthdate = DateTime.Parse(Convert.ToString(viewModel.Date)),
                GenerationCode_ID = viewModel.GenerationCodeID,
                Gender_ID = viewModel.GenderID
            };

            _context.Children.Add(child);
            _context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public ActionResult Details(int id)
        {
            var address = _context.Children.Single(a => a.ID == id);
            return View(address);
        }

        [Authorize]
        public ActionResult Delete(int id)
        {
            var child = _context.Children.Single(a => a.ID == id);
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
        public ActionResult Associate(int id)
        {
            var ChildRace = new ChildRaceViewModel
            {
                raceDictionary = this.raceDictionary
            };
            return View(ChildRace);
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