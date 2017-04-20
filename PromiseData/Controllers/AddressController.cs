using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PromiseData.Models;

namespace PromiseData.Controllers
{
    public class AddressController : Controller
    {
        private Model1 _context;

        public AddressController()
        {
            _context = new Model1();
        }

        public ActionResult Create()
        {
            var address = new Address();
            return View();
        }

        public ActionResult Details(int addressID)
        {
            var address = _context.Addresses.Where(a => a.ID == addressID);
            return View( address);
        }

        // GET: Address
        public ActionResult Index()
        {
            return View();
        }
    }
}