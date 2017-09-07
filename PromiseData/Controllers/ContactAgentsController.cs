using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PromiseData.Models;

namespace PromiseData.Controllers
{
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

        // GET: ContactAgents/Create
        public ActionResult Create(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.InstitutionId = id;
            return View();
        }

        // POST: ContactAgents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AgentId,AgentName,AgentTitle,AgentPhone,AgentEmail,AgentFax,InstitutionId")] ContactAgent contactAgent)
        {
            if (ModelState.IsValid)
            {
                db.ContactAgents.Add(contactAgent);
                db.SaveChanges();
                return RedirectToAction("Details", "Institution", contactAgent.InstitutionId);
            }

            ViewBag.InstitutionId = new SelectList(db.Institutions, "Id", "LegalName", contactAgent.InstitutionId);
            return View(contactAgent);
        }

        // GET: ContactAgents/Edit/5
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
            ViewBag.InstitutionId = new SelectList(db.Institutions, "Id", "LegalName", contactAgent.InstitutionId);
            return View(contactAgent);
        }

        // POST: ContactAgents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AgentId,AgentName,AgentTitle,AgentPhone,AgentEmail,AgentFax,InstitutionId")] ContactAgent contactAgent)
        {
            if (ModelState.IsValid)
            {
                db.Entry(contactAgent).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.InstitutionId = new SelectList(db.Institutions, "Id", "LegalName", contactAgent.InstitutionId);
            return View(contactAgent);
        }

        // GET: ContactAgents/Delete/5
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
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ContactAgent contactAgent = db.ContactAgents.Find(id);
            db.ContactAgents.Remove(contactAgent);
            db.SaveChanges();
            return RedirectToAction("Index");
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
