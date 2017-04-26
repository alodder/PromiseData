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

        public int ELDID { get; set; }

        public int AdultTypeID { get; set; }

        public String AdultType { get; set; }

        public int Age { get; set; }

        [Required]
        [DisplayName("Sex")]
        public int GenderID { get; set; }

        public IEnumerable<Code_Gender> Genders { get; set; }

        public String ResidentialTime { get; set; }

        public IEnumerable<String> TimeTypes { get; set; }

        [Required]
        [DisplayName("Education")]
        public int Education_ID { get; set; }

        public IEnumerable<Code_Education> EducationTypes { get; set; }

        public String Employment { get; set; }
    }
}