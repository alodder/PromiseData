using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PromiseData.ViewModels
{
    public class UserRoleViewModel
    {
        public String UserName { get; set; }

        public String Id { get; set; }

        public IEnumerable<Microsoft.AspNet.Identity.EntityFramework.IdentityUserRole> CurrentRoles { get; set; }

        public IEnumerable<Microsoft.AspNet.Identity.EntityFramework.IdentityRole> Roles { get; set; }
    }
}