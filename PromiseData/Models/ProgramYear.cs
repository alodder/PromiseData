namespace PromiseData.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ProgramYear")]
    public partial class ProgramYear
    {
        [Key]
        public int ID { get; set; }

        [DisplayName("Start Date")]
        public DateTime StartDate { get; set; }

        [DisplayName("End Date")]
        public DateTime EndDate { get; set; }

        [DisplayName("Anticipated Service Hours in program year")]
        public int? ServiceHours { get; set; }

        public int ProviderID { get; set; }

        [ForeignKey("ProviderID")]
        public virtual Facility Provider { get; set; }
    }
}

