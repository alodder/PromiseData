namespace PromiseData.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Address")]
    public partial class Address
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Address()
        {
            Institutions = new HashSet<Institution>();
            Institutions1 = new HashSet<Institution>();
        }

        public int ID { get; set; }

        public int? AddressType_ID { get; set; }

        [StringLength(40)]
        [DisplayName("Address Line 1")]
        public string Address1 { get; set; }

        [StringLength(40)]
        [DisplayName("Address Line 2")]
        public string Address2 { get; set; }

        [StringLength(40)]
        [DisplayName("Address Line 3")]
        public string Address3 { get; set; }

        [StringLength(40)]
        [DisplayName("City")]
        public string City { get; set; }

        [StringLength(2)]
        [DisplayName("State")]
        public string State_ID { get; set; }

        [StringLength(9)]
        [DisplayName("Zip Code")]
        public string ZipCode { get; set; }

        [StringLength(40)]
        public string County { get; set; }

        public virtual Code_AddressType Code_AddressType { get; set; }

        [DisplayName("State")]
        public virtual LU_State LU_State { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Institution> Institutions { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Institution> Institutions1 { get; set; }
    }
}
