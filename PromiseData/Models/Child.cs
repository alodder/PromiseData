namespace PromiseData.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Child")]
    public partial class Child
    {
        [Key]
        public int ID { get; set; }

        [StringLength(20)]
        public string ELD_ID { get; set; }

        [StringLength(20)]
        public string SSID { get; set; }

        [StringLength(255)]
        public string LastName { get; set; }

        [StringLength(255)]
        public string FirstName { get; set; }

        [StringLength(255)]
        public string MiddleName { get; set; }

        public int? GenerationCode_ID { get; set; }

        public int? OtherNameType_ID { get; set; }

        [StringLength(255)]
        public string OtherLastName { get; set; }

        [StringLength(255)]
        public string OtherFirstName { get; set; }

        [StringLength(255)]
        public string OtherMiddleName { get; set; }

        public int? Address_ID { get; set; }

        public DateTime? Birthdate { get; set; }

        [StringLength(1)]
        public string Gender_ID { get; set; }

        public bool Homeless { get; set; }

        public bool Migrant { get; set; }

        public bool HeadStartEligibility { get; set; }

        public int? Language_ID { get; set; }

        public bool EnglishLanguageLearner { get; set; }

        public int? ClassroomLanguage_ID { get; set; }

        [StringLength(255)]
        public string SchoolDistrictCode { get; set; }

        [StringLength(255)]
        public string ElementarySchoolCode { get; set; }

        public int? PriorEarlyLearningMonths { get; set; }

        public int? Program_ID { get; set; }

        [StringLength(255)]
        public string SchoolDistrictCatchment { get; set; }

        [StringLength(255)]
        public string ElementarySchoolCatchment { get; set; }

        [StringLength(255)]
        public string Parent_ProviderConsultations { get; set; }

        public DateTime? TransitionPlanConferenceDate { get; set; }

        public int? ExitReason_ID { get; set; }

        public int? FamilyID { get; set; }

        [ForeignKey("FamilyID")]
        public virtual Family Family { get; set; }

        [ForeignKey("Address_ID")]
        public virtual Address Address { get; set; }

        [ForeignKey("GenerationCode_ID")]
        public virtual Code_GenerationCode Generation { get; set; }

        [ForeignKey("OtherNameType_ID")]
        public virtual Code_OtherNameType OtherNameType { get; set; }

        [ForeignKey("Gender_ID")]
        public virtual Code_Gender Gender { get; set; }

        [ForeignKey("Language_ID")]
        public virtual Code_Language Language { get; set; }

        [ForeignKey("Program_ID")]
        public virtual Code_ProgramSessionType ProgramSessionType { get; set; }

        [ForeignKey("ExitReason_ID")]
        public virtual Code_ExitReason ExitReason { get; set; }

        public virtual ICollection<Child_Facility> Child_Facilities { get; set; }

        public virtual ICollection<Child_IFSP> Child_IFSP { get; set; }

    }
}
