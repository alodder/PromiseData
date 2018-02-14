using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PromiseData.Models;
using System.ComponentModel;
using System.Linq.Expressions;
using PromiseData.Controllers;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace PromiseData.ViewModels
{
    public class ClassroomViewModel
    {
        public int ID { get; set; }

        public Boolean CanView { get; set; }

        public Boolean CanEdit { get; set; }

        public Boolean CanDelete { get; set; }

        public String Action
        {
            get
            {
                Expression<Func<ClassroomController, ActionResult>> update =
                    (c => c.Update(this));
                Expression<Func<ClassroomController, ActionResult>> create =
                    (c => c.Create(this));

                var action = (ID != 0) ? update : create;
                return (action.Body as MethodCallExpression).Method.Name;
            }

        }

        public string Heading { get; set; }

        [DisplayName("Provider")]
        public int Facility_ID { get; set; }

        [DisplayName("Program")]
        public int Program_ID { get; set; }

        [DisplayName("Program Session Type")]
        public string ProgramSessionType { get; set; }

        [DisplayName("New or Expanded Class")]
        public string NewOrExpandedClass { get; set; }

        [DisplayName("Session Hours per Year")]
        public double SessionHours { get; set; }

        [DisplayName("Session Days per Year")]
        public int SessionDays { get; set; }

        [DisplayName("Session Weeks per Year")]
        public int SessionWeeks { get; set; }

        [DisplayName("Number of Preschool Promise Students")]
        public int PPStudents { get; set; }

        [DisplayName("Number of students funded by HS/OPK/MHS/SHS")]
        public int NonPPStudentsHSOPK { get; set; }

        [DisplayName("Number of students funded by other source")]
        public int NonPPStudentsThirdParty { get; set; }

        [DisplayName("Number of parent pay students")]
        public int NonPPStudentsParentPay { get; set; }

        [DisplayName("Preschool Promise Slots")]
        public int? PPSlots { get; set; }

        [DisplayName("Preschool Promise Slots Unfilled")]
        public int PPSlotsUnfilled { get; set; }

        [DisplayName("Classroom Capacity")]
        public int? Capacity { get; set; }

        [DisplayName("Number of students funded by Title funds")]
        public int? NonPPStudentsTitleFunds { get; set; }

        [DisplayName("Number of students receiving ERDC (this could include Preschool Promise children)")]
        public int? StudentsERDC { get; set; }

        [DisplayName("Upsize")]
        public byte[] upsize_ts { get; set; }

        [DisplayName("Name")]
        public string Description { get; set; }

        [DisplayName("Total Students")]
        public int? StudentsTotal { get; set; }

        [DisplayName("Other Curriculum")]
        public string CurriculumOther { get; set; }

        [DisplayName("Other Assessment Tool")]
        public string AssessmentOther { get; set; }

        [DisplayName("Other Screening Tool")]
        public string ScreeningOther { get; set; }

        [Range(0, 7)]
        [DisplayName("Emotional Support")]
        public int? CLASSScore_EmotionalSupport { get; set; } = 0;

        [Range(0, 7)]
        [DisplayName("Classroom Organization")]
        public int? CLASSScore_ClassroomOrganization { get; set; } = 0;

        [Range(0, 7)]
        [DisplayName("Instructional Support")]
        public int? CLASSScore_InstructionalSupport { get; set; } = 0;

        //dictionary for checkbox values
        public Dictionary<int, bool> ClassroomCurricula { get; set; }
        //dictionary for checkbox values
        public Dictionary<int, bool> ClassroomAssessments { get; set; }
        //dictionary for checkbox values
        public Dictionary<int, bool> ClassroomScreenings { get; set; }

        public IEnumerable<Institution> Operators { get; set; }

        public IEnumerable<Facility> Facilities { get; set; }

        public List<String> SessionTypes
        {
            get
            {
                List<String> types = new List<String>();
                types.Add("Half Day");
                types.Add("Full Day");
                return types;
            }
        }

        public List<Curricula> Curricula { get; set; }

        public List<AssessmentTools> AssessmentTools { get; set; }

        public List<ScreeningTools> ScreeningTools { get; set; }
    }
}