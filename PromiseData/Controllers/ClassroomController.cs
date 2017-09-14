using PromiseData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PromiseData.ViewModels;
using System.Net;
using System.Security.Claims;

namespace PromiseData.Controllers
{
    public class ClassroomController : Controller
    {
        private ApplicationDbContext _context;

        public ClassroomController()
        {
            _context = new ApplicationDbContext();
        }

        [Authorize]
        [HttpGet]
        public ActionResult Create(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var facility = _context.Facilities.Single(a => a.ID == id);
            if (facility == null)
            {
                return HttpNotFound();
            }

            var viewModel = new ClassroomViewModel
            {
                Facilities = _context.Facilities,
                SessionTypes = _context.Code_ProgramSessionType,
                Services = _context.Services,
                Facility_ID = facility.ID
            };
            return View("ClassroomForm", viewModel);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ClassroomViewModel viewModel)
        {

            if (!ModelState.IsValid)
            {
                return View("Create", viewModel);
            }

            var classroom = new Classroom
            {
                Facility_ID = viewModel.Facility_ID,
                Program_ID = viewModel.Program_ID,
                ProgramSessionType_ID = viewModel.Program_ID,
                NewOrExpandedClass = viewModel.NewOrExpandedClass,
                SessionHours = viewModel.SessionHours,
                SessionWeeks = viewModel.SessionWeeks,
                PPStudents = viewModel.PPStudents,
                NonPPStudentsHSOPK = viewModel.NonPPStudentsHSOPK,
                NonPPStudentsThirdParty = viewModel.NonPPStudentsThirdParty,
                NonPPStudentsParentPay = viewModel.NonPPStudentsParentPay,
                PPSlotsUnfilled = viewModel.PPSlotsUnfilled,
                CLASSScore_EmotionalSupport = viewModel.CLASSScore_EmotionalSupport,
                CLASSScore_ClassroomOrganization = viewModel.CLASSScore_ClassroomOrganization,
                CLASSScore_InstructionalSupport = viewModel.CLASSScore_InstructionalSupport,
                upsize_ts = viewModel.upsize_ts,
                Description = viewModel.Description
            };

            _context.Classrooms.Add(classroom);
            _context.SaveChanges();

            return RedirectToAction("Index", "Classroom");
        }

        [Authorize]
        public ActionResult Details(int id)
        {
            var classroom = _context.Classrooms.Single(a => a.ID == id);
            return View(classroom);
        }

        [Authorize]
        public ActionResult Delete(int id)
        {
            var classroom = _context.Classrooms.Single(a => a.ID == id);
            var viewModel = new Classroom
            {
                ID = classroom.ID,
                Facility_ID = classroom.Facility_ID,
                Program_ID = classroom.Program_ID,
                ProgramSessionType_ID = classroom.Program_ID,
                NewOrExpandedClass = classroom.NewOrExpandedClass,
                SessionHours = classroom.SessionHours,
                SessionWeeks = classroom.SessionWeeks,
                PPStudents = classroom.PPStudents,
                NonPPStudentsHSOPK = classroom.NonPPStudentsHSOPK,
                NonPPStudentsThirdParty = classroom.NonPPStudentsThirdParty,
                NonPPStudentsParentPay = classroom.NonPPStudentsParentPay,
                PPSlotsUnfilled = classroom.PPSlotsUnfilled,
                CLASSScore_EmotionalSupport = classroom.CLASSScore_EmotionalSupport,
                CLASSScore_ClassroomOrganization = classroom.CLASSScore_ClassroomOrganization,
                CLASSScore_InstructionalSupport = classroom.CLASSScore_InstructionalSupport,
                upsize_ts = classroom.upsize_ts,
                Description = classroom.Description
            };
            return View( viewModel);
        }

        [Authorize]
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult ConfirmDelete(int id)
        {
            var classroom = _context.Classrooms.Single(a => a.ID == id);
            _context.Classrooms.Remove(classroom);
            _context.SaveChanges();
            return RedirectToAction("Index", "Classroom");
        }

        // GET: Institution
        [HttpGet]
        [Authorize(Roles = "Administrator, System Administrator")]
        public ActionResult Edit(int id)
        {
            var classroom = _context.Classrooms.Single(a => a.ID == id);

            var viewModel = new ClassroomViewModel
            {
                ID = classroom.ID,
                Facilities = _context.Facilities,
                SessionTypes = _context.Code_ProgramSessionType,
                Services = _context.Services,
                Facility_ID = classroom.Facility_ID.GetValueOrDefault(),
                Program_ID = classroom.Program_ID.GetValueOrDefault(),
                ProgramSessionType_ID = classroom.Program_ID.GetValueOrDefault(),
                NewOrExpandedClass = classroom.NewOrExpandedClass,
                SessionHours = classroom.SessionHours.GetValueOrDefault(),
                SessionWeeks = classroom.SessionWeeks.GetValueOrDefault(),
                PPStudents = classroom.PPStudents.GetValueOrDefault(),
                NonPPStudentsHSOPK = classroom.NonPPStudentsHSOPK.GetValueOrDefault(),
                NonPPStudentsThirdParty = classroom.NonPPStudentsThirdParty.GetValueOrDefault(),
                NonPPStudentsParentPay = classroom.NonPPStudentsParentPay.GetValueOrDefault(),
                PPSlotsUnfilled = classroom.PPSlotsUnfilled.GetValueOrDefault(),
                CLASSScore_EmotionalSupport = classroom.CLASSScore_EmotionalSupport.GetValueOrDefault(),
                CLASSScore_ClassroomOrganization = classroom.CLASSScore_ClassroomOrganization.GetValueOrDefault(),
                CLASSScore_InstructionalSupport = classroom.CLASSScore_InstructionalSupport.GetValueOrDefault(),
                upsize_ts = classroom.upsize_ts,
                Description = classroom.Description
            };

            return View("ClassroomForm", viewModel);
        }

        //POST: Institution update/edit
        [HttpPost]
        [Authorize(Roles = "Administrator, System Administrator")]
        public ActionResult Update(ClassroomViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Facilities = _context.Facilities;
                viewModel.SessionTypes = _context.Code_ProgramSessionType;
                viewModel.Services = _context.Services;
                return View("ClassroomForm", viewModel);
            }

            var classroom = _context.Classrooms.Single(a => a.ID == viewModel.ID);
            classroom.Facility_ID = viewModel.Facility_ID;
            classroom.Program_ID = viewModel.Program_ID;
            //classroom.ProgramSessionType_ID = viewModel.ProgramSessionType_ID;
            classroom.NewOrExpandedClass = viewModel.NewOrExpandedClass;
            classroom.SessionHours = viewModel.SessionHours;
            classroom.SessionDays = viewModel.SessionDays;
            classroom.SessionHours = viewModel.SessionHours;
            classroom.SessionWeeks = viewModel.SessionWeeks;
            classroom.PPStudents = viewModel.PPStudents;
            classroom.NonPPStudentsHSOPK = viewModel.NonPPStudentsHSOPK;
            classroom.NonPPStudentsThirdParty = viewModel.NonPPStudentsThirdParty;
            classroom.NonPPStudentsParentPay = viewModel.NonPPStudentsParentPay;
            classroom.PPSlotsUnfilled = viewModel.PPSlotsUnfilled;
            classroom.CLASSScore_EmotionalSupport = viewModel.CLASSScore_EmotionalSupport;
            classroom.CLASSScore_ClassroomOrganization = viewModel.CLASSScore_ClassroomOrganization;
            classroom.CLASSScore_InstructionalSupport = viewModel.CLASSScore_InstructionalSupport;
            classroom.Description = viewModel.Description;

            _context.SaveChanges();

            return RedirectToAction("Details", "Facility", new { id = viewModel.Facility_ID });
        }

        [Authorize]
        // GET: Classroom
        public ActionResult Index()
        {
            IEnumerable<Classroom> classrooms;

            if (User.IsInRole("Administrator") || User.IsInRole("System Administrator"))
            {
                classrooms = _context.Classrooms;
            }
            else
            {
                ClaimsIdentity identity = (ClaimsIdentity)User.Identity;

                var claims = (from c in identity.Claims
                              where c.Type == "Institution"
                              select c);
                var institutionId= Int32.Parse(claims.FirstOrDefault().Value);

                classrooms = _context.Classrooms.Where(
                    c => c.Facility.ProviderID == institutionId);
            }

            return View( classrooms);
        }
    }
}