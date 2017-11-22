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
using PromiseData.ViewModels;
using PromiseData.Repositories;

namespace PromiseData.Controllers
{
    public class WaiverRequestsController : Controller
    {
        private ApplicationDbContext db;
        private WaiverRepository _waiverRepository;
        private SitesRepository _sitesRepository;
        private TeachersRepository _teachersRepository;

        public WaiverRequestsController()
        {
            db = new ApplicationDbContext();
            _waiverRepository = new WaiverRepository( db);
            _sitesRepository = new SitesRepository( db);
            _teachersRepository = new TeachersRepository( db);
        }




        [Authorize]
        public ActionResult Index()
        {
            WaiversProcessViewModel viewModel = new WaiversProcessViewModel();

            if( User.IsInRole("System Administrator") || User.IsInRole("Administrator"))
            {
                viewModel.CanDelete = true;
                viewModel.CanEdit = true;
            }

            viewModel.WaiverRequests = _waiverRepository.getWaiverRequests( (ClaimsPrincipal)User).Include(w => w.Site).Include(w => w.Staff);

            return View( viewModel);
        }

        [Authorize]
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

        [Authorize]
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
            ViewBag.SiteID = new SelectList(_sitesRepository.GetUserSites((ClaimsPrincipal)User), "ID", "Description");
            ViewBag.StaffID = new SelectList(_teachersRepository.GetUserTeachers((ClaimsPrincipal)User), "ID", "NameLast");
            return View();
        }

        // POST: WaiverRequests/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
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

            ViewBag.SiteID = new SelectList(_sitesRepository.GetUserSites((ClaimsPrincipal)User), "ID", "Description", waiverRequest.SiteID);
            ViewBag.StaffID = new SelectList(_teachersRepository.GetUserTeachers( (ClaimsPrincipal)User), "ID", "NameLast", waiverRequest.StaffID);
            return View(waiverRequest);
        }

        // GET: WaiverRequests/Edit/5
        [Authorize]
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
            ViewBag.SiteID = new SelectList(_sitesRepository.GetUserSites( (ClaimsPrincipal)User), "ID", "Description", waiverRequest.SiteID);
            ViewBag.StaffID = new SelectList(_teachersRepository.GetUserTeachers((ClaimsPrincipal)User), "ID", "NameLast", waiverRequest.StaffID);
            return View(waiverRequest);
        }

        // POST: WaiverRequests/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
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
            ViewBag.SiteID = new SelectList(_sitesRepository.GetUserSites((ClaimsPrincipal)User), "ID", "Description", waiverRequest.SiteID);
            ViewBag.StaffID = new SelectList(_teachersRepository.GetUserTeachers((ClaimsPrincipal)User), "ID", "NameLast", waiverRequest.StaffID);
            return View(waiverRequest);
        }

        // GET: WaiverRequests/Delete/5
        [Authorize]
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
        [Authorize]
        [Audit(AuditingLevel = 2)]
        public ActionResult DeleteConfirmed(int id)
        {
            WaiverRequest waiverRequest = db.WaiverRequests.Find(id);
            db.WaiverRequests.Remove(waiverRequest);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
