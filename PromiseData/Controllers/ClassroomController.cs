using PromiseData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PromiseData.ViewModels;
using System.Net;
using System.Security.Claims;
using PromiseData.Repositories;

namespace PromiseData.Controllers
{
    public class ClassroomController : Controller
    {
        private ApplicationDbContext _context;
        private ClassroomRepository _classroomRepository;
        private Dictionary<int, bool> CurriculaDictionary;
        private Dictionary<int, bool> AssessmentsDictionary;
        private Dictionary<int, bool> ScreeningsDictionary;

        public ClassroomController()
        {
            _context = new ApplicationDbContext();
            _classroomRepository = new ClassroomRepository( _context);

            BuildCurriculaDictionary();
            BuildAssessmentsDictionary();
            BuildScreeningsDictionary();
        }

        private void BuildCurriculaDictionary()
        {
            CurriculaDictionary = new Dictionary<int, bool>();
            var curriculaList = _context.Curricula.ToList();
            foreach (Curricula curricula in curriculaList)
            {
                CurriculaDictionary.Add(curricula.Code, false);
            }
        }

        private void BuildAssessmentsDictionary()
        {
            AssessmentsDictionary = new Dictionary<int, bool>();
            var assessmentList = _context.AssessmentTools.ToList();
            foreach (AssessmentTools assessment in assessmentList)
            {
                AssessmentsDictionary.Add(assessment.Code, false);
            }
        }

        private void BuildScreeningsDictionary()
        {
            ScreeningsDictionary = new Dictionary<int, bool>();
            var screeningList = _context.ScreeningTools.ToList();
            foreach (ScreeningTools screening in screeningList)
            {
                ScreeningsDictionary.Add(screening.Code, false);
            }
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
                Facility_ID = facility.ID,
                Curricula = _context.Curricula,
                ClassroomCurricula = CurriculaDictionary,
                AssessmentTools = _context.AssessmentTools,
                ClassroomAssessments = AssessmentsDictionary,
                ScreeningTools = _context.ScreeningTools,
                ClassroomScreenings = ScreeningsDictionary
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
                //ProgramSessionType_ID = viewModel.Program_ID,
                NewOrExpandedClass = viewModel.NewOrExpandedClass,
                SessionHours = viewModel.SessionHours,
                SessionWeeks = viewModel.SessionWeeks,
                PPStudents = viewModel.PPStudents,
                NonPPStudentsHSOPK = viewModel.NonPPStudentsHSOPK,
                NonPPStudentsThirdParty = viewModel.NonPPStudentsThirdParty,
                NonPPStudentsParentPay = viewModel.NonPPStudentsParentPay,
                PPSlotsUnfilled = viewModel.PPSlotsUnfilled,
                upsize_ts = viewModel.upsize_ts,
                Description = viewModel.Description
            };

            _context.Classrooms.Add(classroom);
            _context.SaveChanges();

            //Add Curricula to ClassroomCurricula table
            foreach (var curriculaCode in viewModel.ClassroomCurricula.Keys)
            {
                if (viewModel.ClassroomCurricula[curriculaCode])
                    _context.ClassroomCurricula.Add(new ClassroomCurricula
                    {
                        ClassroomID = classroom.ID,
                        CurriculaCode = curriculaCode
                    });
            }

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
                PPSlots = classroom.PPSlots.GetValueOrDefault(),
                Capacity = classroom.Capacity.GetValueOrDefault(),
                PPStudents = classroom.PPStudents.GetValueOrDefault(),
                NonPPStudentsHSOPK = classroom.NonPPStudentsHSOPK.GetValueOrDefault(),
                NonPPStudentsThirdParty = classroom.NonPPStudentsThirdParty.GetValueOrDefault(),
                NonPPStudentsParentPay = classroom.NonPPStudentsParentPay.GetValueOrDefault(),
                PPSlotsUnfilled = classroom.PPSlotsUnfilled.GetValueOrDefault(),
                upsize_ts = classroom.upsize_ts,
                Description = classroom.Description,
                Curricula = _context.Curricula,
                ClassroomCurricula = CurriculaDictionary,
                AssessmentTools = _context.AssessmentTools,
                ClassroomAssessments = AssessmentsDictionary,
                ScreeningTools = _context.ScreeningTools,
                ClassroomScreenings = ScreeningsDictionary
            };

            var curriculaList = _context.Curricula.ToList();

            viewModel.ClassroomCurricula = new Dictionary<int, bool>();

            var curriculaClassSet = _context.ClassroomCurricula.Where(t => t.ClassroomID == classroom.ID);
            foreach (Curricula curriculum in curriculaList)
            {
                if (curriculaClassSet.Select(t => t.CurriculaCode).Contains(curriculum.Code))
                    viewModel.ClassroomCurricula.Add(curriculum.Code, true);
                else
                    viewModel.ClassroomCurricula.Add(curriculum.Code, false);
            }

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
            classroom.PPSlots = viewModel.PPSlots;
            classroom.Capacity = viewModel.Capacity;
            classroom.PPSlotsUnfilled = viewModel.PPSlotsUnfilled;
            classroom.Description = viewModel.Description;

            UpdateClassCurriculum(classroom.ID, viewModel.ClassroomCurricula, viewModel.CurriculumOther);
            UpdateClassAssessment(classroom.ID, viewModel.ClassroomCurricula, viewModel.AssessmentOther);
            UpdateClassScreening(classroom.ID, viewModel.ClassroomCurricula, viewModel.ScreeningOther);

            _context.SaveChanges();

            return RedirectToAction("Details", "Classroom", new { id = viewModel.ID });
        }

        private void UpdateClassCurriculum(int classroomID, Dictionary<int, bool> ClassroomCurricula, string CurriculumOther)
        {
            ////////////////////////////////////
            //Update curricula in ClassroomCurricula to ClassroomCurricula table
            var curriculaClassSet = _context.ClassroomCurricula.Where(t => t.ClassroomID == classroomID);
            foreach (var curriculumCode in ClassroomCurricula.Keys)
            {
                //Create TeacherLanguageClassroom for Teacher and Language pair
                var classroomCurriculum = new ClassroomCurricula
                {
                    ClassroomID = classroomID,
                    CurriculaCode = curriculumCode,
                    UserDefined = CurriculumOther
                };

                /**
                 * If the curriculum wasn't checked, remove from table,
                 * else if it was both checked and does not yet exist, add it
                 */
                if (!ClassroomCurricula[curriculumCode])
                {
                    _context.ClassroomCurricula.RemoveRange(curriculaClassSet.Where(c => c.CurriculaCode == classroomCurriculum.CurriculaCode));
                }
                else if (ClassroomCurricula[curriculumCode] &&
                        !curriculaClassSet.Select(t => t.CurriculaCode).Contains(classroomCurriculum.CurriculaCode))
                {
                    _context.ClassroomCurricula.Add(classroomCurriculum);
                }
            }
            _context.SaveChanges();
        }

        private void UpdateClassAssessment(int classroomID, Dictionary<int, bool> ClassroomAssessments, string AssessmentOther)
        {
            ////////////////////////////////////
            //Update curricula in ClassroomCurricula to ClassroomCurricula table
            var assessmentClassSet = _context.ClassroomAssessments.Where(t => t.ClassroomID == classroomID);
            foreach (var assessmentCode in ClassroomAssessments.Keys)
            {
                //Create TeacherLanguageClassroom for Teacher and Language pair
                var classroomAssessment = new ClassroomAssessment
                {
                    ClassroomID = classroomID,
                    AssessmentCode = assessmentCode,
                    UserDefined = AssessmentOther
                };

                /**
                 * If the curriculum wasn't checked, remove from table,
                 * else if it was both checked and does not yet exist, add it
                 */
                if (!ClassroomAssessments[assessmentCode])
                {
                    _context.ClassroomAssessments.RemoveRange(assessmentClassSet.Where(c => c.AssessmentCode == classroomAssessment.AssessmentCode));
                }
                else if (ClassroomAssessments[assessmentCode] &&
                        !assessmentClassSet.Select(t => t.AssessmentCode).Contains(classroomAssessment.AssessmentCode))
                {
                    _context.ClassroomAssessments.Add(classroomAssessment);
                }
            }
            _context.SaveChanges();
        }

        private void UpdateClassScreening(int classroomID, Dictionary<int, bool> ClassroomScreenings, string ScreeningOther)
        {
            ////////////////////////////////////
            //Update curricula in ClassroomCurricula to ClassroomCurricula table
            var screeningClassSet = _context.ClassroomScreenings.Where(t => t.ClassroomID == classroomID);
            foreach (var screeningCode in ClassroomScreenings.Keys)
            {
                //Create TeacherLanguageClassroom for Teacher and Language pair
                var classroomScreening = new ClassroomScreening
                {
                    ClassroomID = classroomID,
                    ScreeningCode = screeningCode,
                    UserDefined = ScreeningOther
                };

                /**
                 * If the curriculum wasn't checked, remove from table,
                 * else if it was both checked and does not yet exist, add it
                 */
                if (!ClassroomScreenings[screeningCode])
                {
                    _context.ClassroomScreenings.RemoveRange(screeningClassSet.Where(c => c.ScreeningCode == classroomScreening.ScreeningCode));
                }
                else if (ClassroomScreenings[screeningCode] &&
                        !screeningClassSet.Select(t => t.ScreeningCode).Contains(classroomScreening.ScreeningCode))
                {
                    _context.ClassroomScreenings.Add(classroomScreening);
                }
            }
            _context.SaveChanges();
        }

        [Authorize]
        // GET: Classroom
        public ActionResult Index()
        {
            IEnumerable<Classroom> classrooms = _classroomRepository.GetUserClassrooms( (ClaimsPrincipal)User).ToList();

            return View( classrooms);
        }
    }
}