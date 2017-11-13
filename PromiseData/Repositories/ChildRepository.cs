using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PromiseData.Models;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace PromiseData.Repositories
{
    public class ChildRepository
    {
        private ApplicationDbContext App_context;
        private static IdentityStoreDbContext Identity_context;

        private UserManager<ApplicationUser> UserManager;
        private RoleManager<IdentityRole> RoleManager;

        public ChildRepository( ApplicationDbContext _context)
        {
            App_context = _context;
            Identity_context = new IdentityStoreDbContext();

            UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(Identity_context));
            RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(Identity_context));
        }

        private ApplicationUser GetUser()
        {
            var User = UserManager.FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            return User;
        }

        /**
        * Return true if Child belongsd to facility/HUB for User or User is admin
        **/
        private bool UserCanUpdateChild(int childID)
        {
            var User = GetUser();
            if ((User.Roles.Select(r => r.RoleId).Contains( RoleManager.FindByName("System Administrator").Id))
                || (User.Roles.Select(r => r.RoleId).Contains(RoleManager.FindByName("Administrator").Id)))
            {
                return true;
            }
            var children = App_context.Children;
            if (children.Select(c => c.ID).Contains(childID))
            {
                return true;
            }
            return false;
        }


        /**
         * Retrieve Identity Claim for user for 'Institution' which holds an ID in its value field
         **/
        private int GetUserInstitutionID( ClaimsPrincipal User)
        {
            ClaimsIdentity identity = (ClaimsIdentity)User.Identity;

            var claims = (from c in identity.Claims
                          where c.Type == "Institution"
                          select c);
            int institutionId = Int32.Parse(claims.FirstOrDefault().Value);

            return institutionId;
        }


        /**
         * Return an IQueryable of Children that the User has rights to see.
         * If the user is an administrator, return all children.
         * Otherwise return the children at sites that belong to the user's Provider or HUB
         * 
         * IQueryable is lighter on memory than IEnumerable using this strategy
         * https://stackoverflow.com/questions/2876616/returning-ienumerablet-vs-iqueryablet
         **/
        public IQueryable<Child> GetUserChildren(ClaimsPrincipal User)
        {
            var children = App_context.Children.AsQueryable();

            if (!(User.IsInRole("System Administrator") || User.IsInRole("Administrator")))
            {
                int institutionId = GetUserInstitutionID( User);
                var institution = App_context.Institutions.SingleOrDefault(i => i.Id == institutionId);

                if (institution.IsHub)
                {
                    var providers = App_context.Institutions.Where(i => i.ParentHubId == institutionId).Select(p => p.Id);

                    //should assume children not enrolled, retrieve by column facility_id in child table
                    var childFacilities = App_context.ChildFacilities.Where(f => providers.Contains(f.Facility.ProviderID)).Select(c => c.ChildID);
                    children = children.Where(c => childFacilities.Contains(c.ID));
                }
                if (institution.IsProvider)
                {
                    var childFacilities = App_context.ChildFacilities.Where(f => f.Facility.ProviderID == institutionId).Select(c => c.ChildID);
                    children = children.Where(c => childFacilities.Contains(c.ID));
                }
            }
            return children;
        }


        /**
         * Return 'Services' (the Class session or school year model) that a user can see/administrate in order to enroll a child
         **/
        private IQueryable<Service> GetUserServices(ClaimsPrincipal User)
        {
            var services = App_context.Services.AsQueryable();

            if (!(User.IsInRole("System Administrator") || User.IsInRole("Administrator")))
                {
                int institutionId = GetUserInstitutionID( User);
                var institution = App_context.Institutions.SingleOrDefault(i => i.Id == institutionId);

                if (institution.IsHub)
                {
                    var providerids = App_context.Institutions.Where(i => i.ParentHubId == institutionId).Select(p => p.Id);

                    var facilityids = App_context.Facilities.Where(f => providerids.Contains(f.ProviderID)).Select(f => f.ID);
                    var classrooms = App_context.Classrooms.Where(c => facilityids.Contains(c.Facility_ID.GetValueOrDefault())).Select(c => c.ID);
                    services = services.Where(s => classrooms.Contains(s.ID));
                }
                if (institution.IsProvider)
                {
                    var childFacilities = App_context.ChildFacilities.Where(f => f.Facility.ProviderID == institutionId).Select(c => c.ChildID);
                    services = services.Where(s => childFacilities.Contains(s.ID));
                }
            }
            return services;
        }

        /**
         * Return Sites (Facility) that a user can see details about or see the Children enrolled
         **/
        private IQueryable<Facility> GetUserSites(ClaimsPrincipal User)
        {
            var sites = App_context.Facilities.AsQueryable();

            if (!(User.IsInRole("System Administrator") 
                || User.IsInRole("Administrator")))
            {
                int institutionId = GetUserInstitutionID( User);
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

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}