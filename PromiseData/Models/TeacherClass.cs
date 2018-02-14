namespace PromiseData.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TeacherClass")]
    public partial class TeacherClass
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int TeacherID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ClassroomID { get; set; }

        [DisplayName("Date Assigned")]
        public DateTime? DateAssigned { get; set; }

        [DisplayName("Months In Classroom")]
        public int? MonthsInClassroom { get; set; }

        public virtual Teacher Teacher { get; set; }

        public virtual Classroom Classroom { get; set; }
    }
}
