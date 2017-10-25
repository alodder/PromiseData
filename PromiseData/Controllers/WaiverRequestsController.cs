using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PromiseData.Models;
using Advanced_Auditing.Models;
using System.Security.Claims;

namespace PromiseData.Controllers
{
    public class WaiverRequestsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: WaiverRequests
        public ActionResult Index()
        {
            var waiverRequests = db.WaiverRequests.Include(w => w.Site).Include(w => w.Staff);
            return View(waiverRequests.ToList());
        }

        // GET: WaiverRequests/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WaiverRequest waiverRequest = db.WaiverRequests.Find(id);
            if (waiverRequest == null)
            {
                return HttpNotFound();
            }
            return View(waiverRequest);
        }

        // GET: WaiverRequests/Create
        [Audit(AuditingLevel = 2)]
        public ActionResult Create()
        {
            ViewBag.WaiverType = new SelectList(new List<SelectListItem>
                                {
                                    new SelectListItem { Selected = true, Text = "Site", Value = "Site"},
                                    new SelectListItem { Selected = false, Text = "Staff", Value = "Staff"}
                                });
            ViewBag.SparkLevels = new SelectList(new List<SelectListItem>
                                {
                                    new SelectListItem { Selected = true, Text = "Unlicensed", Value = "Unlicensed"},
                                    new SelectListItem { Selected = false, Text = "Licensed", Value = "Licensed"},
                                    new SelectListItem { Selected = false, Text = "Waiver", Value = "Waiver"},
                                    new SelectListItem { Selected = false, Text = "Portfolio Submitted (awaiting star designation)", Value = "Portfolio"},
                                    new SelectListItem { Selected = false, Text = "3 Star Rated", Value = "3star"}
                                });
            ViewBag.SiteID = new SelectList(db.Facilities, "ID", "Description");
            ViewBag.StaffID = new SelectList(db.Teachers, "ID", "NameLast");
            return View();
        }

        // POST: WaiverRequests/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Audit(AuditingLevel = 2)]
        public ActionResult Create([Bind(Include = "WaiverRequestID,waiverType,SiteID,SparkCurrent,StaffID,Qualification,AdditionalComments")] WaiverRequest waiverRequest)
        {
            if (ModelState.IsValid)
            {
                waiverRequest.RequestUpdated = DateTime.Now;
                db.WaiverRequests.Add(waiverRequest);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.SiteID = new SelectList(db.Facilities, "ID", "Description", waiverRequest.SiteID);
            ViewBag.StaffID = new SelectList(db.Teachers, "ID", "NameLast", waiverRequest.StaffID);
            return View(waiverRequest);
        }

        // GET: WaiverRequests/Edit/5
        [Audit(AuditingLevel = 2)]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WaiverRequest waiverRequest = db.WaiverRequests.Find(id);
            if (waiverRequest == null)
            {
                return HttpNotFound();
            }
            ViewBag.SiteID = new SelectList(GetUserSites(), "ID", "Description", waiverRequest.SiteID);
            ViewBag.StaffID = new SelectList(db.Teachers, "ID", "NameLast", waiverRequest.StaffID);
            return View(waiverRequest);
        }

        // POST: WaiverRequests/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Audit(AuditingLevel = 2)]
        public ActionResult Edit([Bind(Include = "WaiverRequestID,waiverType,SiteID,SparkCurrent,StaffID,Qualification,AdditionalComments")] WaiverRequest waiverRequest)
        {
            if (ModelState.IsValid)
            {
                waiverRequest.RequestUpdated = DateTime.Now;
                db.Entry(waiverRequest).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SiteID = new SelectList(db.Facilities, "ID", "Description", waiverRequest.SiteID);
            ViewBag.StaffID = new SelectList(db.Teachers, "ID", "NameLast", waiverRequest.StaffID);
            return View(waiverRequest);
        }

        // GET: WaiverRequests/Delete/5
        [Audit(AuditingLevel = 2)]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WaiverRequest waiverRequest = db.WaiverRequests.Find(id);
            if (waiverRequest == null)
            {
                return HttpNotFound();
            }
            return View(waiverRequest);
        }

        // POST: WaiverRequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Audit(AuditingLevel = 2)]
        public ActionResult DeleteConfirmed(int id)
        {
            WaiverRequest waiverRequest = db.WaiverRequests.Find(id);
            db.WaiverRequests.Remove(waiverRequest);
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

        private int GetUserInstitutionID()
        {
            ClaimsIdentity identity = (ClaimsIdentity)User.Identity;

            var claims = (from c in identity.Claims
                          where c.Type == "Institution"
                          select c);
            int institutionId = Int32.Parse(claims.FirstOrDefault().Value);

            return institutionId;
        }

        private IEnumerable<Facility> GetUserSites()
        {
            var sites = db.Facilities.AsQueryable();

            if (!(User.IsInRole("System Administrator") || User.IsInRole("Administrator")))
            {
                int institutionId = GetUserInstitutionID();
                var institution = db.Institutions.SingleOrDefault(i => i.Id == institutionId);

                if (institution.IsHub)
                {
                    var providerids = db.Institutions.Where(i => i.ParentHubId == institutionId).Select(p => p.Id);

                    sites = db.Facilities.Where(f => providerids.Contains(f.ProviderID));
                }
                if (institution.IsProvider)
                {
                    sites = db.Facilities.Where(f => f.ProviderID == institutionId);
                }
            }
            return sites;
        }

        private IEnumerable<Teacher> GetUserTeachers()
        {
            var teachers = db.Teachers.AsQueryable();

            if (!(User.IsInRole("System Administrator") || User.IsInRole("Administrator")))
            {
                int institutionId = GetUserInstitutionID();
                var institution = db.Institutions.SingleOrDefault(i => i.Id == institutionId);

                if (institution.IsHub)
                {
                    var providerids = db.Institutions.Where(i => i.ParentHubId == institutionId).Select(p => p.Id);
                    teachers = db.TeacherClasses.Where(tc => providerids.Contains( tc.Classroom.Facility.ProviderID)).Select(tc => tc.Teacher);
                }
                if (institution.IsProvider)
                {
                    var providerids = db.Institutions.Where(i => i.ParentHubId == institutionId).Select(p => p.Id);
                    teachers = db.TeacherClasses.Where(tc => providerids.Contains(tc.Classroom.Facility.ProviderID)).Select(tc => tc.Teacher);
                }
            }
            return teachers;
        }
    }
}
