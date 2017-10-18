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
    public class ChildFormViewModel
    {
        [DisplayName("RowID")]
        [Required]
        public int ID { get; set; }

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

        [DisplayName("Other Last Name")]
        public String OtherLastName { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Date of Birth")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        [Required]
        [DisplayName("Sex")]
        public char GenderID { get; set; }

        public IEnumerable<Code_Gender> Genders { get; set; }

        [DisplayName("Race/Ethnicity")]
        public String RaceEthnicityID { get; set; }

        public IEnumerable<RaceEthnicity> RaceEthnicityList { get; set; }
        public Dictionary<int, bool> RaceDictionary { get; set; }

        [DisplayName("First Language")]
        public int LanguageID { get; set; }

        public IEnumerable<Code_Language> Languages { get; set; }

        public int familyId { get; set; }
    }
}