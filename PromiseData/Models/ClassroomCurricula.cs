namespace PromiseData.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ClassroomCurricula")]
    public partial class ClassroomCurricula
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ClassroomID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CurriculaCode { get; set; }

        [StringLength(100)]
        [Column(Order = 2)]
        public string UserDefined { get; set; }

        public virtual Curricula Curricula { get; set; }

        public virtual Classroom Classroom { get; set; }
    }
}
