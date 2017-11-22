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

            viewModel.WaiverCurrents = db.WaiverCurrents.Where(w => DateTime.Compare((w.Expiration ?? DateTime.MaxValue), DateTime.Now) > 0)
                .Include(w => w.Site)
                .Include(w => w.Staff);

            return View( viewModel);
        }

        // GET: WaiverCurrents
        public ActionResult Archived()
        {
            WaiversProcessViewModel viewModel = new WaiversProcessViewModel();

            viewModel.WaiverCurrents = db.WaiverCurrents.Where(w => DateTime.Compare((w.Expiration ?? DateTime.MaxValue), DateTime.Now) <= 0)
                .Include(w => w.Site)
                .Include(w => w.Staff);

            return View(viewModel);
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
        public ActionResult Create(int? id)
        {
            WaiverCurrentViewModel viewModel = new WaiverCurrentViewModel();
            viewModel.Sites = db.Facilities;
            viewModel.Staffs = db.Teachers;

            if (id != null)
            {
                WaiverRequest waiverrequest = db.WaiverRequests.Find(id);
                if (waiverrequest != null)
                {
                    viewModel.WaiverRequest = waiverrequest;
                    viewModel.WaiverRequestID = id.GetValueOrDefault();

                    viewModel.WaiverType = waiverrequest.WaiverType;
                    if (String.Compare(waiverrequest.WaiverType, "Site", true) == 0)
                    {

                    }
                    viewModel.SiteID = waiverrequest.SiteID.GetValueOrDefault();
                    viewModel.SparkCurrent = waiverrequest.SparkCurrent;
                    viewModel.ServiceHourCount = waiverrequest.ServiceHours.GetValueOrDefault();
                    viewModel.StaffID = waiverrequest.StaffID.GetValueOrDefault();
                    viewModel.Qualification = waiverrequest.Qualification;
                }
            }
            return View(viewModel);
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
    }
}
