using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.Linq;
using System.Web;
using PromiseData.Models;
using static System.DateTime;

namespace PromiseData.ViewModels
{
    public class ChildEnrollViewModel
    {
        [DisplayName("Child")]
        [Required]
        public int ChildID { get; set; }

        [DisplayName("Classroom")]
        [Required]
        public int ServicesID { get; set; }

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

        [DisplayName("Monthly Attendance")]
        [RegularExpression(@"^\d+\.\d{0,2}$")]
        public decimal MonthlyAttendance { get; set; }

        [DisplayName("Received Info")]
        [StringLength(10)]
        public string ReceivedInfo { get; set; }

        [DisplayName("Received Info")]
        public bool TransportationUse { get; set; }

        public Child Child { get; set; }

        public Facility Facility { get; set; }

        public Service Service { get; set; }

        public IEnumerable<Child> Children { get; set; }

        public IEnumerable<Facility> Sites { get; set; }

        public IEnumerable<Service> Services { get; set; }
    }
}