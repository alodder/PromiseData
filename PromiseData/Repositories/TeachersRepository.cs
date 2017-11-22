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
    public class TeachersRepository
    {
        private ApplicationDbContext App_context;
        private UserRepository _userRepository;

        public TeachersRepository(ApplicationDbContext _context)
        {
            App_context = _context;
            _userRepository = new UserRepository(App_context);
        }

        /**
         * Return Teachers (Staff) that a user can see details about
         **/
        public IQueryable<Teacher> GetUserTeachers(ClaimsPrincipal User)
        {
            var teachers = App_context.Teachers.AsQueryable();

            if (!(User.IsInRole("System Administrator") || User.IsInRole("Administrator")))
            {
                int institutionId = _userRepository.GetUserInstitutionID( User);
                var institution = App_context.Institutions.SingleOrDefault(i => i.Id == institutionId);

                if (institution.IsHub)
                {
                    var providerids = App_context.Institutions.Where(i => i.ParentHubId == institutionId).Select(p => p.Id);
                    teachers = App_context.TeacherClasses.Where(tc => providerids.Contains(tc.Classroom.Facility.ProviderID)).Select(tc => tc.Teacher);
                }
                if (institution.IsProvider)
                {
                    var providerids = App_context.Institutions.Where(i => i.ParentHubId == institutionId).Select(p => p.Id);
                    teachers = App_context.TeacherClasses.Where(tc => providerids.Contains(tc.Classroom.Facility.ProviderID)).Select(tc => tc.Teacher);
                }
            }
            return teachers;
        }
    }
}