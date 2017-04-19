using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PromiseData.Models;

namespace PromiseData.Controllers
{
    public class HomeController : Controller
    {
        private Model1 _context;

        public HomeController()
        {
            _context = new Model1();
        }

        public ActionResult Index()
        {
            var students = _context.Children;
            return View( students);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}