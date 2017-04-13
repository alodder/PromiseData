using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PromiseData.Models;

namespace PromiseData.ViewModels
{
    public class ChildFormViewModel
    {
        public String LastName { get; set; }
        public String FirstName { get; set; }
        public String MiddleName { get; set; }
        public String OtherMiddleName { get; set; }
        public String OtherLastName { get; set; }
        public DateTime Birthdate { get; set; }
        public int GenderID { get; set; }
        public IEnumerable<Code_Gender> Genders { get; set; }
        public String RaceEthnicityID { get; set; }
        public int LanguageID { get; set; }
    }
}