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
    public class InstitutionRepository
    {
        private ApplicationDbContext App_context;
        private UserRepository _userRepository;

        public InstitutionRepository(ApplicationDbContext _context)
        {
            App_context = _context;
            _userRepository = new UserRepository( App_context);
        }

        /**
         * Return Institutions (Facility) that a user can see details about or see the Children enrolled
         **/
        public IQueryable<Institution> GetUserInstitutions(ClaimsPrincipal User)
        {
            var institutions = App_context.Institutions.AsQueryable();

            if (!(User.IsInRole("System Administrator")
                || User.IsInRole("Administrator")))
            {
                int institutionId = _userRepository.GetUserInstitutionID(User);
                var institution = App_context.Institutions.SingleOrDefault(i => i.Id == institutionId);

                institutions = App_context.Institutions.Where(i => i.ParentHubId == institutionId);

                /*if (institution.IsHub)
                {
                    institutions = App_context.Institutions.Where(i => i.ParentHubId == institutionId);
                }
                else if (institution.IsProvider)
                {
                    institutions = null;
                }*/
            }
            return institutions;
        }

        /**
         * Return Sites (Facility) that a user can see details about or see the Children enrolled
         **/
        public IQueryable<Institution> GetUserProviders(ClaimsPrincipal User)
        {
            var institutions = App_context.Institutions.Where(i => i.IsProvider == true).AsQueryable();

            if (!(User.IsInRole("System Administrator")
                || User.IsInRole("Administrator")))
            {
                int institutionId = _userRepository.GetUserInstitutionID(User);
                var institution = App_context.Institutions.SingleOrDefault(i => i.Id == institutionId);

                institutions = App_context.Institutions.Where(i => i.ParentHubId == institutionId);
            }
            return institutions;
        }

        /**
         * 
         **/
        public bool IsHub(int institutionId)
        {
            var institution = App_context.Institutions.Single(i => i.Id == institutionId);

            return institution.IsHub;
        }

        public bool IsProvider(int institutionId)
        {
            var institution = App_context.Institutions.Single(i => i.Id == institutionId);

            return institution.IsProvider;
        }

        /**
         * 
         **/
        public String GetInstitutionName(int institutionId)
        {
            var institution = App_context.Institutions.Single(i => i.Id == institutionId);

            return institution.LegalName;
        }
    }
}