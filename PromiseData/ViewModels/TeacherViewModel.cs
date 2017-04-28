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

        public String TeacherIDNumber { get; set; }

        public int ClassroomId { get; set; }   //? Classroom w/PP Students 1: Teacher ID Number
        public IEnumerable<Classroom> Classrooms { get; set; }

        public String TeacherType { get; set; }
        public IEnumerable<String> TeacherTypes { get; set; }

        public DateTime TeacherBirthdate { get; set; }

        public int GenderId { get; set; }
        public IEnumerable<Code_Gender> Genders { get; set; }

        public int RaceEthnicityIdentity { get; set; }

        public String ClassroomLanguages { get; set; }

        public String FluentLanguages { get; set; }

        public DateTime StartDate { get; set; }

        public Decimal TeacherSalary { get; set; }

        public int EducationID { get; set; }
        public IEnumerable<Code_Education> EducationTypes { get; set; }

        public bool CDA { get; set; }

        public String DegreeField { get; set; }

        public int PDStep { get; set; }  //?

        public int YearsExperience { get; set; }

        public DateTime EndDate { get; set; }

        public String ReasonForLeaving { get; set; }
    }
}