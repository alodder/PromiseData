namespace PromiseData.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Institution")]
    public partial class Institution
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string LegalName { get; set; }

        [StringLength(50)]
        public string Region { get; set; }

        [StringLength(50)]
        public string BackboneOrg { get; set; }

        public string WebAddress { get; set; }

        public int? DirectorAgentId { get; set; }

        public int? ContactAgentId { get; set; }

        public int? LocationAddressId { get; set; }

        public int? MailingAddressId { get; set; }

        [Column(TypeName = "date")]
        public DateTime ActiveDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? EndDate { get; set; }

        public bool isHub { get; set; }

        public bool isProvider { get; set; }

        public virtual ContactAgent ContactAgent { get; set; }

        public virtual ContactAgent ContactAgent1 { get; set; }

        public virtual Address AddressMail { get; set; }

        public virtual Address AddressPhysical { get; set; }
    }
}
