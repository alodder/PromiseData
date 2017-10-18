using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using PromiseData.Models;

namespace PromiseData.ViewModels
{
    public class AdultFormViewModel
    {
        public int id { get; set; }

        [DisplayName("First Name")]
        public String NameFirst { get; set; }

        [DisplayName("Last Name")]
        public String NameLast { get; set; }

        public int ELDID { get; set; }

        public int AdultTypeID { get; set; }

        [DisplayName("Parent, Guardian, Foster Parent")]
        public String AdultType { get; set; }

        public IEnumerable<String> ParentTypes { get; set; }

        public int Age { get; set; }

        [Required]
        [DisplayName("Sex")]
        public char GenderID { get; set; }

        public IEnumerable<Code_Gender> Genders { get; set; }

        [DisplayName("Time residing with child")]
        public String ResidentialTime { get; set; }

        public IEnumerable<String> TimeTypes { get; set; }

        [Required]
        [DisplayName("Education")]
        public int Education_ID { get; set; }

        public IEnumerable<Code_Education> EducationTypes { get; set; }

        public String Employment { get; set; }

        public IEnumerable<RaceEthnicity> RaceEthnicityList { get; set; }
        
        //dictionary for checkbox values
        public Dictionary<int, bool> RaceDictionary { get; set; }

        public int familyId { get; set; }   
    }
}