﻿using PromiseData.Models;
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
        private InstitutionRepository _institutionRepository;
        private ProviderRepository _providerRepository;


        public ClassroomController()
        {
            _context = new ApplicationDbContext();
            _classroomRepository = new ClassroomRepository( _context);
            _institutionRepository = new InstitutionRepository( _context);
            _providerRepository = new ProviderRepository( _context);
        }

        private void BuildCurriculaDictionary(ClassroomViewModel viewModel)
        {
            var curriculaList = _context.Curricula.ToList();
            viewModel.ClassroomCurricula = new Dictionary<int, bool>();
            var curriculaClassSet = _context.ClassroomCurricula.Where(t => t.ClassroomID == viewModel.ID);
            foreach (Curricula curriculum in curriculaList)
            {
                if (curriculaClassSet.Select(t => t.CurriculaCode).Contains(curriculum.Code))
                {
                    viewModel.ClassroomCurricula.Add(curriculum.Code, true);
                    viewModel.CurriculumOther = (curriculum.Code == 1) ? (curriculaClassSet.FirstOrDefault(t => t.CurriculaCode == curriculum.Code).UserDefined) : viewModel.CurriculumOther;
                }
                else
                {
                    viewModel.ClassroomCurricula.Add(curriculum.Code, false);
                }
            }
        }

        private void BuildAssessmentsDictionary( ClassroomViewModel viewModel)
        {
            viewModel.ClassroomAssessments = new Dictionary<int, bool>();
            var assessmentList = _context.AssessmentTools.ToList();
            var assessmentClassSet = _context.ClassroomAssessments.Where(t => t.ClassroomID == viewModel.ID);
            foreach (AssessmentTools assessment in assessmentList)
            {
                if (assessmentClassSet.Select(t => t.AssessmentCode).Contains(assessment.Code))
                {
                    viewModel.ClassroomAssessments.Add(assessment.Code, true);
                    viewModel.AssessmentOther = (assessment.Code == 1) ? (assessmentClassSet.FirstOrDefault(t => t.AssessmentCode == assessment.Code).UserDefined) : viewModel.AssessmentOther;
                }
                else
                {
                    viewModel.ClassroomAssessments.Add(assessment.Code, false);
                }
            }
        }

        private void BuildScreeningsDictionary(ClassroomViewModel viewModel)
        {
            viewModel.ClassroomScreenings = new Dictionary<int, bool>();
            var screeningList = _context.ScreeningTools.ToList();
            var screeningClassSet = _context.ClassroomScreenings.Where(t => t.ClassroomID == viewModel.ID);
            foreach (ScreeningTools screening in screeningList)
            {
                if (screeningClassSet.Select(t => t.ScreeningCode).Contains(screening.Code))
                {
                    viewModel.ClassroomScreenings.Add(screening.Code, true);
                    viewModel.ScreeningOther = (screening.Code == 1) ? (screeningClassSet.FirstOrDefault(t => t.ScreeningCode == screening.Code).UserDefined) : viewModel.ScreeningOther;
                }
                else
                {
                    viewModel.ClassroomScreenings.Add(screening.Code, false);
                }
            }
        }

        [Authorize]
        [HttpGet]
        public ActionResult Create(int? id)
        {
            if ((id == null) && !(User.IsInRole("Administrator") || User.IsInRole("System Administrator")))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var facility = _context.Facilities.Find( id);
            if ((facility == null) && !(User.IsInRole("Administrator") || User.IsInRole("System Administrator")))
            {
                return HttpNotFound();
            }

            var viewModel = new ClassroomViewModel
            {
                Facilities = _providerRepository.GetUserProviders( (ClaimsPrincipal) User),
                Operators = _institutionRepository.GetUserProviders( (ClaimsPrincipal)User),
                SessionTypes = _context.Code_ProgramSessionType,
                Curricula = _context.Curricula,
                AssessmentTools = _context.AssessmentTools,
                ScreeningTools = _context.ScreeningTools
            };

            if(facility == null)
            {
                viewModel.Facility_ID = 0;
            }
            else
            {
                viewModel.Facility_ID = facility.ID;
            }

            BuildCurriculaDictionary( viewModel);
            BuildAssessmentsDictionary( viewModel);
            BuildScreeningsDictionary( viewModel);

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

            UpdateClassCurriculum(classroom.ID, viewModel.ClassroomCurricula, viewModel.CurriculumOther);
            UpdateClassAssessment(classroom.ID, viewModel.ClassroomCurricula, viewModel.AssessmentOther);
            UpdateClassScreening(classroom.ID, viewModel.ClassroomCurricula, viewModel.ScreeningOther);

            return RedirectToAction("Index", "Classroom");
        }

        [Authorize]
        public ActionResult Details(int id)
        {
            var classroom = _context.Classrooms.Find( id);
            return View(classroom);
        }

        [Authorize]
        public ActionResult Delete(int id)
        {
            var classroom = _context.Classrooms.Find( id);
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
            var classroom = _context.Classrooms.Find( id);
            _context.Classrooms.Remove(classroom);
            _context.SaveChanges();
            return RedirectToAction("Index", "Classroom");
        }

        // GET: Institution
        [HttpGet]
        [Authorize(Roles = "Administrator, System Administrator")]
        public ActionResult Edit(int id)
        {
            var classroom = _context.Classrooms.Find( id);

            var viewModel = new ClassroomViewModel
            {
                ID = classroom.ID,
                Facilities = _providerRepository.GetUserProviders( (ClaimsPrincipal) User),
                Operators = _institutionRepository.GetUserProviders( (ClaimsPrincipal)User),
                SessionTypes = _context.Code_ProgramSessionType,
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
                AssessmentTools = _context.AssessmentTools,
                ScreeningTools = _context.ScreeningTools,
            };

            BuildCurriculaDictionary(viewModel);
            BuildAssessmentsDictionary(viewModel);
            BuildScreeningsDictionary(viewModel);

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
                return View("ClassroomForm", viewModel);
            }

            var classroom = _context.Classrooms.Find( viewModel.ID);
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
            UpdateClassAssessment(classroom.ID, viewModel.ClassroomAssessments, viewModel.AssessmentOther);
            UpdateClassScreening(classroom.ID, viewModel.ClassroomScreenings, viewModel.ScreeningOther);

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
                    UserDefined = (curriculumCode == 1) ? (CurriculumOther) : String.Empty
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
                    UserDefined = (assessmentCode == 1) ? (AssessmentOther) : String.Empty
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
                    UserDefined = (screeningCode == 1) ? (ScreeningOther) : String.Empty
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

        //[HttpGet]
        public JsonResult getProviders(int id)
        {
            var providers = _context.Facilities
                .Where(c => c.ProviderID == id)
                .Select(c => new {
                    ID = c.ID,
                    Description = c.Description
                })
                .ToList();
            return Json(providers, JsonRequestBehavior.AllowGet);
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