using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PromiseData.Controllers
{
    public class InstitutionController : Controller
    {

        // GET: Institution
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        // GET: Institution
        public ActionResult Index()
        {
            return View();
        }
    }
}