namespace PromiseData.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class LU_State
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public LU_State()
        {
            Addresses = new HashSet<Address>();
        }

        [StringLength(2)]
        public string ID { get; set; }

        [StringLength(30)]
        public string StateName { get; set; }

        [StringLength(2)]
        public string Abbreviation { get; set; }

        [StringLength(255)]
        public string Country { get; set; }

        [StringLength(255)]
        public string Type { get; set; }

        [StringLength(255)]
        public string Sort { get; set; }

        [StringLength(255)]
        public string Status { get; set; }

        [StringLength(255)]
        public string Occupied { get; set; }

        [StringLength(255)]
        public string Notes { get; set; }

        [StringLength(255)]
        public string FIPS_State { get; set; }

        [StringLength(255)]
        public string Assoc_press { get; set; }

        [StringLength(255)]
        public string Standard_Federal_Region { get; set; }

        [StringLength(255)]
        public string Census_Region { get; set; }

        [StringLength(255)]
        public string Census_Region_Name { get; set; }

        [StringLength(255)]
        public string Census_Division { get; set; }

        [StringLength(255)]
        public string Census_Division_Name { get; set; }

        [StringLength(255)]
        public string Circuit_Court { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Address> Addresses { get; set; }
    }
}
