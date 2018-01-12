using PromiseData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PromiseData.Controllers
{
    public class ServiceController : Controller
    {
        private ApplicationDbContext _context;
        private List<String> EnrollmentUnits;

        public ServiceController()
        {
            _context = new ApplicationDbContext();

            EnrollmentUnits = new List<string>();
            EnrollmentUnits.Add("Hours");
            EnrollmentUnits.Add("Days");
        }

        [Authorize]
        [HttpGet]
        public ActionResult Create(int? id)
        {
            Service service = new Service();
            service.ClassroomId = id;
            return View( service);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Create(Service viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("Create", viewModel);
            }


            var service = new Service
            {
                Early_Childhood_Services_Received = viewModel.Early_Childhood_Services_Received,
                PP_Program_Enrollment_Year_Start_Date = DateTime.Parse(Convert.ToString(viewModel.PP_Program_Enrollment_Year_Start_Date)),
                Expected_Annual_Attendance_Days = viewModel.Expected_Annual_Attendance_Days,
                Class_Session_Attendance_Quantity = viewModel.Class_Session_Attendance_Quantity,
                Class_Session_Attendance_Units = viewModel.Class_Session_Attendance_Units,
                PP_Exit_Date = DateTime.Parse(Convert.ToString(viewModel.PP_Exit_Date)),
                ClassroomId = viewModel.ClassroomId
            };

            var returnService = _context.Services.Add(service);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        // GET: Services
        public ActionResult Index()
        {
            var ViewModel = _context.Services.ToList();
            return View(ViewModel);
        }
    }
}