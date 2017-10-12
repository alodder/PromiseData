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

namespace PromiseData.Controllers
{
    public class WaiverCurrentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: WaiverCurrents
        public ActionResult Index()
        {
            WaiversProcessViewModel viewModel = new WaiversProcessViewModel();

            viewModel.WaiverCurrents = db.WaiverCurrents.Include(w => w.Site).Include(w => w.Staff);
            viewModel.WaiverRequests = db.WaiverRequests.Include(w => w.Site).Include(w => w.Staff);

            return View( viewModel);
        }

        // GET: WaiverCurrents/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WaiverCurrent waiverCurrent = db.WaiverCurrents.Find(id);
            if (waiverCurrent == null)
            {
                return HttpNotFound();
            }
            return View(waiverCurrent);
        }

        // GET: WaiverCurrents/Create
        public ActionResult Create()
        {
            ViewBag.SiteID = new SelectList(db.Facilities, "ID", "ProviderFacilityType");
            ViewBag.StaffID = new SelectList(db.Teachers, "ID", "TeacherIDNumber");
            return View();
        }

        // POST: WaiverCurrents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "WaiverCurrentID,WaiverType,SiteID,SparkCurrent,StaffID,Qualification,Development,Credits,TrainingHours,NineHundredServiceHours,ServiceHourImpact,ServiceHourImpactOther,ServiceHourCount,AdditionalComments,Unsatisfied")] WaiverCurrent waiverCurrent)
        {
            if (ModelState.IsValid)
            {
                db.WaiverCurrents.Add(waiverCurrent);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.SiteID = new SelectList(db.Facilities, "ID", "ProviderFacilityType", waiverCurrent.SiteID);
            ViewBag.StaffID = new SelectList(db.Teachers, "ID", "TeacherIDNumber", waiverCurrent.StaffID);
            return View(waiverCurrent);
        }

        // GET: WaiverCurrents/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WaiverCurrent waiverCurrent = db.WaiverCurrents.Find(id);
            if (waiverCurrent == null)
            {
                return HttpNotFound();
            }
            ViewBag.SiteID = new SelectList(db.Facilities, "ID", "ProviderFacilityType", waiverCurrent.SiteID);
            ViewBag.StaffID = new SelectList(db.Teachers, "ID", "TeacherIDNumber", waiverCurrent.StaffID);
            return View(waiverCurrent);
        }

        // POST: WaiverCurrents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "WaiverCurrentID,WaiverType,SiteID,SparkCurrent,StaffID,Qualification,Development,Credits,TrainingHours,NineHundredServiceHours,ServiceHourImpact,ServiceHourImpactOther,ServiceHourCount,AdditionalComments,Unsatisfied")] WaiverCurrent waiverCurrent)
        {
            if (ModelState.IsValid)
            {
                db.Entry(waiverCurrent).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SiteID = new SelectList(db.Facilities, "ID", "ProviderFacilityType", waiverCurrent.SiteID);
            ViewBag.StaffID = new SelectList(db.Teachers, "ID", "TeacherIDNumber", waiverCurrent.StaffID);
            return View(waiverCurrent);
        }

        // GET: WaiverCurrents/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WaiverCurrent waiverCurrent = db.WaiverCurrents.Find(id);
            if (waiverCurrent == null)
            {
                return HttpNotFound();
            }
            return View(waiverCurrent);
        }

        // POST: WaiverCurrents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            WaiverCurrent waiverCurrent = db.WaiverCurrents.Find(id);
            db.WaiverCurrents.Remove(waiverCurrent);
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
