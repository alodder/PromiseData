using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PromiseData.Models;
using PromiseData.ViewModels;

namespace PromiseData.Controllers
{
    public class AddressController : Controller
    {
        private ApplicationDbContext _context;

        public AddressController()
        {
            _context = new ApplicationDbContext();
        }

        [Authorize]
        [HttpGet]
        public ActionResult Create()
        {
            var viewModel = new AddressViewModel
            {
                AddressTypes = _context.Code_AddressType.ToList(),
                States = _context.LU_State.ToList()
            };
            return View(viewModel);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AddressViewModel viewModel)
        {

            if (!ModelState.IsValid)
            {
                viewModel.AddressTypes = _context.Code_AddressType.ToList();
                viewModel.States = _context.LU_State.ToList();
                return View("Create", viewModel);
            }


            var address = new Address
            {
                AddressType_ID = viewModel.AddressType_ID,
                Address1 = viewModel.Address1,
                Address2 = viewModel.Address2,
                Address3 = viewModel.Address3,
                City = viewModel.City,
                State_ID = viewModel.State_ID,
                ZipCode = viewModel.ZipCode,
                County = viewModel.County
            };

            _context.Addresses.Add(address);
            _context.SaveChanges();

            return RedirectToAction("Index", "Address");
        }

        [Authorize]
        public ActionResult Details(int id)
        {
            var address = _context.Addresses.Single(a => a.ID == id);
            return View( address);
        }

        [Authorize]
        public ActionResult Delete(int id)
        {
            var address = _context.Addresses.Single(a => a.ID == id);
            var viewModel = new AddressViewModel
            {
                AddressType_ID = (int)address.AddressType_ID,
                Address1 = address.Address1,
                Address2 = address.Address2,
                Address3 = address.Address3,
                City = address.City,
                State_ID = address.State_ID,
                ZipCode = address.ZipCode,
                County = address.County,
                AddressTypes = _context.Code_AddressType.ToList(),
                States = _context.LU_State.ToList()
            };
            return View(viewModel);
        }

        [Authorize]
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult ConfirmDelete(int id)
        {
            var address = _context.Addresses.Single(a => a.ID == id);
            _context.Addresses.Remove( address);
            _context.SaveChanges();
            return RedirectToAction("Index", "Address");
        }

        [Authorize]
        // GET: Address
        public ActionResult Index()
        {
            var viewModel = _context.Addresses;
            return View(viewModel);
        }
    }
}