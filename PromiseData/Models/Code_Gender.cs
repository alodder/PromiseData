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
        [StringLength(1)]
        public string Code { get; set; }

        [StringLength(100)]
        public string Description { get; set; }

        public DateTime? Effective { get; set; }

        public DateTime? EndDate { get; set; }
    }
}
