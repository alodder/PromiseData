using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using PromiseData.Models;
using System.Linq.Expressions;
using PromiseData.Controllers;
using System.Web.Mvc;

namespace PromiseData.ViewModels
{
    public class AdultFormViewModel
    {
        public int id { get; set; }

        public Boolean CanView { get; set; }

        public Boolean CanEdit { get; set; }

        public Boolean CanDelete { get; set; }

        public String Action
        {
            get
            {
                Expression<Func<AdultController, ActionResult>> update =
                    (c => c.Update(this));
                Expression<Func<AdultController, ActionResult>> create =
                    (c => c.Create(this));

                var action = (id != 0) ? update : create;
                return (action.Body as MethodCallExpression).Method.Name;
            }

        }

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

        public int FamilyID { get; set; }   
    }
}