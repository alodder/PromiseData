namespace PromiseData.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Adult")]
    public partial class Adult
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Adult()
        {
            AdultRaces = new HashSet<AdultRace>();
        }

        public int ID { get; set; }

        public int? ELD_ID { get; set; }

        public String AdultType { get; set; }

        public int? Age { get; set; }

        [DisplayName("Gender")]
        [StringLength(1)]
        public string Gender_Code { get; set; }

        [DisplayName("Time with child/Week")]
        [StringLength(255)]
        public string ResidentialTime { get; set; }

        [DisplayName("Education")]
        public int? Education_ID { get; set; }

        [StringLength(16)]
        public String Employment { get; set; }

        public virtual Code_Education Code_Education { get; set; }

        public virtual ELD_ID ELD_ID1 { get; set; }

        public int? FamilyID { get; set; }

        [DisplayName("First Name")]
        public String NameFirst { get; set; }

        [DisplayName("Last Name")]
        public String NameLast { get; set; }

        [ForeignKey("FamilyID")]
        public virtual Family Family { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AdultRace> AdultRaces { get; set; }

        [ForeignKey("Gender_Code")]
        public virtual Code_Gender Gender { get; set; }
    }
}
