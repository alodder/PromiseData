namespace PromiseData.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Code_ProgramType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Code { get; set; }

        [StringLength(255)]
        [DisplayName("Program Type")]
        public string Description { get; set; }

        public DateTime? Effective { get; set; }

        public DateTime? EndDate { get; set; }
    }
}
