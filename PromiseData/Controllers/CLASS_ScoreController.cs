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
    public class CLASS_ScoreController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: CLASS_Score
        public ActionResult Index()
        {
            var classScores = db.ClassScores.Include(c => c.Classroom);
            return View(classScores.ToList());
        }

        // GET: CLASS_Score/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CLASS_Score cLASS_Score = db.ClassScores.Find(id);
            if (cLASS_Score == null)
            {
                return HttpNotFound();
            }
            return View(cLASS_Score);
        }

        // GET: CLASS_Score/Create
        public ActionResult Create(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Classroom classroom = db.Classrooms.Find(id);
            if (classroom == null)
            {
                return HttpNotFound();
            }
            ViewBag.Classroom_id = new SelectList(db.Classrooms, "ID", "Description", classroom.ID);
            return View();
        }

        // POST: CLASS_Score/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Score_id,Classroom_id,CLASSScore_EmotionalSupport,CLASSScore_ClassroomOrganization,CLASSScore_InstructionalSupport,Score_date")] CLASS_Score cLASS_Score)
        {
            if (ModelState.IsValid)
            {
                db.ClassScores.Add(cLASS_Score);
                db.SaveChanges();
                return RedirectToAction("Details", "Classroom", new { id = cLASS_Score.Classroom_id });
            }

            ViewBag.Classroom_id = new SelectList(db.Classrooms, "ID", "Description", cLASS_Score.Classroom_id);
            return View(cLASS_Score);
        }

        // GET: CLASS_Score/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CLASS_Score cLASS_Score = db.ClassScores.Find(id);
            if (cLASS_Score == null)
            {
                return HttpNotFound();
            }
            ViewBag.Classroom_id = new SelectList(db.Classrooms, "ID", "Description", cLASS_Score.Classroom_id);
            return View(cLASS_Score);
        }

        // POST: CLASS_Score/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Score_id,Classroom_id,CLASSScore_EmotionalSupport,CLASSScore_ClassroomOrganization,CLASSScore_InstructionalSupport,Score_date")] CLASS_Score cLASS_Score)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cLASS_Score).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Classroom_id = new SelectList(db.Classrooms, "ID", "Description", cLASS_Score.Classroom_id);
            return View(cLASS_Score);
        }

        // GET: CLASS_Score/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CLASS_Score cLASS_Score = db.ClassScores.Find(id);
            if (cLASS_Score == null)
            {
                return HttpNotFound();
            }
            return View(cLASS_Score);
        }

        // POST: CLASS_Score/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CLASS_Score cLASS_Score = db.ClassScores.Find(id);
            db.ClassScores.Remove(cLASS_Score);
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
