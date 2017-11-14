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
    public class ClassroomRepository
    {
        private ApplicationDbContext App_context;
        private UserRepository _userRepository;

        public ClassroomRepository(ApplicationDbContext _context)
        {
            App_context = _context;
            _userRepository = new UserRepository( App_context);
    }

        /**
         * 
         */
        public IQueryable<Classroom> GetUserClassrooms(ClaimsPrincipal User)
        {
            var classrooms = App_context.Classrooms.AsQueryable();

            if (!(User.IsInRole("System Administrator") || User.IsInRole("Administrator")))
            {
                int institutionId = _userRepository.GetUserInstitutionID( User);
                var institution = App_context.Institutions.SingleOrDefault(i => i.Id == institutionId);

                if (institution.IsHub)
                {
                    var providers = App_context.Institutions.Where(i => i.ParentHubId == institutionId).Select(p => p.Id);

                    //should assume children not enrolled, retrieve by column facility_id in child table
                    var childFacilities = App_context.ChildFacilities.Where(f => providers.Contains(f.Facility.ProviderID)).Select(c => c.ChildID);
                    classrooms = classrooms.Where(c => childFacilities.Contains(c.ID));
                }
                if (institution.IsProvider)
                {
                    var childFacilities = App_context.ChildFacilities.Where(f => f.Facility.ProviderID == institutionId).Select(c => c.ChildID);
                    classrooms = classrooms.Where(c => childFacilities.Contains(c.ID));
                }
            }
            return classrooms;
        }
    }
}