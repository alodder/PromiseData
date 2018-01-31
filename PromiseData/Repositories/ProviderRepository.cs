using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PromiseData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.Net;

namespace PromiseData.Repositories
{
    public class ProviderRepository
    {
        private ApplicationDbContext App_context;
        private UserRepository _userRepository;

        public ProviderRepository(ApplicationDbContext _context)
        {
            App_context = _context;
            _userRepository = new UserRepository( App_context);
        }

        /**
         * Return Providers (Facility) that a user can see details about or see the Children enrolled
         **/
        public IQueryable<Facility> GetUserProviders(ClaimsPrincipal User)
        {
            var providers = App_context.Facilities.AsQueryable();

            if (!(User.IsInRole("System Administrator")
                || User.IsInRole("Administrator")))
            {
                int institutionId = _userRepository.GetUserInstitutionID(User);
                var provider = App_context.Institutions.SingleOrDefault(i => i.Id == institutionId);

                providers = App_context.Facilities.Where(i => i.ProviderID == institutionId);
            }
            return providers;
        }

        /**
         * Return Providers (Facility) that a user can see details about or see the Children enrolled
         **/
        public IQueryable<Facility> GetOperatorProviders(ClaimsPrincipal User, int OperatorId)
        {
            var providers = App_context.Facilities.Where(i => i.ProviderID == OperatorId).AsQueryable();
           
            return providers;
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