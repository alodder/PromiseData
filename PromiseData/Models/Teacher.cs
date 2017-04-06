using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PromiseData.Models
{
    public class Teacher
    {
        public int Id { get; set; }
        public int  ClassroomId { get; set; }   //? Classroom w/PP Students 1: Teacher ID Number
        public String TeacherType { get; set; }
        public DateTime TeacherBirthdate { get; set; }
        public int GenderId { get; set; }
        public int RaceEthnicityIdentity { get; set; }
        public String ClassroomLanguages { get; set; }
        public String FluentLanguages { get; set; }
        public DateTime StartDate { get; set; }
        public Decimal TeacherSalary { get; set; }
        public int EducationID { get; set; }
        public bool CDA { get; set; }
        public String DegreeField { get; set; }
        public int PDStep { get; set; }  //?
        public int YearsExperience { get; set; }
        public DateTime EndDate { get; set; }
        public String ReadsonForLeaving { get; set; }
    }
}