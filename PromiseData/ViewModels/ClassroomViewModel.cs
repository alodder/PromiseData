using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PromiseData.Models;
using System.ComponentModel;
using System.Linq.Expressions;
using PromiseData.Controllers;
using System.Web.Mvc;

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

        [DisplayName("Facility")]
        public int Facility_ID { get; set; }

        [DisplayName("Program")]
        public int Program_ID { get; set; }

        [DisplayName("Program Session Type")]
        public int ProgramSessionType_ID { get; set; }

        [DisplayName("New or Expanded Class")]
        public string NewOrExpandedClass { get; set; }

        [DisplayName("Session Hours")]
        public double SessionHours { get; set; }

        [DisplayName("Session Days")]
        public int SessionDays { get; set; }

        [DisplayName("Session Weeks")]
        public int SessionWeeks { get; set; }

        [DisplayName("Preschool Promise Students")]
        public int PPStudents { get; set; }

        [DisplayName("Non-PP HSOPK")]
        public int NonPPStudentsHSOPK { get; set; }

        [DisplayName("Non-PP 3rd Party")]
        public int NonPPStudentsThirdParty { get; set; }

        [DisplayName("Non-PP Parent Pay")]
        public int NonPPStudentsParentPay { get; set; }

        [DisplayName("PP Slots Unfilled")]
        public int PPSlotsUnfilled { get; set; }

        [DisplayName("CLASSScore Emotional Support")]
        public int CLASSScore_EmotionalSupport { get; set; }

        [DisplayName("CLASSScore Classroom Organization")]
        public int CLASSScore_ClassroomOrganization { get; set; }

        [DisplayName("CLASSScore Instructional Support")]
        public int CLASSScore_InstructionalSupport { get; set; }

        [DisplayName("Upsize")]
        public byte[] upsize_ts { get; set; }

        [DisplayName("Descriptor")]
        public string Description { get; set; }

        [DisplayName("Classroom Capacity")]
        public int? Capacity { get; set; }

        [DisplayName("Preschool Promise Slots")]
        public int? PPSlots { get; set; }

        [DisplayName("Other Curriculum")]
        public string CurriculumOther { get; set; }

        [DisplayName("Other Assessment Tool")]
        public string AssessmentOther { get; set; }

        [DisplayName("Other Screening Tool")]
        public string ScreeningOther { get; set; }

        //dictionary for checkbox values
        public Dictionary<int, bool> ClassroomCurricula { get; set; }
        //dictionary for checkbox values
        public Dictionary<int, bool> ClassroomAssessments { get; set; }
        //dictionary for checkbox values
        public Dictionary<int, bool> ClassroomScreenings { get; set; }

        public IEnumerable<Facility> Facilities { get; set; }

        public IEnumerable<Code_ProgramSessionType> SessionTypes { get; set; }

        public IEnumerable<Curricula> Curricula { get; set; }

        public IEnumerable<AssessmentTools> AssessmentTools { get; set; }

        public IEnumerable<ScreeningTools> ScreeningTools { get; set; }
    }
}