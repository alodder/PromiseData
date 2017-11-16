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