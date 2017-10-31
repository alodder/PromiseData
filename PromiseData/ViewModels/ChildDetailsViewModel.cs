using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.Linq;
using System.Web;
using PromiseData.Models;
using static System.DateTime;

namespace PromiseData.ViewModels
{
    public class ChildDetailsViewModel
    {
        [DisplayName("RowID")]
        [Required]
        public int ID { get; set; }

        [DisplayName("ELD ID")]
        public string ELD_ID { get; set; }

        [DisplayName("SSID")]
        public string SSID { get; set; }

        [DisplayName("Last Name")]
        [Required]
        public String LastName { get; set; }

        [Required]
        [DisplayName("First Name")]
        public String FirstName { get; set; }

        [DisplayName("Suffix")]
        public int GenerationCodeID { get; set; }
        public IEnumerable<Code_GenerationCode> Generations { get; set; }

        [DisplayName("Middle Name")]
        public String MiddleName { get; set; }

        [DisplayName("2nd Middle Name")]
        public String OtherMiddleName { get; set; }

        [DisplayName("Other First Name")]
        public String OtherFirstName { get; set; }

        [DisplayName("Other Last Name")]
        public String OtherLastName { get; set; }

        public int? OtherNameTypeID { get; set; }

        public int? Address_ID { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Date of Birth")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Birthdate { get; set; }

        [Required]
        [DisplayName("Sex")]
        public char GenderID { get; set; }

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

        public virtual Address Address { get; set; }

        public virtual Code_GenerationCode Generation { get; set; }

        public virtual Code_OtherNameType OtherNameType { get; set; }

        public virtual Code_Gender Gender { get; set; }

        public virtual Code_Language Language { get; set; }

        public virtual Code_ProgramSessionType ProgramSessionType { get; set; }

        public virtual Code_ExitReason ExitReason { get; set; }

        public virtual ICollection<Child_Facility> Child_Facilities { get; set; }

        public virtual ICollection<Child_Special_Needs> Child_Special_Needs { get; set; }

        public virtual ICollection<Child_IFSP> Child_IFSP { get; set; }

        public IEnumerable<RaceEthnicity> RaceEthnicityList { get; set; }
        public Dictionary<int, bool> RaceDictionary { get; set; }

        [DisplayName("First Language")]
        public int LanguageID { get; set; }

        public IEnumerable<Code_Language> Languages { get; set; }

        public IEnumerable<ChildRace> ChildRaces { get; set; }

        public int familyId { get; set; }
    }
}