using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PromiseData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace PromiseData.Repositories
{
    public class UserRepository
    {
        private ApplicationDbContext App_context;

        public UserRepository(ApplicationDbContext _context)
        {
            App_context = _context;
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
            int institutionId = Int32.Parse(claims.FirstOrDefault().Value);

            return institutionId;
        }
    }
}