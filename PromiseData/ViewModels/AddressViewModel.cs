using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using PromiseData.Models;
using System.Web.Mvc;
using System.Linq.Expressions;
using PromiseData.Controllers;

namespace PromiseData.ViewModels
{
    public class AddressViewModel
    {
        public int ID { get; set; }

        public String Action
        {
            get
            {
                Expression<Func<AddressController, ActionResult>> update =
                    (c => c.Update(this));
                Expression<Func<AddressController, ActionResult>> create =
                    (c => c.Create(this));

                var action = (ID != 0) ? update : create;
                return (action.Body as MethodCallExpression).Method.Name;
            }

        }

        [DisplayName("Address Type")]
        public int AddressType_ID { get; set; }

        public IEnumerable<Code_AddressType> AddressTypes { get; set; }

        [Required]
        [DisplayName("Address Line 1")]
        public string Address1 { get; set; }

        [DisplayName("Address Line 2")]
        public string Address2 { get; set; }

        [DisplayName("Address Line 3")]
        public string Address3 { get; set; }

        [DisplayName("City")]
        public string City { get; set; }

        [DisplayName("State")]
        public string State_ID { get; set; }

        public IEnumerable<LU_State> States { get; set; }

        [DisplayName("Zip Code")]
        public string ZipCode { get; set; }

        [Required]
        [DisplayName("County")]
        public string County { get; set; }

        public virtual Code_AddressType Code_AddressType { get; set; }

        public virtual LU_State LU_State { get; set; }
    }
}