namespace PromiseData.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AssessmentTools")]
    public partial class AssessmentTools
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Code { get; set; }

        [StringLength(100)]
        public string Description { get; set; }

        public DateTime? EffectiveDate { get; set; }

        public DateTime? EndDate { get; set; }

        public virtual ICollection<ClassroomAssessment> ClassroomAssessments { get; set; }
    }
}
