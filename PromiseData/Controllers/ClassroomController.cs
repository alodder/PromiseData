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
            return View(viewModel);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Classroom viewModel)
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