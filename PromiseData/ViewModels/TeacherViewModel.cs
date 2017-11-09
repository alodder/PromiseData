using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using PromiseData.Models;
using System.Linq.Expressions;
using PromiseData.Controllers;
using System.Web.Mvc;

namespace PromiseData.ViewModels
{
    public class TeacherViewModel
    {
        public int Id { get; set; }

        public Boolean CanView { get; set; }

        public Boolean CanEdit { get; set; }

        public Boolean CanDelete { get; set; }

        public String Action
        {
            get
            {
                Expression<Func<TeacherController, ActionResult>> update =
                    (c => c.Update(this));
                Expression<Func<TeacherController, ActionResult>> create =
                    (c => c.Create(this));

                var action = (Id != 0) ? update : create;
                return (action.Body as MethodCallExpression).Method.Name;
            }

        }

        [DisplayName("Teacher ID Number")]
        public String TeacherIDNumber { get; set; }

        [Required]
        [DisplayName("Classroom")]
        public int ClassroomId { get; set; }   //? Classroom w/PP Students 1: Teacher ID Number

        public IEnumerable<Classroom> Classrooms { get; set; }

        [DisplayName("Teacher Role")]
        public String TeacherType { get; set; }

        public IEnumerable<String> TeacherTypes { get; set; }

        [DisplayName("Birthday")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime TeacherBirthdate { get; set; }

        [DisplayName("Teacher Sex")]
        public char GenderId { get; set; }

        public IEnumerable<Code_Gender> Genders { get; set; }

        [DisplayName("Teacher Race/Ethnicity")]
        public int? RaceEthnicityIdentity { get; set; }

        public IEnumerable<RaceEthnicity> RaceEthnicityList { get; set; }

        //dictionary for checkbox values
        public Dictionary<int, bool> RaceDictionary { get; set; }

        //List of possible languages
        public IEnumerable<Code_Language> Languages { get; set; }

        [DisplayName("Classroom Languages")]
        //dictionary for checkbox values
        public Dictionary<int, bool> ClassroomLanguages { get; set; }

        [DisplayName("Fluent Languages")]
        //dictionary for checkbox values
        public Dictionary<int, bool> FluentLanguages { get; set; }

        [DisplayName("Start Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [DisplayName("Salary")]
        public Decimal TeacherSalary { get; set; }

        [DisplayName("Education")]
        public int EducationID { get; set; }
        public IEnumerable<Code_Education> EducationTypes { get; set; }

        [DisplayName("CDA")]
        public bool CDA { get; set; }

        [DisplayName("Degree Field")]
        public String DegreeField { get; set; }

        [DisplayName("Other")]
        public String OtherField { get; set; }

        [DisplayName("PD Step?")]
        public int PDStep { get; set; }  //?

        [DisplayName("Years Experience")]
        public int YearsExperience { get; set; }

        [DisplayName("End Date")]
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }

        [DisplayName("Reason for Leaving")]
        public String ReasonForLeaving { get; set; }

        [DisplayName("Last Name")]
        public String NameLast { get; set; }

        [DisplayName("First Name")]
        public String NameFirst { get; set; }
    }
}