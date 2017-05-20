using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PromiseData.Models;
using PromiseData.ViewModels;

namespace PromiseData.Controllers
{
    public class TeacherController : Controller
    {
        private ApplicationDbContext _context;
        private List<String> types;
        private Dictionary<int, bool> LangBoolDictionary;

        public TeacherController()
        {
            _context = new ApplicationDbContext();

            types = new List<string>();
            types.Add("Lead");
            types.Add("Assistant");
            types.Add("Support");

            LangBoolDictionary = new Dictionary<int, bool>();
            var langList = _context.CodeLanguage.ToList();
            foreach (Code_Language language in langList)
            {
                LangBoolDictionary.Add(language.Code, false);
            }
        }

        [HttpGet]
        [Authorize]
        public ActionResult Create()
        {
            var viewModel = new TeacherViewModel
            {
                Genders = _context.CodeGender.ToList(),
                RaceEthnicityList = _context.RaceEthnic.ToList(),
                EducationTypes = _context.Code_Education.ToList(),
                Classrooms = _context.Classrooms,
                TeacherTypes = types,
                Languages = _context.CodeLanguage.ToList(),
                ClassroomLanguages = LangBoolDictionary, 
                FluentLanguages = LangBoolDictionary
            };
            return View(viewModel);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Create( TeacherViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Genders = _context.CodeGender.ToList();
                viewModel.EducationTypes = _context.Code_Education.ToList();
                viewModel.Classrooms = _context.Classrooms;
                viewModel.TeacherTypes = types;
                return View("Create", viewModel);
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
                Gender_ID = viewModel.GenderId
            };

            if (viewModel.DegreeField.Equals("Other"))
                teacher.DegreeField = viewModel.OtherField;

            _context.Teachers.Add( teacher);

            var teacherClass = new TeacherClass
            {
                TeacherID = viewModel.Id,
                ClassID = viewModel.ClassroomId
            };
            _context.TeacherClasses.Add(teacherClass);
            
            _context.SaveChanges();

            return RedirectToAction("Index", "Teacher");
        }

        [Authorize]
        public ActionResult Details(int id)
        {
            var teacher = _context.Teachers.Single(t => t.ID == id);
            return View( teacher);
        }

        [Authorize]
        public ActionResult Delete(int id)
        {
            var teacher = _context.Teachers.Single(t => t.ID == id);
            return View( teacher);
        }

        [Authorize]
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult ConfirmDelete(int id)
        {
            var teacher = _context.Teachers.Single(t => t.ID == id);
            _context.Teachers.Remove(teacher);
            _context.SaveChanges();
            return RedirectToAction("Index", "Teacher");
        }

        [Authorize]
        // GET: Adult
        public ActionResult Index()
        {
            var viewModel = _context.Teachers;
            return View( viewModel);
        }
    }
}