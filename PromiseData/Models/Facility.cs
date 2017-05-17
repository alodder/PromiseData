namespace PromiseData.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
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

        [DisplayName("Provider Facility Type")]
        [StringLength(24)]
        public string ProviderFacilityType { get; set; }

        [DisplayName("Turnover - Non-PPStaff")]
        [StringLength(255)]
        public string Turnover_NonPPStaff { get; set; }

        [DisplayName("Turnover Reasons - Non-PPStaff")]
        [StringLength(255)]
        public string TurnoverReasons_NonPPStaff { get; set; }

        [DisplayName("Transportation Services Offered")]
        public bool Transportation_services_offered { get; set; }

        [DisplayName("Number Children Receiving Transportation Service")]
        public int? ChildrenReceivingTransportationServices { get; set; }

        [DisplayName("Additional Family Supports")]
        public int? AdditionalChildFamilySupports_ID { get; set; }

        [DisplayName("Monitoring Visit Date #1")]
        public DateTime? MonitoringVisit1Date { get; set; }

        [DisplayName("Monitoring Visit Result #1")]
        [StringLength(255)]
        public string MonitoringVisit1Result { get; set; }

        [DisplayName("Monitoring Visit Date #2")]
        public DateTime? MonitoringVisit2Date { get; set; }

        [DisplayName("Monitoring Visit Result #2")]
        [StringLength(255)]
        public string MonitoringVisit2Result { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Classroom> Classrooms { get; set; }

        public virtual Code_AdditionalSupportTypes Code_AdditionalSupportTypes { get; set; }
    }
}