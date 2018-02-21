using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PromiseData.Models;
using PromiseData.ViewModels;
using Advanced_Auditing.Models;

namespace PromiseData.Controllers
{
    [Authorize]
    [Audit]
    public class ContactAgentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ContactAgents
        public ActionResult Index()
        {
            var contactAgents = db.ContactAgents.Include(c => c.Institution);
            return View(contactAgents.ToList());
        }

        // GET: ContactAgents/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContactAgent contactAgent = db.ContactAgents.Find(id);
            if (contactAgent == null)
            {
                return HttpNotFound();
            }
            return View(contactAgent);
        }

        [Authorize(Roles = "Administrator, System Administrator, Hub, Provider")]
        public ActionResult Create(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContactAgentViewModel view = new ContactAgentViewModel();
            view.InstitutionID = id;
            return View("ContactAgentForm", view);
        }

        [Authorize(Roles = "Administrator, System Administrator, Hub, Provider")]
        [HttpGet]
        public ActionResult CreateForProvider(int id)
        {
            var viewModel = new ContactAgentViewModel
            {
                ProviderID = id
            };
            return View("ContactAgentForm", viewModel);
        }

        // POST: ContactAgents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Administrator, System Administrator, Hub, Provider")]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ContactAgentViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.InstitutionId = new SelectList(db.Institutions, "Id", "LegalName", viewModel.InstitutionID);
                return View("ContactAgentForm", viewModel);
            }

            var contactAgent = new ContactAgent
            {
                AgentName = viewModel.AgentName,
                AgentEmail = viewModel.AgentEmail,
                AgentFax = viewModel.AgentFax,
                AgentPhone = viewModel.AgentPhone,
                AgentTitle = viewModel.AgentTitle
            };

            db.ContactAgents.Add(contactAgent);
            db.SaveChanges();

            if (viewModel.ProviderID != null)
            {
                var facility = db.Facilities.FirstOrDefault(a => a.ID == viewModel.ProviderID);
                facility.ContactAgentID = contactAgent.AgentId;
                db.SaveChanges();
                return RedirectToAction("Details", "Facility", new { id = viewModel.ProviderID });
            } 
            else
            {
                contactAgent.InstitutionId = viewModel.InstitutionID;
                db.SaveChanges();
            }

            return RedirectToAction("Details", "Institution", new { id = contactAgent.InstitutionId });
        }

        [HttpGet]
        [Authorize(Roles = "Administrator, System Administrator, Hub, Provider")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ContactAgent contactAgent = db.ContactAgents.Find(id);
            if (contactAgent == null)
            {
                return HttpNotFound();
            }
            var viewModel = new ContactAgentViewModel
            {
                AgentId = contactAgent.AgentId,
                AgentName = contactAgent.AgentName,
                AgentEmail = contactAgent.AgentEmail,
                AgentFax = contactAgent.AgentFax,
                AgentPhone = contactAgent.AgentPhone,
                AgentTitle = contactAgent.AgentTitle,
                InstitutionID = contactAgent.InstitutionId
            };

            var provider = db.Facilities.FirstOrDefault(f => f.ContactAgentID == id);
            if(provider != null)
            {
                viewModel.ProviderID = provider.ProviderID;
            }
            

            return View("ContactAgentForm", viewModel);
        }

        // POST: ContactAgents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Administrator, System Administrator, Hub, Provider")]
        [ValidateAntiForgeryToken]
        public ActionResult Update(ContactAgentViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                //viewModel.InstitutionID = new SelectList(db.Institutions, "Id", "LegalName", contactAgent.InstitutionId);
                return View("ContactAgentForm", viewModel);
            }

            var contactAgent = db.ContactAgents.Find( viewModel.AgentId);
            if (contactAgent == null)
            {
                return HttpNotFound();
            }

            contactAgent.AgentName = viewModel.AgentName;
            contactAgent.AgentEmail = viewModel.AgentEmail;
            contactAgent.AgentFax = viewModel.AgentFax;
            contactAgent.AgentPhone = viewModel.AgentPhone;
            contactAgent.AgentTitle = viewModel.AgentTitle;
            
            db.Entry(contactAgent).State = EntityState.Modified;
            db.SaveChanges();

            if(contactAgent.InstitutionId > 0)
            {
                return RedirectToAction("Details", "Institution", new { id = contactAgent.InstitutionId });
            }
            else
            {
                var provider = db.Facilities.FirstOrDefault(p => p.ContactAgentID == contactAgent.AgentId);
                if(provider != null)
                {
                    return RedirectToAction("Details", "Facility", new { id = provider.ID });
                }
            }
            return RedirectToAction("Index");
        }

        // GET: ContactAgents/Delete/5
        [Authorize(Roles = "Administrator, System Administrator, Hub")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContactAgent contactAgent = db.ContactAgents.Find(id);
            if (contactAgent == null)
            {
                return HttpNotFound();
            }
            return View(contactAgent);
        }

        // POST: ContactAgents/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Administrator, System Administrator, Hub")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ContactAgent contactAgent = db.ContactAgents.Find(id);
            var institutionID = contactAgent.InstitutionId;

            db.ContactAgents.Remove(contactAgent);
            db.SaveChanges();
            return RedirectToAction("Details", "Institution", new { id = institutionID });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
