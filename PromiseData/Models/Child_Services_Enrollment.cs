namespace PromiseData.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Child_Services_Enrollment
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ChildID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ServicesID { get; set; }

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

        [DisplayName("Transportation Use")]
        public bool TransportationUse { get; set; }

        [ForeignKey("ChildID")]
        public virtual Child Child { get; set; }

        [ForeignKey("ServicesID")]
        public virtual Service Service { get; set; }
    }
}
