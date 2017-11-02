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
        public Boolean CanView { get; set; }
        public Boolean CanEdit { get; set; }
        public Boolean CanDelete { get; set; }

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

        [DisplayName("Second Middle Name")]
        public String OtherMiddleName { get; set; }

        [DisplayName("Other First Name")]
        public String OtherFirstName { get; set; }

        [DisplayName("Other Last Name")]
        public String OtherLastName { get; set; }

        [DisplayName("Other Name")]
        public int? OtherNameTypeID { get; set; }

        [DisplayName("Address")]
        public int? Address_ID { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Date of Birth")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Birthdate { get; set; }

        [Required]
        [DisplayName("Gender")]
        public char GenderID { get; set; }

        public bool Homeless { get; set; }

        public bool Migrant { get; set; }

        [DisplayName("Head Start Eligibility")]
        public bool HeadStartEligibility { get; set; }

        [DisplayName("Language")]
        public int? Language_ID { get; set; }

        [DisplayName("English Language Learner")]
        public bool EnglishLanguageLearner { get; set; }

        [DisplayName("Classroom Language")]
        public int? ClassroomLanguage_ID { get; set; }

        [DisplayName("School District Code")]
        public string SchoolDistrictCode { get; set; }

        [DisplayName("Elementary School Code")]
        public string ElementarySchoolCode { get; set; }

        [DisplayName("Prior Early Learning Months")]
        public int? PriorEarlyLearningMonths { get; set; }

        [DisplayName("Program")]
        public int? Program_ID { get; set; }

        [DisplayName("School District Catchment")]
        public string SchoolDistrictCatchment { get; set; }

        [DisplayName("Elementary School Catchment")]
        public string ElementarySchoolCatchment { get; set; }

        [DisplayName("Parent Provider Consultations")]
        public string Parent_ProviderConsultations { get; set; }

        [DisplayName("Transition Plan ConferenceDate")]
        public DateTime? TransitionPlanConferenceDate { get; set; }

        [DisplayName("Exit Reason")]
        public int? ExitReason_ID { get; set; }

        [DisplayName("Family")]
        public int FamilyID { get; set; }

        public Family Family { get; set; }

        public virtual IEnumerable<Adult> Adults { get; set; }

        public virtual Address Address { get; set; }

        public virtual Code_GenerationCode Generation { get; set; }

        public virtual Code_OtherNameType OtherNameType { get; set; }

        public virtual Code_Gender Gender { get; set; }

        public virtual Code_Language Language { get; set; }

        public virtual Code_ProgramSessionType ProgramSessionType { get; set; }

        public virtual Code_ExitReason ExitReason { get; set; }

        public virtual IEnumerable<Child_Facility> Child_Facilities { get; set; }

        public virtual IEnumerable<Child_Special_Needs> Child_Special_Needs { get; set; }

        public virtual IEnumerable<Child_IFSP> Child_IFSP { get; set; }

        //List of all Possible Special Needs
        public virtual IEnumerable<Special_Needs> Special_Needs { get; set; }

        [DisplayName("Child Special Needs")]
        //dictionary for checkbox values
        public Dictionary<int, bool> MySpecialNeeds { get; set; }

        //List of all Possible Code_IFSP
        public virtual IEnumerable<Code_IFSP> IFSPs { get; set; }

        [DisplayName("Child IFSP")]
        //dictionary for checkbox values
        public Dictionary<int, bool> MyIFSP { get; set; }

        public IEnumerable<RaceEthnicity> RaceEthnicityList { get; set; }
        public Dictionary<int, bool> RaceDictionary { get; set; }

        [DisplayName("First Language")]
        public int LanguageID { get; set; }

        public IEnumerable<Code_Language> Languages { get; set; }

        public IEnumerable<ChildRace> ChildRaces { get; set; }

    }
}