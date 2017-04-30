using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PromiseData.Models;
using PromiseData.ViewModels;

namespace PromiseData.Controllers
{
    public class FacilityController : Controller
    {
        private ApplicationDbContext _context;
        private List<String> FacilityTypes;

        public FacilityController()
        {
            _context = new ApplicationDbContext();
            FacilityTypes = new List<string>();
            FacilityTypes.Add("Registered Family");
            FacilityTypes.Add("Certified Family");
            FacilityTypes.Add("Certified Center");
            FacilityTypes.Add("Relief Nursery");
            FacilityTypes.Add("Private Preschool");
            FacilityTypes.Add("Public School");
            FacilityTypes.Add("ESD");
            FacilityTypes.Add("CBO w/ preschool");
        }

        [Authorize]
        [HttpGet]
        public ActionResult Create()
        {
            var viewModel = new FacilityViewModel
            {
                FacilityTypes = this.FacilityTypes,
                SupportTypes = _context.Code_AdditionalSupportTypes
            };
            return View(viewModel);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FacilityViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.FacilityTypes = this.FacilityTypes;
                viewModel.SupportTypes = _context.Code_AdditionalSupportTypes.ToList();
                return View("Create", viewModel);
            }

            var facility = new Facility
            {
                ProviderFacilityType = viewModel.ProviderFacilityType,
                Turnover_NonPPStaff = viewModel.Turnover_NonPPStaff,
                TurnoverReasons_NonPPStaff = viewModel.TurnoverReasons_NonPPStaff,
                Transportation_services_offered = viewModel.Transportation_services_offered,
                ChildrenReceivingTransportationServices = viewModel.ChildrenReceivingTransportationServices,
                AdditionalChildFamilySupports_ID = viewModel.AdditionalChildFamilySupports_ID,
                MonitoringVisit1Date = DateTime.Parse(Convert.ToString(viewModel.MonitoringVisit1Date)),
                MonitoringVisit1Result = viewModel.MonitoringVisit1Result,
                MonitoringVisit2Date = DateTime.Parse(Convert.ToString(viewModel.MonitoringVisit2Date)),
                MonitoringVisit2Result = viewModel.MonitoringVisit2Result
            };

            _context.Facilities.Add(facility);
            _context.SaveChanges();

            return RedirectToAction("Index", "Facility");
        }

        // GET: Facility
        public ActionResult Index()
        {
            var viewModel = _context.Facilities;
            return View( viewModel);
        }
    }
}