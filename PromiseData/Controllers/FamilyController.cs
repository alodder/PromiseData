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
    public class FamilyController : Controller
    {
        private ApplicationDbContext _context;

        public FamilyController()
        {
            _context = new ApplicationDbContext();
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View( );
        }

        [HttpPost]
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

            return View( family);
        }



// GET: Family
public ActionResult Index()
        {
            var allTheFamilies = _context.Families;
            return View(allTheFamilies);
        }
    }
}