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
        private ApplicationDbContext _context;

        public AddressController()
        {
            _context = new ApplicationDbContext();
        }

        public ActionResult Create()
        {
            
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
            var viewModel = _context.Addresses;
            return View(viewModel);
        }
    }
}