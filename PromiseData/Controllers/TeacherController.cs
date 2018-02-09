﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PromiseData.Models;
using PromiseData.ViewModels;
using System.Security.Claims;
using Advanced_Auditing.Models;
using PromiseData.Repositories;

namespace PromiseData.Controllers
{
    public class TeacherController : Controller
    {
        private ApplicationDbContext _context;
        private List<String> types;
        private List<Code_Language> Languages;
        private Dictionary<int, bool> LangBoolDictionary;
        private TeachersRepository _teacherRepository;
        private ClassroomRepository _classroomRepository;

        public TeacherController()
        {
            _context = new ApplicationDbContext();
            _teacherRepository = new TeachersRepository(_context);
            _classroomRepository = new ClassroomRepository(_context);

            types = new List<string>();
            types.Add("Lead");
            types.Add("Assistant");
            types.Add("Support");

            LangBoolDictionary = new Dictionary<int, bool>();
            Languages = _context.CodeLanguage.ToList();
            foreach (Code_Language language in Languages)
            {
                LangBoolDictionary.Add(language.Code, false);
            }
        }

        [HttpGet]
        [Audit(AuditingLevel = 1)]
        [Authorize(Roles = "Hub, Provider, Administrator, System Administrator")]
        public ActionResult Create(int? id)
        {
            var viewModel = new TeacherViewModel
            {
                Genders = _context.CodeGender.ToList(),
                RaceEthnicityList = _context.RaceEthnic.ToList(),
                EducationTypes = _context.Code_Education.ToList(),
                //Classrooms = _context.Classrooms,
                TeacherTypes = types,
                Languages = _context.CodeLanguage.ToList(),
                ClassroomLanguages = LangBoolDictionary,
                FluentLanguages = LangBoolDictionary
            };

            viewModel.Classrooms = _classroomRepository.GetUserClassrooms( (ClaimsPrincipal)User);

            return View("TeacherForm", viewModel);
        }

        [HttpPost]
        [Audit(AuditingLevel = 2)]
        [Authorize(Roles = "Provider, Hub, Administrator, System Administrator")]
        public ActionResult Create(TeacherViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Genders = _context.CodeGender.ToList();
                viewModel.RaceEthnicityList = _context.RaceEthnic.ToList();
                viewModel.EducationTypes = _context.Code_Education.ToList();
                viewModel.Classrooms = _context.Classrooms;
                viewModel.TeacherTypes = types;
                viewModel.Languages = _context.CodeLanguage.ToList();
                viewModel.ClassroomLanguages = LangBoolDictionary;
                viewModel.FluentLanguages = LangBoolDictionary;
                return View("TeacherForm", viewModel);
            }


            var teacher = new Teacher
            {
                TeacherIDNumber = viewModel.TeacherIDNumber,
                TeacherType = viewModel.TeacherType,
                TeacherBirthdate = viewModel.TeacherBirthdate,
                //Languages_spoken_in_classroom = viewModel.ClassroomLanguages,
                //FluentLanguages = viewModel.FluentLanguages,
                StartDate = viewModel.StartDate,
                TeacherSalary = viewModel.TeacherSalary,
                Education_ID = viewModel.EducationID,
                CDA = viewModel.CDA,
                DegreeField = viewModel.DegreeField,
                PDStep = viewModel.PDStep,
                YearsExperience = viewModel.YearsExperience,
                EndDate = viewModel.EndDate,
                ReasonForleaving = viewModel.ReasonForLeaving,
                TeacherRaceEthnicity = viewModel.RaceEthnicityIdentity,
                Gender_ID = viewModel.GenderId.ToString(),
                NameLast = viewModel.NameLast,
                NameFirst = viewModel.NameFirst
            };

            if (viewModel.DegreeField.Equals("Other"))
                teacher.DegreeField = viewModel.OtherField;

            _context.Teachers.Add(teacher);

            //SaveChanges() to set teacher.Id
            _context.SaveChanges();

            /*var classroom = _context.Classrooms.SingleOrDefault(c => c.ID == viewModel.ClassroomId);
            if(classroom != null)
                teacher.Classrooms.Add( classroom);*/

            //Associate teacher and Classroom in TeacherClass table - many to many?
            var teacherClass = new TeacherClass
            {
                TeacherID = teacher.ID,
                ClassroomID = viewModel.ClassroomId
            };
            _context.TeacherClasses.Add(teacherClass);

            //Add languages to TeacherLanguageClassroom table
            foreach (var languageId in viewModel.ClassroomLanguages.Keys)
            {
                if (viewModel.ClassroomLanguages[languageId])
                    _context.TeacherLanguageClassrooms.Add(new TeacherLanguageClassroom
                    {
                        TeacherID = teacher.ID,
                        LanguageID = languageId
                    });
            }

            //Add languages to TeacherLanguageFluency table
            foreach (var languageId in viewModel.FluentLanguages.Keys)
            {
                if (viewModel.FluentLanguages[languageId])
                    _context.TeacherLanguageFluencies.Add(new TeacherLanguageFluency
                    {
                        TeacherID = teacher.ID,
                        LanguageCode = languageId
                    });
            }

            _context.SaveChanges();

            return RedirectToAction("Index", "Teacher");
        }

        // GET: Teacher
        [HttpGet]
        [Audit(AuditingLevel = 1)]
        [Authorize(Roles = "Provider, Hub, Administrator, System Administrator")]
        public ActionResult Edit(int id)
        {
            var teacher = _context.Teachers.Single(i => i.ID == id);

            var viewModel = new TeacherViewModel {
                Id = teacher.ID,
                TeacherIDNumber = teacher.TeacherIDNumber,
                TeacherType = teacher.TeacherType,
                TeacherBirthdate = teacher.TeacherBirthdate.GetValueOrDefault(),
                RaceEthnicityIdentity = teacher.TeacherRaceEthnicity,
                StartDate = teacher.StartDate.GetValueOrDefault(),
                EndDate = teacher.EndDate,
                ReasonForLeaving = teacher.ReasonForleaving,
                TeacherSalary = teacher.TeacherSalary.GetValueOrDefault(),
                CDA = teacher.CDA,
                DegreeField = teacher.DegreeField,
                PDStep = teacher.PDStep.GetValueOrDefault(),
                YearsExperience = teacher.YearsExperience.GetValueOrDefault(),
                NameLast = teacher.NameLast,
                NameFirst = teacher.NameFirst
            };

            viewModel.Genders = _context.CodeGender.ToList();
            viewModel.RaceEthnicityList = _context.RaceEthnic.ToList();
            viewModel.EducationTypes = _context.Code_Education.ToList();

            //List of classrooms limited to classrooms assigned to user
            viewModel.Classrooms = _context.Classrooms;

            viewModel.TeacherTypes = types;
            viewModel.Languages = _context.CodeLanguage.ToList();

            var langList = _context.CodeLanguage.ToList();

            viewModel.ClassroomLanguages = new Dictionary<int, bool>();
            viewModel.FluentLanguages = new Dictionary<int, bool>();

            var langClassSet = _context.TeacherLanguageClassrooms.Where(t => t.TeacherID == teacher.ID);
            var langFluentSet = _context.TeacherLanguageFluencies.Where(t => t.TeacherID == teacher.ID);
            foreach (Code_Language language in langList)
            {
                if (langClassSet.Select(t => t.LanguageID).Contains(language.Code))
                    viewModel.ClassroomLanguages.Add(language.Code, true);
                else
                    viewModel.ClassroomLanguages.Add(language.Code, false);

                if (langFluentSet.Select(t => t.LanguageCode).Contains(language.Code))
                    viewModel.FluentLanguages.Add(language.Code, true);
                else
                    viewModel.FluentLanguages.Add(language.Code, false);
            }

            return View("TeacherForm", viewModel);
        }

        [HttpPost]
        [Audit(AuditingLevel = 2)]
        [Authorize(Roles = "Provider, Hub, Administrator, System Administrator")]
        public ActionResult Update(TeacherViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Genders = _context.CodeGender.ToList();
                viewModel.RaceEthnicityList = _context.RaceEthnic.ToList();
                viewModel.EducationTypes = _context.Code_Education.ToList();
                viewModel.Classrooms = _context.Classrooms;
                viewModel.TeacherTypes = types;
                viewModel.Languages = _context.CodeLanguage.ToList();
                viewModel.ClassroomLanguages = LangBoolDictionary;
                viewModel.FluentLanguages = LangBoolDictionary;
                return View("TeacherForm", viewModel);
            }

            //get teacher object from db
            var teacher = _context.Teachers.Single(i => i.ID == viewModel.Id);

            //Update values from viewmodel
            teacher.TeacherIDNumber = viewModel.TeacherIDNumber;
            teacher.TeacherType = viewModel.TeacherType;
            teacher.TeacherBirthdate = viewModel.TeacherBirthdate;
            //Languages_spoken_in_classroom = viewModel.ClassroomLanguages,
            //FluentLanguages = viewModel.FluentLanguages,
            teacher.StartDate = viewModel.StartDate;
            teacher.TeacherSalary = viewModel.TeacherSalary;
            teacher.Education_ID = viewModel.EducationID;
            teacher.CDA = viewModel.CDA;
            teacher.DegreeField = viewModel.DegreeField;
            teacher.PDStep = viewModel.PDStep;
            teacher.YearsExperience = viewModel.YearsExperience;
            teacher.EndDate = viewModel.EndDate;
            teacher.ReasonForleaving = viewModel.ReasonForLeaving;
            teacher.TeacherRaceEthnicity = viewModel.RaceEthnicityIdentity;
            teacher.Gender_ID = viewModel.GenderId.ToString();
            teacher.NameLast = viewModel.NameLast;
            teacher.NameFirst = viewModel.NameFirst;

            if (viewModel.DegreeField.Equals("Other"))
                teacher.DegreeField = viewModel.OtherField;

            //SaveChanges()
            _context.SaveChanges();

            //Associate teacher and Classroom in TeacherClass table - many to many?
            var teacherClass = new TeacherClass
            {
                TeacherID = teacher.ID,
                ClassroomID = viewModel.ClassroomId
            };
            //if(!_context.TeacherClasses.Contains(teacherClass))
            //    _context.TeacherClasses.Add(teacherClass);

            ////////////////////////////////////
            //Update languages in ClassroomLanguages to TeacherLanguageClassroom table
            var langClassSet = _context.TeacherLanguageClassrooms.Where(t => t.TeacherID == teacher.ID);
            foreach (var languageId in viewModel.ClassroomLanguages.Keys)
            {
                //Create TeacherLanguageClassroom for Teacher and Language pair
                var teacherLangClass = new TeacherLanguageClassroom {
                    TeacherID = teacher.ID,
                    LanguageID = languageId
                };

                if (Languages[languageId].Description == "Other")
                {
                    teacherLangClass.UserDefined = viewModel.OtherClassroomLanguage;
                }

                /**
                 * If the language wasn't checked, remove from table,
                 * else if it was both checked and doesn't yet exist, add it
                 */
                if (!viewModel.ClassroomLanguages[languageId])
                {
                    _context.TeacherLanguageClassrooms.RemoveRange(langClassSet.Where(tlc => tlc.LanguageID == teacherLangClass.LanguageID));
                }
                else if (viewModel.ClassroomLanguages[languageId] &&
                        !langClassSet.Select(t => t.TeacherID).Contains(teacherLangClass.TeacherID))
                {
                    _context.TeacherLanguageClassrooms.Add(teacherLangClass);
                }
            }

            ////////////////////////////////////
            //Add languages to TeacherLanguageFluency table
            var TeacherLanguageSet = _context.TeacherLanguageFluencies.Where(t => t.TeacherID == teacher.ID);
            foreach (var languageId in viewModel.FluentLanguages.Keys)
            {
                //Create TeacherLanguageFluency for Teacher and Language pair
                var teacherLangFluent = new TeacherLanguageFluency
                {
                    TeacherID = teacher.ID,
                    LanguageCode = languageId
                };

                /**
                 * If the language wasn't checked, remove from table,
                 * else if it was both checked and doesn't yet exist, add it
                 */
                if (!viewModel.FluentLanguages[languageId])
                {
                    _context.TeacherLanguageFluencies.RemoveRange(TeacherLanguageSet.Where(t => t.LanguageCode == languageId));
                }
                else if (viewModel.FluentLanguages[languageId] &&
                            !TeacherLanguageSet.Select(t => t.LanguageCode).Contains(teacherLangFluent.LanguageCode))
                {

                    _context.TeacherLanguageFluencies.Add(teacherLangFluent);
                }
            }

            _context.SaveChanges();

            return RedirectToAction("Index", "Teacher");
        }

        [HttpGet]
        [Audit(AuditingLevel = 1)]
        [Authorize(Roles = "Provider, Hub, Administrator, System Administrator")]
        public ActionResult RemoveFromClassroom(int id, int classroomid)
        {
            var teacher = _context.Teachers.Single(t => t.ID == id);
            var classroom = _context.Classrooms.Single(t => t.ID == classroomid);
            var teacherClass = _context.TeacherClasses.Single(tc => tc.TeacherID == id && tc.ClassroomID == classroomid);
            var teacherClassView = new TeacherClassViewModel {
                teacher = teacher,
                classroom = classroom,
                teacherClass = teacherClass
            };
            return View(teacherClassView);
        }

        [HttpPost]
        [Audit(AuditingLevel = 2)]
        [Authorize(Roles = "Provider, Hub, Administrator, System Administrator")]
        public ActionResult RemoveFromClassroom(TeacherClassViewModel teacherClassView)
        {

            _context.TeacherClasses.RemoveRange(_context.TeacherClasses.Where( tc => tc.TeacherID == teacherClassView.teacherid && tc.ClassroomID == teacherClassView.classroomid));
            _context.SaveChanges();

            return RedirectToAction("Details", "Classroom", new { id = teacherClassView.classroomid } );
        }

        [Authorize]
        public ActionResult Details(int id)
        {
            var teacher = _context.Teachers.Single(i => i.ID == id);

            var viewModel = new TeacherViewModel
            {
                Id = teacher.ID,
                TeacherIDNumber = teacher.TeacherIDNumber,
                TeacherType = teacher.TeacherType,
                TeacherBirthdate = teacher.TeacherBirthdate.GetValueOrDefault(),
                GenderId = teacher.Gender_ID[0],
                RaceEthnicityIdentity = teacher.TeacherRaceEthnicity,
                StartDate = teacher.StartDate.GetValueOrDefault(),
                EndDate = teacher.EndDate,
                ReasonForLeaving = teacher.ReasonForleaving,
                TeacherSalary = teacher.TeacherSalary.GetValueOrDefault(),
                EducationID = teacher.Education_ID.GetValueOrDefault(),
                CDA = teacher.CDA,
                DegreeField = teacher.DegreeField,
                PDStep = teacher.PDStep.GetValueOrDefault(),
                YearsExperience = teacher.YearsExperience.GetValueOrDefault(),
                NameLast = teacher.NameLast,
                NameFirst = teacher.NameFirst
            };

            viewModel.Genders = _context.CodeGender.ToList();
            viewModel.RaceEthnicityList = _context.RaceEthnic.ToList();
            viewModel.EducationTypes = _context.Code_Education.ToList();

            //List of classrooms limited to classrooms assigned to user
            var classIDs = teacher.TeacherClasses.Select(tc => tc.ClassroomID);
            viewModel.Classrooms = _context.Classrooms.Where( c => classIDs.Contains( c.ID)).ToList();

            viewModel.TeacherTypes = types;
            viewModel.Languages = _context.CodeLanguage.ToList();

            viewModel.WaiverRequests = teacher.WaiverRequests;
            viewModel.WaiversCurrent = teacher.WaiverCurrents;

            var langList = _context.CodeLanguage.ToList();

            viewModel.ClassroomLanguages = new Dictionary<int, bool>();
            viewModel.FluentLanguages = new Dictionary<int, bool>();

            var langClassSet = _context.TeacherLanguageClassrooms.Where(t => t.TeacherID == teacher.ID);
            var langFluentSet = _context.TeacherLanguageFluencies.Where(t => t.TeacherID == teacher.ID);
            foreach (Code_Language language in langList)
            {
                if (langClassSet.Select(t => t.LanguageID).Contains( language.Code))
                    viewModel.ClassroomLanguages.Add(language.Code, true);
                else
                    viewModel.ClassroomLanguages.Add(language.Code, false);

                if (langFluentSet.Select(t => t.LanguageCode).Contains( language.Code))
                    viewModel.FluentLanguages.Add(language.Code, true);
                else
                    viewModel.FluentLanguages.Add(language.Code, false);
            }

            return View( viewModel);
        }

        [Audit(AuditingLevel = 2)]
        [Authorize(Roles = "Administrator, System Administrator")]
        public ActionResult Delete(int id)
        {
            var teacher = _context.Teachers.Single(t => t.ID == id);
            return View( teacher);
        }

        [Audit(AuditingLevel = 2)]
        [Authorize(Roles = "Administrator, System Administrator")]
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult ConfirmDelete(int id)
        {
            var teacher = _context.Teachers.Single(t => t.ID == id);
            _context.Teachers.Remove(teacher);
            _context.SaveChanges();
            return RedirectToAction("Index", "Teacher");
        }

        [HttpPost]
        [Authorize]
        public ActionResult Search(TeacherListViewModel viewModel)
        {
            return RedirectToAction("Index", "Teacher", new { query = viewModel.SearchTerm });
        }

        [Authorize]
        public ActionResult Index(string query = null)
        {
            var viewModel = new TeacherListViewModel();
            viewModel.Teachers = _teacherRepository.GetUserTeachers( (ClaimsPrincipal)User).ToList();

            if (User.IsInRole("Administrator") || User.IsInRole("System Administrator"))
            {
                viewModel.CanAdd = true;
                viewModel.CanEdit = true;
                viewModel.CanDelete = true;
            }

            return View( viewModel);
        }
    }
}