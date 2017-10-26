namespace PromiseData.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Code_IFSP
    {
        [Key]
        [StringLength(24)]
        public string Code { get; set; }

        [StringLength(50)]
        public string Description { get; set; }
    }
}
