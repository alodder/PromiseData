using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PromiseData.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace PromiseData.ViewModels
{
    public class UserFormViewModel
    {
        public ApplicationUser User { get; set; }

        public Institution Institution { get; set; }

        public String UserName { get; set; }

        public String UserId { get; set; }

        public String InstitutionId { get; set; }

        public string[] ListInstitutionNames { get; set; }

        public IEnumerable<Institution> Institutions { get; set; }

        public IEnumerable<IdentityUserRole> CurrentRoles { get; set; }

        public string[] RoleNames { get; set; }

        public string[] SelectedRoleNames { get; set; }

        public IEnumerable<IdentityRole> Roles { get; set; }
    }
}