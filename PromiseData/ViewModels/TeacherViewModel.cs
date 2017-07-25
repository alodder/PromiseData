using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using PromiseData.Models;

namespace PromiseData.ViewModels
{
    public class TeacherViewModel
    {
        public int Id { get; set; }

        [DisplayName("Teacher ID Number")]
        public String TeacherIDNumber { get; set; }

        [DisplayName("Classroom")]
        public int ClassroomId { get; set; }   //? Classroom w/PP Students 1: Teacher ID Number
        public IEnumerable<Classroom> Classrooms { get; set; }

        [DisplayName("Teacher Role")]
        public String TeacherType { get; set; }
        public IEnumerable<String> TeacherTypes { get; set; }

        [DisplayName("Birthday")]
        public DateTime TeacherBirthdate { get; set; }

        [DisplayName("Teacher Sex")]
        public int GenderId { get; set; }
        public IEnumerable<Code_Gender> Genders { get; set; }

        [DisplayName("Teacher Race/Ethnicity")]
        public int RaceEthnicityIdentity { get; set; }

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
        public DateTime EndDate { get; set; }

        [DisplayName("Reason for Leaving")]
        public String ReasonForLeaving { get; set; }
    }
}