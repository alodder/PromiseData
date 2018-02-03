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
    public class ContactAgentViewModel
    {
        public int AgentId { get; set; }

        public String Action
        {
            get
            {
                Expression<Func<ContactAgentsController, ActionResult>> update =
                    (c => c.Update(this));
                Expression<Func<ContactAgentsController, ActionResult>> create =
                    (c => c.Create(this));

                var action = (AgentId != 0) ? update : create;
                return (action.Body as MethodCallExpression).Method.Name;
            }
        }

        [Required]
        [DisplayName("Name")]
        public string AgentName { get; set; }

        [DisplayName("Title")]
        public string AgentTitle { get; set; }

        [DisplayName("Phone")]
        public string AgentPhone { get; set; }

        [DisplayName("Email")]
        public string AgentEmail { get; set; }

        [DisplayName("Fax")]
        public string AgentFax { get; set; }

        [DisplayName("Institution")]
        public int? InstitutionID { get; set; }

        [DisplayName("Provider")]
        public int? ProviderID { get; set; }
    }
}