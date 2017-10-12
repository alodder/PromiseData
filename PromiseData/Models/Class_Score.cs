namespace PromiseData.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CLASS_Scores")]
    public partial class CLASS_Score
    {
        [Key]
        public int Score_id { get; set; }

        public int Classroom_id { get; set; }

        [DisplayName("Emotional Support")]
        public int? CLASSScore_EmotionalSupport { get; set; }

        [DisplayName("Classroom Organization")]
        public int? CLASSScore_ClassroomOrganization { get; set; }

        [DisplayName("Instructional Support")]
        public int? CLASSScore_InstructionalSupport { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Column(TypeName = "date")]
        [DisplayName("Score Date")]
        public DateTime Score_date { get; set; }

        [ForeignKey("Classroom_id")]
        public virtual Classroom Classroom { get; set; }
    }
}
