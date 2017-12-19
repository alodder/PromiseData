namespace PromiseData.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ClassroomScreening")]
    public partial class ClassroomScreening
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ClassroomID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ScreeningCode { get; set; }

        [StringLength(100)]
        [Column(Order = 2)]
        public string UserDefined { get; set; }

        public virtual ScreeningTools Screening { get; set; }

        public virtual Classroom Classroom { get; set; }
    }
}
