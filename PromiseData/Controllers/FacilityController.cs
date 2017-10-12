using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PromiseData.Models;
using PromiseData.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Security.Claims;

namespace PromiseData.Controllers
{
    public class FacilityController : Controller
    {
        private IdentityStoreDbContext _IdentityContext;
        private UserManager<ApplicationUser> UserManager;
        private RoleManager<IdentityRole> RoleManager;


        private ApplicationDbContext _context;
        private List<String> FacilityTypes;
        private Dictionary<int, bool> SupportBoolDictionary;

        public FacilityController()
        {
            _IdentityContext = new IdentityStoreDbContext();
            RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new IdentityStoreDbContext()));
            UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_IdentityContext));

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
        public ActionResult Create( int id)
        {
            var viewModel = new FacilityViewModel
            {
                Heading = "New Site",
                FacilityTypes = this.FacilityTypes,
                SupportTypes = _context.Code_AdditionalSupportTypes,
                SupportDictionary = SupportBoolDictionary,
                ProviderId = id
            };

            viewModel.SupportsList = _context.Code_AdditionalSupportTypes.ToList();

            var facilitySupports = _context.FacilitySupports.Select(s => s.SupportTypesCode).ToList();
            //viewModel.Supports = _context.Code_AdditionalSupportTypes.Where( support => facilitySupports.Contains( support.Code));

            //viewModel.SupportsCodeList = new String[viewModel.Supports.ToArray().Length];

            /*int i = 0;
            foreach (FacilitySupport fSupport in viewModel.Supports)
            {
                viewModel.SupportsCodeList[i] = viewModel.Supports.Single(a => a.SupportTypesCode == role.RoleId).Name;
                i++;
            }*/

            return View("FacilityForm", viewModel);
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
                return View("FacilityForm", viewModel);
            }

            var facility = new Facility
            {
                ProviderFacilityType = viewModel.ProviderFacilityType,
                Turnover_NonPPStaff = viewModel.Turnover_NonPPStaff,
                TurnoverReasons_NonPPStaff = viewModel.TurnoverReasons_NonPPStaff,
                Transportation_services_offered = viewModel.Transportation_services_offered,
                ChildrenReceivingTransportationServices = viewModel.ChildrenReceivingTransportationServices,
                AdditionalChildFamilySupports_ID = viewModel.AdditionalChildFamilySupports_ID,
                MonitoringVisit1Date = viewModel.MonitoringVisit1Date,
                MonitoringVisit1Result = viewModel.MonitoringVisit1Result,
                MonitoringVisit2Date = viewModel.MonitoringVisit2Date,
                MonitoringVisit2Result = viewModel.MonitoringVisit2Result,
                Description = viewModel.Description,
                ProviderID = viewModel.ProviderId
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

        [HttpGet]
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
                MonitoringVisit1Date = facility.MonitoringVisit1Date,
                MonitoringVisit1Result = facility.MonitoringVisit1Result,
                MonitoringVisit2Date = facility.MonitoringVisit2Date,
                MonitoringVisit2Result = facility.MonitoringVisit2Result,
                Description = facility.Description
            };

            viewModel.Classrooms = _context.Classrooms.Where(c => c.Facility_ID == id).ToList();
            viewModel.Provider = _context.Institutions.SingleOrDefault(i => i.Id == facility.ProviderID);
            var facilitySupports = _context.FacilitySupports.Where(s=> s.FacilityID == id).Select(s => s.SupportTypesCode).ToList();
            viewModel.Supports = _context.Code_AdditionalSupportTypes.Where(support => facilitySupports.Contains(support.Code)).ToList();

            viewModel.Waivers = facility.WaiverCurrents.ToList();

            /**
             * Set user access to controls
             **/
            if (User.IsInRole("System Administrator") || User.IsInRole("Administrator"))
            {
                viewModel.CanEdit = true;
                viewModel.CanView = true;
            }
            //int institutionId;

            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (User.IsInRole("Hub") 
                && ( identity.Claims.Any(
                    t => t.Type == "Institution" && t.Value == viewModel.Provider.ParentHubId.ToString())))
            {
                viewModel.CanEdit = true;
                viewModel.CanView = true;
            }
            if (User.IsInRole("Provider") && identity.Claims.Any(t => t.Type == "Institution" && t.Value == viewModel.Provider.Id.ToString()))
            {
                viewModel.CanEdit = true;
                viewModel.CanView = true;
            }

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
                Heading = "Update Site",
                ProviderFacilityType = facility.ProviderFacilityType,
                Turnover_NonPPStaff = facility.Turnover_NonPPStaff,
                TurnoverReasons_NonPPStaff = facility.TurnoverReasons_NonPPStaff,
                Transportation_services_offered = facility.Transportation_services_offered,
                ChildrenReceivingTransportationServices = facility.ChildrenReceivingTransportationServices.GetValueOrDefault(),
                AdditionalChildFamilySupports_ID = facility.AdditionalChildFamilySupports_ID.GetValueOrDefault(),
                MonitoringVisit1Date = facility.MonitoringVisit1Date,
                MonitoringVisit1Result = facility.MonitoringVisit1Result,
                MonitoringVisit2Date = facility.MonitoringVisit2Date,
                MonitoringVisit2Result = facility.MonitoringVisit2Result,
                Description = facility.Description,
                FacilityTypes = this.FacilityTypes,
                SupportTypes = _context.Code_AdditionalSupportTypes,
                SupportDictionary = SupportBoolDictionary
            };

            viewModel.SupportsList = _context.Code_AdditionalSupportTypes.ToList();

            viewModel.Classrooms = _context.Classrooms.Where(c => c.ID == id).ToList();
            viewModel.Provider = _context.Institutions.SingleOrDefault(i => i.Id == facility.ProviderID);

            var facilitySupports = _context.FacilitySupports.Where(s => s.FacilityID == id).Select(s => s.SupportTypesCode).ToList();
            viewModel.Supports = _context.Code_AdditionalSupportTypes.Where(support => facilitySupports.Contains(support.Code)).ToList();

            foreach( Code_AdditionalSupportTypes support in viewModel.Supports)
            {
                viewModel.SupportDictionary[support.Code] = true;
            }

            return View("FacilityForm", viewModel);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Update(FacilityViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.FacilityTypes = this.FacilityTypes;
                viewModel.SupportTypes = _context.Code_AdditionalSupportTypes.ToList();
                return View("FacilityForm", viewModel);
            }

            var facility = _context.Facilities.Single(i => i.ID == viewModel.ID);
            facility.ProviderFacilityType = viewModel.ProviderFacilityType;
            facility.Turnover_NonPPStaff = viewModel.Turnover_NonPPStaff;
            facility.TurnoverReasons_NonPPStaff = viewModel.TurnoverReasons_NonPPStaff;
            facility.Transportation_services_offered = viewModel.Transportation_services_offered;
            facility.ChildrenReceivingTransportationServices = viewModel.ChildrenReceivingTransportationServices;
            facility.AdditionalChildFamilySupports_ID = viewModel.AdditionalChildFamilySupports_ID;
            facility.MonitoringVisit1Date = viewModel.MonitoringVisit1Date;
            facility.MonitoringVisit1Result = viewModel.MonitoringVisit1Result;
            facility.MonitoringVisit2Date = viewModel.MonitoringVisit2Date;
            facility.MonitoringVisit2Result = viewModel.MonitoringVisit2Result;
            facility.Description = viewModel.Description;

            _context.FacilitySupports.RemoveRange(_context.FacilitySupports.Where(x => x.FacilityID == facility.ID));

            foreach (var supportId in viewModel.SupportDictionary.Keys)
            {
                if (viewModel.SupportDictionary[supportId])
                {
                    var facilitySupport = new FacilitySupport()
                    {
                        FacilityID = facility.ID,
                        SupportTypesCode = supportId
                    };
                    _context.FacilitySupports.Add(facilitySupport);
                }
            }

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
                MonitoringVisit1Date = facility.MonitoringVisit1Date,
                MonitoringVisit1Result = facility.MonitoringVisit1Result,
                MonitoringVisit2Date = facility.MonitoringVisit2Date,
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