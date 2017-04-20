using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PromiseData.Models;

namespace PromiseData.Controllers
{
    public class RoleController : Controller
    {
        private ApplicationDbContext _context;

        public RoleController()
        {
            _context = new ApplicationDbContext();
        }

        // GET: Role
        public ActionResult Index()
        {
            var Roles = _context.Roles.ToList();
            return View();
        }
    }
}