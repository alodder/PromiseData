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

        // GET: Facility
        public ActionResult Index()
        {
            var viewModel = _context.Facilities;
            return View( viewModel);
        }
    }
}