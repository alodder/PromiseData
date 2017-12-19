namespace PromiseData.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ClassroomAssessment")]
    public partial class ClassroomAssessment
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ClassroomID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int AssessmentCode { get; set; }

        [StringLength(100)]
        [Column(Order = 2)]
        public string UserDefined { get; set; }

        public virtual AssessmentTools AssessmentTool { get; set; }

        public virtual Classroom Classroom { get; set; }
    }
}
