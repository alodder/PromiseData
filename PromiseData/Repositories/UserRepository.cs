using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PromiseData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Security;

namespace PromiseData.Repositories
{
    public class UserRepository
    {
        private ApplicationDbContext App_context;
        private RoleManager<IdentityRole> RoleManager;

        public UserRepository(ApplicationDbContext _context)
        {
            App_context = _context;
            RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new IdentityStoreDbContext()));
        }

        public IEnumerable<string> GetUserRoles(ClaimsPrincipal User)
        {
            var roles = ((ClaimsIdentity)User.Identity).Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value);
            return roles;
        }

        public bool UserIsAdmin(ClaimsPrincipal User)
        {
            return (User.IsInRole("System Administrator") || User.IsInRole("Administrator"));
        }

        /**
         * Retrieve Identity Claim for user for 'Institution' which holds an ID in its value field
         **/
        public int GetUserInstitutionID(ClaimsPrincipal User)
        {
            ClaimsIdentity identity = (ClaimsIdentity)User.Identity;

            var claims = (from c in identity.Claims
                            where c.Type == "Institution"
                            select c);
            if (claims != null)
                return Int32.Parse(claims.FirstOrDefault().Value);

            throw new Exception("No User Institution Claims");
        }
    }
}