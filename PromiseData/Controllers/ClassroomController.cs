using PromiseData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PromiseData.Controllers
{
    public class ClassroomController : Controller
    {
        private ApplicationDbContext _context;

        public ClassroomController()
        {
            _context = new ApplicationDbContext();
        }

        // GET: Classroom
        public ActionResult Index()
        {
            var students = _context.Children;
            var classroom = _context.Classrooms;
            return View();
        }
    }
}