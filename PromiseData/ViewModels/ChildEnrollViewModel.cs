using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.Linq;
using System.Web;
using PromiseData.Models;
using static System.DateTime;
using System.Linq.Expressions;
using PromiseData.Controllers;
using System.Web.Mvc;

namespace PromiseData.ViewModels
{
    public class ChildEnrollViewModel
    {
        public int ID { get; set; }

        public String Action
        {
            get
            {
                Expression<Func<ChildController, ActionResult>> update =
                    (c => c.UpdateEnrollment(this));
                Expression<Func<ChildController, ActionResult>> create =
                    (c => c.Enroll(this));

                var action = (ID != 0) ? update : create;
                return (action.Body as MethodCallExpression).Method.Name;
            }

        }

        [DisplayName("Child")]
        [Required]
        public int ChildID { get; set; }

        [DisplayName("Session")]
        [Required]
        public int ServicesID { get; set; }

        [DisplayName("Classroom")]
        [Required]
        public int ClassroomID { get; set; }

        [DisplayName("Site")]
        [Required]
        public int FacilityID { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Start Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("End Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }

        [DisplayName("Reason for End")]
        public string EndReason { get; set; }

        [DisplayName("Specify Reason for End")]
        public string OtherEndReason { get; set; }

        [DisplayName("Monthly Attendance")]
        //[RegularExpression(@"^\d+\.\d{0,2}$")]
        public decimal MonthlyAttendance { get; set; }

        [DisplayName("Did child/family receive or attend transition services or information?")]
        public bool? ReceivedInfo { get; set; }

        [DisplayName("Transportation Use")]
        public bool? TransportationUse { get; set; }

        public Child Child { get; set; }

        public Facility Facility { get; set; }

        public List<String> ExitReasonList
        {
            get
            {
                List<String> reasons = new List<String>();
                reasons.Add("Transition to kindergarten");
                reasons.Add("Moved out of the area");
                reasons.Add("Other");
                return reasons;
            }
        }

        public IEnumerable<Child> Children { get; set; }

        public IEnumerable<Facility> Sites { get; set; }

        public IEnumerable<Classroom> Classrooms { get; set; }

    }
}