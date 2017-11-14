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
    public class SitesRepository
    {
        private ApplicationDbContext App_context;
        private UserRepository _userRepository;

        public SitesRepository(ApplicationDbContext _context)
        {
            App_context = _context;
            _userRepository = new UserRepository(App_context);
        }

        /**
         * Return Sites (Facility) that a user can see details about or see the Children enrolled
         **/
        public IQueryable<Facility> GetUserSites(ClaimsPrincipal User)
        {
            var sites = App_context.Facilities.AsQueryable();

            if (!(User.IsInRole("System Administrator")
                || User.IsInRole("Administrator")))
            {
                int institutionId = _userRepository.GetUserInstitutionID(User);
                var institution = App_context.Institutions.SingleOrDefault(i => i.Id == institutionId);

                if (institution.IsHub)
                {
                    var providerids = App_context.Institutions.Where(i => i.ParentHubId == institutionId).Select(p => p.Id);

                    sites = App_context.Facilities.Where(f => providerids.Contains(f.ProviderID));
                }
                if (institution.IsProvider)
                {
                    sites = App_context.Facilities.Where(f => f.ProviderID == institutionId);
                }
            }
            return sites;
        }

        public bool UserCanEditSite(ClaimsPrincipal User, int siteId)
        {
            if (User.IsInRole("System Administrator") || User.IsInRole("Administrator"))
            {
                return true;
            }

            var institutionid = _userRepository.GetUserInstitutionID( User);
            var site = App_context.Facilities.Single(f => f.ID == siteId);

            if (User.IsInRole("Hub")
                && (institutionid == site.Provider.ParentHubId))
            {
                return true;
            }

            if (User.IsInRole("Provider") 
                && (institutionid == site.Provider.Id))
            {
                return true;
            }
            return false;
        }
    }
}