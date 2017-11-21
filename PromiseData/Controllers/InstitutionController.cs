using Advanced_Auditing.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PromiseData.Models;
using PromiseData.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;


namespace PromiseData.Controllers
{
    public class InstitutionController : Controller
    {
        private ApplicationDbContext _context;
        //private UserManager<ApplicationUser> UserManager;

        public InstitutionController()
        {
            _context = new ApplicationDbContext();
            //UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context));
        }

        // GET: Institution
        [HttpGet]
        [Audit]
        [Authorize(Roles = "Administrator, System Administrator")]
        public ActionResult Create()
        {
            var ViewModel = new InstitutionFormViewModel();
            ViewModel.States = _context.LU_State.ToList();
            ViewModel.Heading = "Add Institution";

            return View("InstitutionForm", ViewModel);
        }

        // POST: Institution
        [HttpPost]
        [Audit(AuditingLevel = 2)]
        [Authorize(Roles = "Administrator, System Administrator")]
        public ActionResult Create(InstitutionFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.States = _context.LU_State.ToList();
                return View("InstitutionForm", viewModel);
            }

            viewModel.DirectorAgent = _context.ContactAgents.Add(viewModel.DirectorAgent);
            /*if (viewModel.DirectorAgent.AgentId == 0)
                viewModel.DirectorAgent = null;*/
            viewModel.ContactAgent = _context.ContactAgents.Add(viewModel.ContactAgent);
            /*if (viewModel.ContactAgent.AgentId == 0)
                viewModel.ContactAgent = null;*/

            viewModel.AddressPhysical = _context.Addresses.Add(viewModel.AddressPhysical);
            viewModel.AddressMail = _context.Addresses.Add(viewModel.AddressMail);


            var institute = new Institution {
                LegalName = viewModel.LegalName,
                Region = viewModel.Region,
                BackboneOrg = viewModel.BackboneOrg,
                WebAddress = viewModel.WebAddress,
                ActiveDate = viewModel.ActiveDate,
                EndDate = viewModel.EndDate,
                IsHub = viewModel.IsHub,
                IsProvider = viewModel.IsProvider
            };

            if (viewModel.ContactAgent.AgentId != 0)
                institute.ContactAgentId = viewModel.ContactAgent.AgentId;
            if (viewModel.DirectorAgent.AgentId != 0)
                institute.DirectorAgentId = viewModel.ContactAgent.AgentId;

            if (viewModel.AddressPhysical.ID != 0)
                institute.LocationAddressId = viewModel.AddressPhysical.ID;
            if (viewModel.AddressMail.ID != 0)
                institute.MailingAddressId = viewModel.AddressMail.ID;

            _context.Institutions.Add(institute);

            _context.SaveChanges();

            return RedirectToAction("Index", "Institution");
        }

        // GET: Institution
        [HttpGet]
        [Audit]
        [Authorize(Roles = "Administrator, System Administrator")]
        public ActionResult Edit(int id)
        {
            var institution = _context.Institutions.Single(i => i.Id == id);

            var mailingAddress = _context.Addresses.SingleOrDefault(c => c.ID == institution.MailingAddressId);
            var locationAddress = _context.Addresses.SingleOrDefault(c => c.ID == institution.LocationAddressId);

            var viewModel = new InstitutionFormViewModel {
                Heading = "Edit Institution",
                Id = institution.Id,
                LegalName = institution.LegalName,
                Region = institution.Region, 
                BackboneOrg = institution.BackboneOrg,
                WebAddress = institution.WebAddress,
                DirectorAgentId = institution.DirectorAgentId,
                ContactAgentId = institution.ContactAgentId, 
                LocationAddressId = institution.LocationAddressId,
                MailingAddressId = institution.MailingAddressId,
                ActiveDate = institution.ActiveDate,
                EndDate = institution.EndDate,
                IsHub = institution.IsHub,
                IsProvider = institution.IsProvider,
                AddressMail = mailingAddress,
                AddressPhysical = locationAddress
            };

            viewModel.States = _context.LU_State.ToList();

            return View("InstitutionForm", viewModel);
        }

        //POST: Institution update/edit
        [HttpPost]
        [Audit(AuditingLevel = 2)]
        [Authorize(Roles = "Administrator, System Administrator")]
        public ActionResult Update(InstitutionFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.States = _context.LU_State.ToList();
                return View("InstitutionForm", viewModel);
            }

            var institution = _context.Institutions.Single(i => i.Id == viewModel.Id);
            institution.LicenseNumber = viewModel.LicenseNumber;
            institution.LegalName = viewModel.LegalName;
            institution.Region = viewModel.Region;
            institution.BackboneOrg = viewModel.BackboneOrg;
            institution.WebAddress = viewModel.WebAddress;
            institution.IsHub = viewModel.IsHub;
            institution.IsProvider = viewModel.IsProvider;
            institution.ActiveDate = viewModel.ActiveDate;

            var director = _context.ContactAgents.Single(i => i.AgentId == institution.DirectorAgentId);
            director.AgentName = viewModel.DirectorAgent.AgentName;
            director.AgentTitle = viewModel.DirectorAgent.AgentTitle;
            director.AgentEmail = viewModel.DirectorAgent.AgentEmail;
            director.AgentPhone = viewModel.DirectorAgent.AgentPhone;
            director.AgentFax = viewModel.DirectorAgent.AgentFax;

            var contact = _context.ContactAgents.Single(i => i.AgentId == institution.ContactAgentId);
            contact.AgentName = viewModel.ContactAgent.AgentName;
            contact.AgentTitle = viewModel.ContactAgent.AgentTitle;
            contact.AgentEmail = viewModel.ContactAgent.AgentEmail;
            contact.AgentPhone = viewModel.ContactAgent.AgentPhone;
            contact.AgentFax = viewModel.ContactAgent.AgentFax;

            var location = _context.Addresses.Single(i => i.ID == institution.LocationAddressId);
            location.Address1 = viewModel.AddressPhysical.Address1;
            location.Address2 = viewModel.AddressPhysical.Address2;
            location.Address3 = viewModel.AddressPhysical.Address3;
            location.City = viewModel.AddressPhysical.City;
            location.State_ID = viewModel.AddressPhysical.State_ID;
            location.ZipCode = viewModel.AddressPhysical.ZipCode;

            var mailing = _context.Addresses.Single(i => i.ID == institution.MailingAddressId);
            mailing.Address1 = viewModel.AddressMail.Address1;
            mailing.Address2 = viewModel.AddressMail.Address2;
            mailing.Address3 = viewModel.AddressMail.Address3;
            mailing.City = viewModel.AddressMail.City;
            mailing.State_ID = viewModel.AddressMail.State_ID;
            mailing.ZipCode = viewModel.AddressMail.ZipCode;

            _context.SaveChanges();

            return RedirectToAction("Index", "Institution");
        }

        [HttpPost]
        [Authorize]
        public ActionResult Search(InstitutionFormViewModel viewModel)
        {
            return RedirectToAction("Index", "Institution", new { query = viewModel.SearchTerm});
        }

        [Authorize]
        public ActionResult FilterHubs(string query = null)
        {
            var viewModel = new InstitutionsViewModel();
            viewModel.Institutions = _context.Institutions.ToList().Where(c => c.IsHub == true);

            if (User.IsInRole("System Administrator") || User.IsInRole("Administrator"))
            {
                viewModel.CanAdd = true;
                viewModel.CanEdit = true;
                viewModel.CanDelete = true;
                viewModel.CanView = true;
            }
            
            return View("Index", viewModel);
        }

        [Authorize]
        public ActionResult FilterProviders(string query = null)
        {
            var viewModel = new InstitutionsViewModel();
            viewModel.Institutions = _context.Institutions.ToList().Where(c => c.IsProvider == true);

            if (User.IsInRole("System Administrator") || User.IsInRole("Administrator"))
            {
                viewModel.CanAdd = true;
                viewModel.CanEdit = true;
                viewModel.CanDelete = true;
                viewModel.CanView = true;
            }

            return View("Index", viewModel);
        }

        //GET: Institution Details
        [Authorize]
        public ActionResult Details(int id)
        {
            var viewModel = new InstitutionFormViewModel();

            viewModel = UserRoleCheck(viewModel);                

            var institution = _context.Institutions.SingleOrDefault(i => i.Id == id);
            viewModel.DirectorAgentId = institution.DirectorAgentId;
            viewModel.ContactAgentId = institution.ContactAgentId;
            viewModel.LocationAddressId = institution.LocationAddressId;
            viewModel.MailingAddressId = institution.MailingAddressId;

            viewModel.DirectorAgent = _context.ContactAgents.SingleOrDefault(d => d.AgentId == institution.DirectorAgentId);
            viewModel.ContactAgent = _context.ContactAgents.SingleOrDefault(d => d.AgentId == institution.ContactAgentId);
            viewModel.AddressPhysical = _context.Addresses.SingleOrDefault(d => d.ID == institution.LocationAddressId);
            viewModel.AddressMail = _context.Addresses.SingleOrDefault(d => d.ID == institution.MailingAddressId);
            viewModel.Agents = _context.ContactAgents.Where(a => a.InstitutionId == id).ToList();

            viewModel.Id = institution.Id;
            viewModel.LegalName = institution.LegalName;
            viewModel.Region = institution.Region;
            viewModel.BackboneOrg = institution.BackboneOrg;
            viewModel.WebAddress = institution.WebAddress;
            viewModel.ActiveDate = institution.ActiveDate;
            viewModel.EndDate = institution.EndDate;

            viewModel.IsProvider = institution.IsProvider;
            viewModel.IsHub = institution.IsHub;

            //Populate Hub with children
            if (institution.IsHub)
            {
                viewModel.Providers = _context.Institutions.Where(i => i.ParentHubId == id).ToList();
            }

            //Populate Provider with children (Facilities/Sites)
            if (institution.IsProvider)
            {
                viewModel.Sites = _context.Facilities.Where(i => i.ProviderID == id).ToList();
                viewModel.ParentHubId = institution.ParentHubId.GetValueOrDefault();
                viewModel.ParentHub = _context.Institutions.SingleOrDefault(i => i.Id == viewModel.ParentHubId);
            }

            return View( viewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Administrator, System Administrator")]
        public ActionResult Index(string query = null)
        { 
            var viewModel = new InstitutionsViewModel();

            //var user = UserManager.FindById( userAndInstitution.UserId); //UserManager.Users.Single(a => a.Id == userAndRole.Id);
            ClaimsIdentity identity = (ClaimsIdentity)User.Identity;// UserManager.CreateIdentity(User, DefaultAuthenticationTypes.ApplicationCookie);

            var claims = (from c in identity.Claims
                          where c.Type == "Institution"
                          select c);

            viewModel.Institutions = _context.Institutions.ToList();

            if(!(User.IsInRole("System Administrator") || User.IsInRole("Administrator")))
            {
                viewModel.Institutions = viewModel.Institutions.Where(i => i.Id == Int32.Parse( claims.FirstOrDefault().Value));
            }

            if (!String.IsNullOrWhiteSpace(query))
            {
                var querylow = query.ToLower();
                var blurb = viewModel.Institutions.Where(i => 
                                            (i.LegalName.ToLower() ?? "").Contains(querylow) ||
                                            (i.BackboneOrg ?? "").Contains(querylow) ||
                                            (i.Region ?? "").Contains(querylow) ||
                                            (i.LicenseNumber ?? "").Contains(querylow)
                                            );
                
                viewModel.Institutions = blurb.ToList();
                viewModel.SearchTerm = query;
            }

            if (User.IsInRole("Administrator") || User.IsInRole("System Administrator"))
            {
                viewModel.CanAdd = true;
                viewModel.CanEdit = true;
                viewModel.CanDelete = true;
            }

            return View( viewModel);
        }

        private InstitutionFormViewModel UserRoleCheck(InstitutionFormViewModel viewModel)
        {
            if (User.IsInRole("System Administrator"))
            {
                viewModel.CanEdit = true;
                viewModel.CanDelete = true;
                viewModel.CanView = true;
            }
            if (User.IsInRole("Administrator"))
            {
                viewModel.CanEdit = true;
                viewModel.CanDelete = true;
                viewModel.CanView = true;
            }
            if (User.IsInRole("Hub"))
            {
                viewModel.CanEdit = true;
                viewModel.CanView = true;
            }
            if (User.IsInRole("Provider"))
            {
                viewModel.CanView = true;
            }

            return viewModel;
        }

    }
}