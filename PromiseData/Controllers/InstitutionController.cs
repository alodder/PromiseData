using PromiseData.Models;
using PromiseData.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PromiseData.Controllers
{
    public class InstitutionController : Controller
    {
        private ApplicationDbContext _context;

        public InstitutionController()
        {
            _context = new ApplicationDbContext();
        }

        // GET: Institution
        [HttpGet]
        [Authorize(Roles = "Administrator, System Administrator")]
        public ActionResult Create()
        {
            var ViewModel = new InstitutionViewModel();
            ViewModel.States = _context.LU_State.ToList();
            ViewModel.Heading = "Add Institution";

            return View("InstitutionForm", ViewModel);
        }

        // POST: Institution
        [HttpPost]
        [Authorize(Roles = "Administrator, System Administrator")]
        public ActionResult Create(InstitutionViewModel viewModel)
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
                isHub = viewModel.isHub,
                isProvider = viewModel.isProvider
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
        [Authorize(Roles = "Administrator, System Administrator")]
        public ActionResult Edit(int id)
        {
            var institution = _context.Institutions.Single(i => i.Id == id);

            var director = _context.ContactAgents.SingleOrDefault(c => c.AgentId == institution.DirectorAgentId);
            var contact = _context.ContactAgents.SingleOrDefault(c => c.AgentId == institution.ContactAgentId);

            var mailingAddress = _context.Addresses.SingleOrDefault(c => c.ID == institution.MailingAddressId);
            var locationAddress = _context.Addresses.SingleOrDefault(c => c.ID == institution.LocationAddressId);

            var viewModel = new InstitutionViewModel {
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
                isHub = institution.isHub,
                isProvider = institution.isProvider,
                DirectorAgent = director,
                ContactAgent = contact,
                AddressMail = mailingAddress,
                AddressPhysical = locationAddress
            };

            viewModel.States = _context.LU_State.ToList();

            return View("InstitutionForm", viewModel);
        }

        //POST: Institution update/edit
        [HttpPost]
        [Authorize(Roles = "Administrator, System Administrator")]
        public ActionResult Update(InstitutionViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.States = _context.LU_State.ToList();
                return View("InstitutionForm", viewModel);
            }

            var institution = _context.Institutions.Single(i => i.Id == viewModel.Id);
            institution.LegalName = viewModel.LegalName;
            institution.Region = viewModel.Region;
            institution.BackboneOrg = viewModel.BackboneOrg;
            institution.WebAddress = viewModel.WebAddress;
            institution.isHub = viewModel.isHub;
            institution.isProvider = viewModel.isProvider;

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
        public ActionResult Search(InstitutionViewModel viewModel)
        {
            return RedirectToAction("Index", "Institution", new { query = viewModel.SearchTerm});
        }

        public ActionResult FilterHubs(string query = null)
        {
            var viewModel = _context.Institutions.ToList().Where(c => c.isHub == true);
            return View("Index", viewModel);
        }

        public ActionResult FilterProviders(string query = null)
        {
            var viewModel = _context.Institutions.ToList().Where(c => c.isProvider == true);
            return View("Index", viewModel);
        }

        // GET: Institution
        public ActionResult Index(string query = null)
        {
            var viewModel = _context.Institutions.ToList();
            if(!String.IsNullOrWhiteSpace(query))
            {
                var blurb = viewModel.Where(i => i.LegalName.Contains(query) ||
                                            i.BackboneOrg.Contains(query) ||
                                            i.ContactAgent.AgentName.Contains(query) ||
                                            i.ContactAgent1.AgentName.Contains(query) ||
                                            i.Address.City.Contains(query) ||
                                            i.Id == int.Parse(query));
                if(blurb.Count() > 0) 
                    viewModel = blurb.ToList();
            }
            
            return View(viewModel);
        }
    }
}