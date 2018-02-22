namespace PromiseData.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Child_Classroom_Enrollment
    {
        [Key]
        public int ID { get; set; }

        [DisplayName("Child")]
        public int ChildID { get; set; }

        [DisplayName("Classroom")]
        public int ClassroomID { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Start Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? StartDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("End Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? EndDate { get; set; }

        [StringLength(50)]
        [DisplayName("Reason for End")]
        public string EndReason { get; set; }

        [DisplayName("Monthly Attendance")]
        //[RegularExpression(@"^\d+\.\d{0,2}$")]
        public decimal? MonthlyAttendance { get; set; }

        [DisplayName("Child/family received or attended transition services or information?")]
        public bool? ReceivedInfo { get; set; }

        [DisplayName("Transportation Use")]
        public bool? TransportationUse { get; set; }

        [ForeignKey("ChildID")]
        public virtual Child Child { get; set; }

        [ForeignKey("ClassroomID")]
        public virtual Classroom Classroom { get; set; }
    }
}
