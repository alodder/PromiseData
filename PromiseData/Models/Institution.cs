namespace PromiseData.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Institution")]
    public partial class Institution
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [DisplayName("Name")]
        public string LegalName { get; set; }

        [DisplayName("Region")]
        [StringLength(50)]
        public string Region { get; set; }

        [StringLength(50)]
        [DisplayName("Organization")]
        public string BackboneOrg { get; set; }

        [DisplayName("Web Address")]
        public string WebAddress { get; set; }

        [DisplayName("Director")]
        public int? DirectorAgentId { get; set; }

        [DisplayName("Contact")]
        public int? ContactAgentId { get; set; }

        [DisplayName("Location")]
        public int? LocationAddressId { get; set; }

        [DisplayName("Mailing Address")]
        public int? MailingAddressId { get; set; }

        [Column(TypeName = "date")]
        public DateTime ActiveDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? EndDate { get; set; }

        [DisplayName("Hub")]
        public bool isHub { get; set; }

        [DisplayName("Provider")]
        public bool isProvider { get; set; }

        [DisplayName("Parent Hub")]
        public int? parentHubId { get; set; }

        [DisplayName("License #")]
        public string LicenseNumber { get; set; }

        public virtual Address Address { get; set; }

        public virtual Address Address1 { get; set; }

        public virtual ContactAgent ContactAgent { get; set; }

        public virtual ContactAgent ContactAgent1 { get; set; }
    }
}
