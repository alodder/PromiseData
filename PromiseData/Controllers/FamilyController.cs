using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PromiseData.Models;
using PromiseData.ViewModels;
using System.Net;
using Advanced_Auditing.Models;

namespace PromiseData.Controllers
{
    [Authorize]
    [Audit]
    public class FamilyController : Controller
    {
        private ApplicationDbContext _context;

        public FamilyController()
        {
            _context = new ApplicationDbContext();
        }

        [HttpGet]
        [Authorize(Roles = "Administrator, System Administrator, Hub")]
        public ActionResult Create(int? id)
        {
            return View("FamilyForm");
        }

        [HttpPost]
        [Authorize(Roles = "Administrator, System Administrator, Hub")]
        public ActionResult Create(FamilyViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View("Create", viewModel);

            var family = new Family
            {
                HouseholdSize = viewModel.HouseholdSize,
                ChildrenInHome = viewModel.ChildrenInHome,
                Income = viewModel.Income,
                SNAP = viewModel.SNAP,
                WIC = viewModel.WIC,
                TANF = viewModel.TANF,
                SSI = viewModel.SSI,
                MonthlyCostAdditionalServices = viewModel.MonthlyCostAdditionalServices
            };

            var returnFamily = _context.Families.Add(family);
            _context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Authorize(Roles = "Administrator, System Administrator, Hub")]
        public ActionResult Edit( int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var family = _context.Families.Single(a => a.ID == id);
            if (family == null)
            {
                return HttpNotFound();
            }

            FamilyViewModel viewModel = new FamilyViewModel
            {
                ID = family.ID,
                HouseholdSize = family.HouseholdSize.GetValueOrDefault(),
                ChildrenInHome = family.ChildrenInHome.GetValueOrDefault(),
                Income = family.Income.GetValueOrDefault(),
                SNAP = family.SNAP,
                WIC = family.WIC,
                TANF = family.TANF,
                SSI = family.SSI,
                MonthlyCostAdditionalServices = family.MonthlyCostAdditionalServices.GetValueOrDefault()
            };

            return View("FamilyForm", viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator, System Administrator, Hub")]
        public ActionResult Update(FamilyViewModel viewModel)
        {
            var family = _context.Families.Single(a => a.ID == viewModel.ID);
            if (family == null)
            {
                return HttpNotFound();
            }

            family.HouseholdSize = viewModel.HouseholdSize;
            family.ChildrenInHome = viewModel.ChildrenInHome;
            family.Income = viewModel.Income;
            family.SNAP = viewModel.SNAP;
            family.WIC = viewModel.WIC;
            family.TANF = viewModel.TANF;
            family.SSI = viewModel.SSI;

            _context.SaveChanges();

            return RedirectToAction("Family");
        }

        public ActionResult Index()
        {
            var allTheFamilies = _context.Families.ToList();
            return View(allTheFamilies);
        }
    }
}