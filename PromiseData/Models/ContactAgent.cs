namespace PromiseData.Models
{
    using System;
    using System.Collections.Generic;
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
        public string AgentName { get; set; }

        [StringLength(50)]
        public string AgentTitle { get; set; }

        [StringLength(15)]
        public string AgentPhone { get; set; }

        [StringLength(50)]
        public string AgentEmail { get; set; }

        [StringLength(15)]
        public string AgentFax { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Institution> Institutions { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Institution> Institutions1 { get; set; }
    }
}
