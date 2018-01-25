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
        public bool IsHub { get; set; }

        [DisplayName("Operator")]
        public bool IsProvider { get; set; }

        [DisplayName("Parent Hub")]
        public int? ParentHubId { get; set; }

        [StringLength(25)]
        [DisplayName("License #")]
        public string LicenseNumber { get; set; }

        [StringLength(100)]
        [DisplayName("Operator Type")]
        public string Type { get; set; }

        [ForeignKey("ParentHubId")]
        public virtual Institution ParentHub { get; set; }

        public virtual Address Address { get; set; }

        public virtual Address Address1 { get; set; }

        public virtual ContactAgent ContactAgent { get; set; }

        public virtual ContactAgent ContactAgent1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ContactAgent> ContactAgents { get; set; }
    }
}
