namespace PromiseData.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Address")]
    public partial class Address
    {
        public int ID { get; set; }

        public int? AddressType_ID { get; set; }

        [StringLength(40)]
        public string Address1 { get; set; }

        [StringLength(40)]
        public string Address2 { get; set; }

        [StringLength(40)]
        public string Address3 { get; set; }

        [StringLength(40)]
        public string City { get; set; }

        [StringLength(2)]
        public string State_ID { get; set; }

        [StringLength(9)]
        public string ZipCode { get; set; }

        [StringLength(40)]
        public string County { get; set; }

        public virtual Code_AddressType Code_AddressType { get; set; }

        public virtual LU_State LU_State { get; set; }
    }
}
