﻿using System;
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
        public ActionResult Create()
        {
            ViewBag.WaiverType = new SelectList(new List<SelectListItem>
                                {
                                    new SelectListItem { Selected = true, Text = "Site", Value = "Site"},
                                    new SelectListItem { Selected = false, Text = "Staff", Value = "Staff"}
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
        public ActionResult Create([Bind(Include = "WaiverRequestID,waiverType,SiteID,SparkCurrent,StaffID,Qualification,AdditionalComments")] WaiverRequest waiverRequest)
        {
            if (ModelState.IsValid)
            {
                db.WaiverRequests.Add(waiverRequest);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.SiteID = new SelectList(db.Facilities, "ID", "Description", waiverRequest.SiteID);
            ViewBag.StaffID = new SelectList(db.Teachers, "ID", "NameLast", waiverRequest.StaffID);
            return View(waiverRequest);
        }

        // GET: WaiverRequests/Edit/5
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
            ViewBag.SiteID = new SelectList(db.Facilities, "ID", "Description", waiverRequest.SiteID);
            ViewBag.StaffID = new SelectList(db.Teachers, "ID", "NameLast", waiverRequest.StaffID);
            return View(waiverRequest);
        }

        // POST: WaiverRequests/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "WaiverRequestID,waiverType,SiteID,SparkCurrent,StaffID,Qualification,AdditionalComments")] WaiverRequest waiverRequest)
        {
            if (ModelState.IsValid)
            {
                db.Entry(waiverRequest).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SiteID = new SelectList(db.Facilities, "ID", "Description", waiverRequest.SiteID);
            ViewBag.StaffID = new SelectList(db.Teachers, "ID", "NameLast", waiverRequest.StaffID);
            return View(waiverRequest);
        }

        // GET: WaiverRequests/Delete/5
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
    }
}
