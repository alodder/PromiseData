using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using PromiseData.Controllers;
using PromiseData.Models;

namespace PromiseData.ViewModels
{
    public class FamilyViewModel
    {
        public int ID { get; set; }

        public String Action
        {
            get
            {
                Expression<Func<FamilyController, ActionResult>> update =
                    (c => c.Update(this));
                Expression<Func<FamilyController, ActionResult>> create =
                    (c => c.Create(this));

                var action = (ID != 0) ? update : create;
                return (action.Body as MethodCallExpression).Method.Name;
            }
        }

        [DisplayName("Household Size")]
        public int HouseholdSize { get; set; }

        [DisplayName("Children in home")]
        public int ChildrenInHome { get; set; }

        public decimal Income { get; set; }

        [DisplayName("Supplemental Nutrition Assistance Program")]
        public bool SNAP { get; set; }

        [DisplayName("Women, infants, and children")]
        public bool WIC { get; set; }

        [DisplayName("Temporary Assistance for Needy Families")]
        public bool TANF { get; set; }

        [DisplayName("Supplemental Security Income")]
        public bool SSI { get; set; }

        [DisplayName("Free or Reduced Lunch Program")]
        public bool FRLP { get; set; }

        [DisplayName("Head Start")]
        public bool HS { get; set; }

        [DisplayName("Family Monthly Pay for Additional Services")]
        public int MonthlyCostAdditionalServices { get; set; }
    }
}