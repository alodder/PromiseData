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
    public class InstitutionFormViewModel
    {
        public int Id { get; set; }

        public Boolean CanView { get; set; }

        public Boolean CanEdit { get; set; }

        public Boolean CanDelete { get; set; }

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
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ActiveDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DisplayName("Date Ended")]
        public DateTime? EndDate { get; set; }

        [DisplayName("Hub")]
        public bool IsHub { get; set; }

        [DisplayName("Operator")]
        public bool IsProvider { get; set; }

        [DisplayName("Parent Hub")]
        public int ParentHubId { get; set; }

        [DisplayName("License #")]
        public string LicenseNumber { get; set; }

        [DisplayName("Operator Type")]
        public string OperatorType { get; set; }

        public List<String> OperatorTypes {
            get
            {
                List < String > types = new List<String>();
                types.Add("Child care provider(center and home)");
                types.Add("Community-based organization that provides a preschool program");
                types.Add("Public School");
                types.Add("Education Service District");
                types.Add("Head Start / OPK program");
                types.Add("Private preschool");
                types.Add("Public charter school");
                types.Add("Relief Nursery");
                types.Add("Other");
                return types;
            }
        }

        public IEnumerable<Institution> Providers { get; set; }

        public IEnumerable<Facility> Sites { get; set; }

        public IEnumerable<ContactAgent> Agents { get; set; }

        public IEnumerable<LU_State> States { get; set; }

        public virtual ContactAgent DirectorAgent { get; set; }

        public virtual ContactAgent ContactAgent { get; set; }

        public virtual Address AddressMail { get; set; }

        public virtual Address AddressPhysical { get; set; }

        public virtual Institution ParentHub { get; set; }

    }
}