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
        public ActionResult Create()
        {
            var ViewModel = new InstitutionViewModel();
            return View(ViewModel);
        }

        // GET: Institution
        [HttpPost]
        public ActionResult Create(InstitutionViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("Create", viewModel);
            }

            var ViewModel = new InstitutionViewModel();

            viewModel.DirectorAgent = _context.ContactAgents.Add(viewModel.DirectorAgent);
            viewModel.ContactAgent = _context.ContactAgents.Add(viewModel.ContactAgent);

            viewModel.AddressPhysical = _context.Addresses.Add(viewModel.AddressPhysical);
            //viewModel.AddressMail = _context.Addresses.Add(viewModel.AddressMail);

            var institute = new Institution {
                LegalName = viewModel.LegalName,
                Region = viewModel.Region,
                BackboneOrg = viewModel.BackboneOrg,
                WebAddress = viewModel.WebAddress,
                DirectorAgentId = viewModel.DirectorAgent.AgentId,
                ContactAgentId = viewModel.ContactAgent.AgentId,
                LocationAddressId = viewModel.AddressPhysical.ID,
                MailingAddressId = null,
                ActiveDate = viewModel.ActiveDate,
                EndDate = viewModel.EndDate,
                isHub = viewModel.isHub,
                isProvider = viewModel.isProvider
            };

            _context.Institutions.Add(institute);

            _context.SaveChanges();

            return RedirectToAction("Index", "Institution");
        }

        // GET: Institution
        public ActionResult Index()
        {
            var viewModel = _context.Institutions.ToList();
            return View(viewModel);
        }
    }
}