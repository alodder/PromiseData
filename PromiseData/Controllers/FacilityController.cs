using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
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
        private Dictionary<int, bool> SupportBoolDictionary;

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

            SupportBoolDictionary = new Dictionary<int, bool>();
            var SupportList = _context.Code_AdditionalSupportTypes.ToList();
            foreach (Code_AdditionalSupportTypes support in SupportList)
            {
                SupportBoolDictionary.Add(support.Code, false);
            }
        }

        [Authorize]
        [HttpGet]
        public ActionResult Create()
        {
            var viewModel = new FacilityViewModel
            {
                FacilityTypes = this.FacilityTypes,
                SupportTypes = _context.Code_AdditionalSupportTypes,
                SupportDictionary = SupportBoolDictionary
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
                MonitoringVisit2Result = viewModel.MonitoringVisit2Result,
                Description = viewModel.Description
            };

            var facilityId = _context.Facilities.Add(facility).ID;

            foreach (var supportId in viewModel.SupportDictionary.Keys)
            {
                if (viewModel.SupportDictionary[supportId])
                {
                    var facilitySupport = new FacilitySupport()
                    {
                        FacilityID = facilityId,
                        SupportTypesCode = supportId
                    };
                    _context.FacilitySupports.Add(facilitySupport);
                }
            }

            _context.SaveChanges();

            return RedirectToAction("Index", "Facility");
        }

        public ActionResult Details(int id)
        {
            var facility = _context.Facilities.Single(a => a.ID == id);
            var viewModel = new FacilityViewModel
            {
                ID = facility.ID,
                ProviderFacilityType = facility.ProviderFacilityType,
                Turnover_NonPPStaff = facility.Turnover_NonPPStaff,
                TurnoverReasons_NonPPStaff = facility.TurnoverReasons_NonPPStaff,
                Transportation_services_offered = facility.Transportation_services_offered,
                ChildrenReceivingTransportationServices = facility.ChildrenReceivingTransportationServices.GetValueOrDefault(),
                AdditionalChildFamilySupports_ID = facility.AdditionalChildFamilySupports_ID.GetValueOrDefault(),
                MonitoringVisit1Date = DateTime.Parse(Convert.ToString(facility.MonitoringVisit1Date)),
                MonitoringVisit1Result = facility.MonitoringVisit1Result,
                MonitoringVisit2Date = DateTime.Parse(Convert.ToString(facility.MonitoringVisit2Date)),
                MonitoringVisit2Result = facility.MonitoringVisit2Result,
                Description = facility.Description
            };
            return View(viewModel);
        }

        [Authorize]
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var facility = _context.Facilities.Single(a => a.ID == id);
            var viewModel = new FacilityViewModel
            {
                ID = facility.ID,
                ProviderFacilityType = facility.ProviderFacilityType,
                Turnover_NonPPStaff = facility.Turnover_NonPPStaff,
                TurnoverReasons_NonPPStaff = facility.TurnoverReasons_NonPPStaff,
                Transportation_services_offered = facility.Transportation_services_offered,
                ChildrenReceivingTransportationServices = facility.ChildrenReceivingTransportationServices.GetValueOrDefault(),
                AdditionalChildFamilySupports_ID = facility.AdditionalChildFamilySupports_ID.GetValueOrDefault(),
                MonitoringVisit1Date = DateTime.Parse(Convert.ToString(facility.MonitoringVisit1Date)),
                MonitoringVisit1Result = facility.MonitoringVisit1Result,
                MonitoringVisit2Date = DateTime.Parse(Convert.ToString(facility.MonitoringVisit2Date)),
                MonitoringVisit2Result = facility.MonitoringVisit2Result,
                Description = facility.Description
            };
            return View(viewModel);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Edit(FacilityViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.FacilityTypes = this.FacilityTypes;
                viewModel.SupportTypes = _context.Code_AdditionalSupportTypes.ToList();
                return View("Create", viewModel);
            }

            var facility = new Facility
            {
                ID = viewModel.ID,
                ProviderFacilityType = viewModel.ProviderFacilityType,
                Turnover_NonPPStaff = viewModel.Turnover_NonPPStaff,
                TurnoverReasons_NonPPStaff = viewModel.TurnoverReasons_NonPPStaff,
                Transportation_services_offered = viewModel.Transportation_services_offered,
                ChildrenReceivingTransportationServices = viewModel.ChildrenReceivingTransportationServices,
                AdditionalChildFamilySupports_ID = viewModel.AdditionalChildFamilySupports_ID,
                MonitoringVisit1Date = DateTime.Parse(Convert.ToString(viewModel.MonitoringVisit1Date)),
                MonitoringVisit1Result = viewModel.MonitoringVisit1Result,
                MonitoringVisit2Date = DateTime.Parse(Convert.ToString(viewModel.MonitoringVisit2Date)),
                MonitoringVisit2Result = viewModel.MonitoringVisit2Result,
                Description = viewModel.Description
            };
            TryUpdateModel(facility);
            _context.SaveChanges();

            return RedirectToAction("Index", "Facility");
        }

        [Authorize]
        public ActionResult Delete(int id)
        {
            var facility = _context.Facilities.Single(a => a.ID == id);
            var viewModel = new FacilityViewModel
            {
                ID = facility.ID,
                ProviderFacilityType = facility.ProviderFacilityType,
                Turnover_NonPPStaff = facility.Turnover_NonPPStaff,
                TurnoverReasons_NonPPStaff = facility.TurnoverReasons_NonPPStaff,
                Transportation_services_offered = facility.Transportation_services_offered,
                ChildrenReceivingTransportationServices = facility.ChildrenReceivingTransportationServices.GetValueOrDefault(),
                AdditionalChildFamilySupports_ID = facility.AdditionalChildFamilySupports_ID.GetValueOrDefault(),
                MonitoringVisit1Date = DateTime.Parse(Convert.ToString(facility.MonitoringVisit1Date)),
                MonitoringVisit1Result = facility.MonitoringVisit1Result,
                MonitoringVisit2Date = DateTime.Parse(Convert.ToString(facility.MonitoringVisit2Date)),
                MonitoringVisit2Result = facility.MonitoringVisit2Result
            };
            return View(viewModel);
        }

        [Authorize]
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult ConfirmDelete(int id)
        {
            var facility = _context.Facilities.Single(a => a.ID == id);
            _context.Facilities.Remove(facility);
            _context.SaveChanges();
            return RedirectToAction("Index", "Facility");
        }

        [Authorize]
        // GET: Facility
        public ActionResult Index()
        {
            var viewModel = _context.Facilities;
            return View( viewModel);
        }
    }
}