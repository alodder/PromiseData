namespace PromiseData.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Facility")]
    public partial class Facility
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Facility()
        {
            Classrooms = new HashSet<Classroom>();
        }

        public int ID { get; set; }

        [StringLength(24)]
        public string ProviderFacilityType { get; set; }

        [StringLength(255)]
        public string Turnover_NonPPStaff { get; set; }

        [StringLength(255)]
        public string TurnoverReasons_NonPPStaff { get; set; }

        public bool Transportation_services_offered { get; set; }

        public int? ChildrenReceivingTransportationServices { get; set; }

        public int? AdditionalChildFamilySupports_ID { get; set; }

        public DateTime? MonitoringVisit1Date { get; set; }

        [StringLength(255)]
        public string MonitoringVisit1Result { get; set; }

        public DateTime? MonitoringVisit2Date { get; set; }

        [StringLength(255)]
        public string MonitoringVisit2Result { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Classroom> Classrooms { get; set; }

        public virtual Code_AdditionalSupportTypes Code_AdditionalSupportTypes { get; set; }
    }
}
