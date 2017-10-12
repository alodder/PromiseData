namespace PromiseData.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CLASS_Scores")]
    public partial class CLASS_Score
    {
        [Key]
        public int Score_id { get; set; }

        public int Classroom_id { get; set; }

        public int? CLASSScore_EmotionalSupport { get; set; }

        public int? CLASSScore_ClassroomOrganization { get; set; }

        public int? CLASSScore_InstructionalSupport { get; set; }

        public DateTime date { get; set; }

        public virtual Classroom Classroom { get; set; }
    }
}
