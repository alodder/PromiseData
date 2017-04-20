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
        private Model1 _context;

        public ClassroomController()
        {
            _context = new Model1();
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