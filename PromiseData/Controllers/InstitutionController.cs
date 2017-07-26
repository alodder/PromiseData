﻿using PromiseData.Models;
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
                return View("InstitutionForm", viewModel);
            }

            var ViewModel = new InstitutionViewModel();

            viewModel.DirectorAgent = _context.ContactAgents.Add(viewModel.DirectorAgent);
            viewModel.ContactAgent = _context.ContactAgents.Add(viewModel.ContactAgent);

            viewModel.AddressPhysical = _context.Addresses.Add(viewModel.AddressPhysical);
            viewModel.AddressMail = _context.Addresses.Add(viewModel.AddressMail);

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

        [HttpPost]
        public ActionResult Search(InstitutionViewModel viewModel)
        {
            return RedirectToAction("Index", "Institutions", new { query = viewModel.SearchTerm});
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
                viewModel = viewModel.Where(i => i.LegalName.Contains(query) ||
                                                i.BackboneOrg.Contains(query) ||
                                                i.ContactAgent.AgentName.Contains(query) ||
                                                i.ContactAgent1.AgentName.Contains(query) ||
                                                i.Address.City.Contains(query) ||
                                                i.Id == int.Parse(query)).ToList();
            }
            
            return View(viewModel);
        }
    }
}