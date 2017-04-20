using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PromiseData.Models;

namespace PromiseData.Controllers
{
    public class RoleManagerController : Controller
    {
        private ApplicationDbContext _context;

        public RoleManagerController()
        {
            _context = new ApplicationDbContext();
        }

        // GET: RoleManager
        public ActionResult Index()
        {
            
            return View();
        }

        // GET: RoleManager/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: RoleManager/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RoleManager/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: RoleManager/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: RoleManager/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: RoleManager/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: RoleManager/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
