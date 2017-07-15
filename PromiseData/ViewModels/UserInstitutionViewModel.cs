using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PromiseData.Models;

namespace PromiseData.ViewModels
{
    public class UserInstitutionViewModel
    {
        public ApplicationUser User { get; set; }

        public Institution Institution { get; set; }

        public String UserName { get; set; }

        public String UserId { get; set; }

        public String InstitutionId { get; set; }

        public string[] ListInstitutionNames { get; set; }

        public IEnumerable<PromiseData.Models.Institution> Institutions { get; set; }
    }
}