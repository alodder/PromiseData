using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PromiseData.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace PromiseData.ViewModels
{
    public class UserFormViewModel
    {

        public Boolean CanView { get; set; }

        public Boolean CanEdit { get; set; }

        public ApplicationUser User { get; set; }

        public Institution Institution { get; set; }

        public String UserName { get; set; }

        [Key]
        public String UserId { get; set; }

        [DisplayName("Institution")]
        public String InstitutionId { get; set; }

        [DisplayName("Provider")]
        public String ProviderId { get; set; }

        public string[] ListInstitutionNames { get; set; }

        public IEnumerable<Institution> Institutions { get; set; }

        public string[] ListProviderNames { get; set; }

        public IEnumerable<Facility> Providers { get; set; }

        public IEnumerable<IdentityUserRole> CurrentRoles { get; set; }

        public string[] RoleNames { get; set; }

        public string[] SelectedRoleNames { get; set; }

        public IEnumerable<IdentityRole> Roles { get; set; }
    }
}