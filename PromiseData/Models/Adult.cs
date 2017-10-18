namespace PromiseData.Models
{
    using System;
    using System.Collections.Generic;
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

        //[DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        public int? ELD_ID { get; set; }

        public int? AdultType_ID { get; set; }

        public int? Age { get; set; }

        public char Gender_ID { get; set; }

        [StringLength(255)]
        public string ResidentialTime { get; set; }

        public int? Education_ID { get; set; }

        [StringLength(16)]
        public String Employment { get; set; }

        public virtual Code_Education Code_Education { get; set; }

        public virtual ELD_ID ELD_ID1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AdultRace> AdultRaces { get; set; }
    }
}
