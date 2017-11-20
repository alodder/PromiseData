﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using PromiseData.Models;

namespace PromiseData.ViewModels
{
    public class ContactAgentViewModel
    {
        public int AgentId { get; set; }

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
        public int? InstitutionId { get; set; }
    }
}