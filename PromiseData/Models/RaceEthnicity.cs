namespace PromiseData.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RaceEthnicity")]
    public partial class RaceEthnicity
    {
        [Key]
        public int Id { get; set; }

        [StringLength(100)]
        public string Description { get; set; }

        [StringLength(32)]
        public string Code { get; set; }
    }
}
