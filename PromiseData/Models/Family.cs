namespace PromiseData.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Family")]
    public partial class Family
    {
        public int ID { get; set; }

        [DisplayName("Household Size")]
        public int? HouseholdSize { get; set; }

        [DisplayName("Children In Home")]
        public int? ChildrenInHome { get; set; }

        [DisplayName("Income")]
        [Column(TypeName = "money")]
        public decimal? Income { get; set; }

        public bool SNAP { get; set; }

        public bool WIC { get; set; }

        public bool TANF { get; set; }

        public bool SSI { get; set; }

        [DisplayName("Monthly Cost Additional Services")]
        public int? MonthlyCostAdditionalServices { get; set; }

        public virtual ICollection<Adult> Adults { get; set; }

        public virtual ICollection<Child> Children { get; set; }
    }
}
