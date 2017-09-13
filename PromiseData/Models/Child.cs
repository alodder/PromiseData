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
        public int ID { get; set; }

        public int? ELD_ID { get; set; }

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

        public int? Gender_ID { get; set; }

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

        [ForeignKey("Address_ID")]
        public virtual Address Address { get; set; }
    }
}
