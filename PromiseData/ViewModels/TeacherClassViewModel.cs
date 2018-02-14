using PromiseData.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace PromiseData.ViewModels
{
    public class TeacherClassViewModel
    {
        public Teacher teacher { get; set; }
        public Classroom classroom { get; set; }
        public TeacherClass teacherClass { get; set; }

        [DisplayName("Teacher")]
        public int teacherid { get; set; }

        [DisplayName("Classroom")]
        public int classroomid { get; set; }

        [DisplayName("Date Assigned")]
        public DateTime? DateAssigned { get; set; }

        [DisplayName("Months In Classroom")]
        public int? MonthsInClassroom { get; set; }

        public bool ShowActions { get; set; }
        public string Heading { get; set; }
        public string SearchTerm { get; set; }
        public Boolean CanAdd { get; set; }
        public Boolean CanEdit { get; set; }
        public Boolean CanDelete { get; set; }
        public Boolean CanView { get; set; }

        public IEnumerable<Teacher> Teachers { get; set; }
    }
}