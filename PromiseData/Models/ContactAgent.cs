namespace PromiseData.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ContactAgent
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ContactAgent()
        {
            Institutions = new HashSet<Institution>();
            Institutions1 = new HashSet<Institution>();
        }

        [Key]
        public int AgentId { get; set; }

        [Required]
        [StringLength(50)]
        [DisplayName("Name")]
        public string AgentName { get; set; }

        [StringLength(50)]
        [DisplayName("Title")]

        public string AgentTitle { get; set; }

        [StringLength(15)]
        [DisplayName("Phone")]
        public string AgentPhone { get; set; }

        [StringLength(50)]
        [DisplayName("Email")]
        public string AgentEmail { get; set; }

        [StringLength(15)]
        [DisplayName("Fax")]
        public string AgentFax { get; set; }

        [DisplayName("Institution")]
        public int? InstitutionId { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Institution> Institutions { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Institution> Institutions1 { get; set; }
    }
}
