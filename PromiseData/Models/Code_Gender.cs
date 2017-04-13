namespace PromiseData.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Code_Gender
    {
        [Key]
        public int Code { get; set; }

        [StringLength(12)]
        public string Desc { get; set; }

        [StringLength(100)]
        public string Description { get; set; }

        public DateTime? Effective { get; set; }

        public DateTime? End { get; set; }
    }
}
