using PromiseData.Controllers;
using PromiseData.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace PromiseData.ViewModels
{
    public class InstitutionViewModel
    {
        public int Id { get; set; }

        public String Action
        {
            get
            {
                Expression<Func<InstitutionController, ActionResult>> update = 
                    (c => c.Update(this));
                Expression<Func<InstitutionController, ActionResult>> create =
                    (c => c.Create(this));

                var action = (Id != 0) ? update : create;
                return (action.Body as MethodCallExpression).Method.Name;
            }

        }

        public string Heading { get; set; }

        public string SearchTerm { get; set; }

        [DisplayName("Name")]
        public string LegalName { get; set; }

        [DisplayName("Region")]
        public string Region { get; set; }

        [DisplayName("Organization")]
        public string BackboneOrg { get; set; }

        [DisplayName("Web Address")]
        public string WebAddress { get; set; }

        [DisplayName("Director")]
        public int? DirectorAgentId { get; set; }

        [DisplayName("Contact")]
        public int? ContactAgentId { get; set; }

        [DisplayName("Location")]
        public int? LocationAddressId { get; set; }

        [DisplayName("Mailing Address")]
        public int? MailingAddressId { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Date Active")]
        [DisplayFormat(DataFormatString = "{0:D}")]
        public DateTime ActiveDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d}")]
        [DisplayName("Date Ended")]
        public DateTime? EndDate { get; set; }

        [DisplayName("Hub")]
        public bool isHub { get; set; }

        [DisplayName("Provider")]
        public bool isProvider { get; set; }

        public IEnumerable<LU_State> States { get; set; }

        public virtual ContactAgent DirectorAgent { get; set; }

        public virtual ContactAgent ContactAgent { get; set; }

        public virtual Address AddressMail { get; set; }

        public virtual Address AddressPhysical { get; set; }
    }
}