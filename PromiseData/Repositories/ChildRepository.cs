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
        private UserRepository _userRepository;

        public ChildRepository( ApplicationDbContext _context)
        {
            App_context = _context;
            _userRepository = new UserRepository( App_context);
        }

        /**
        * Return true if Child belongsd to facility/HUB for User or User is admin
        **/
        public bool UserCanUpdateChild(ClaimsPrincipal User, int childID)
        {
            if (!(User.IsInRole("System Administrator") || User.IsInRole("Administrator")))
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
                int institutionId = _userRepository.GetUserInstitutionID( User);
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
    }
}