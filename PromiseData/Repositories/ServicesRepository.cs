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
    public class ServicesRepository
    {
        private ApplicationDbContext App_context;
        private UserRepository _userRepository;

        public ServicesRepository(ApplicationDbContext _context)
        {
            App_context = _context;
            _userRepository = new UserRepository( App_context);
        }

        /**
         * Return 'Services' (the Class session or school year model) that a user can see/administrate in order to enroll a child
         **/
        public IQueryable<Service> GetUserServices(ClaimsPrincipal User)
        {
            var services = App_context.Services.AsQueryable();

            if (!(User.IsInRole("System Administrator") || User.IsInRole("Administrator")))
            {
                int institutionId = _userRepository.GetUserInstitutionID( User);
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
    }
}