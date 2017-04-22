using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PromiseData.Models;
using PromiseData.ViewModels;

namespace PromiseData.Controllers
{
    public class AdultController : Controller
    {
        private ApplicationDbContext _context;
        private List<String> types;

        public AdultController()
        {
            _context = new ApplicationDbContext();

            types = new List<string>();
            types.Add("Full-time");
            types.Add("Part-time");
            types.Add("None");
        }

        public ActionResult Create()
        {
            var viewModel = new AdultFormViewModel
            {
                Genders = _context.CodeGender.ToList(),
                EducationTypes = _context.Code_Education.ToList(),
                TimeTypes = types
            };
            return View(viewModel);
        }

        [HttpPost]
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

        // GET: Adult
        public ActionResult Index()
        {
            return View();
        }
    }
}