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

        [DisplayName("Operator Facility Type")]
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

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DisplayName("Monitoring Visit Date #1")]
        public DateTime? MonitoringVisit1Date { get; set; }

        [DisplayName("Monitoring Visit Result #1")]
        [StringLength(255)]
        public string MonitoringVisit1Result { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DisplayName("Monitoring Visit Date #2")]
        public DateTime? MonitoringVisit2Date { get; set; }

        [DisplayName("Monitoring Visit Result #2")]
        [StringLength(255)]
        public string MonitoringVisit2Result { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        [DisplayName("Parent Provider")]
        public int ProviderID { get; set; }

        [StringLength(10)]
        [DisplayName("Child Care License")]
        public string LicenseNumber { get; set; }

        [DisplayName("Unlicensed")]
        public bool Unlicensed { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Classroom> Classrooms { get; set; }

        public virtual Code_AdditionalSupportTypes Code_AdditionalSupportTypes { get; set; }

        [ForeignKey("ProviderID")]
        public virtual Institution Provider { get; set; }

        public virtual ICollection<WaiverCurrent> WaiverCurrents { get; set; }

        public virtual ICollection<WaiverRequest> WaiverRequests { get; set; }

        public virtual ICollection<Child_Facility> Child_Facilities { get; set; }
    }
}