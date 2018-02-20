using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PromiseData.Models;
using PromiseData.ViewModels;
using System.Net;

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
            return View("AddressForm", viewModel);
        }

        [Authorize]
        [HttpGet]
        public ActionResult CreateForChild(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var viewModel = new AddressViewModel
            {
                AddressTypes = _context.Code_AddressType.ToList(),
                States = _context.LU_State.ToList(),
                ChildID = id
            };
            return View("AddressForm", viewModel);
        }

        [Authorize]
        [HttpGet]
        public ActionResult CreateForProvider(int id)
        {
            var viewModel = new AddressViewModel
            {
                AddressTypes = _context.Code_AddressType.ToList(),
                States = _context.LU_State.ToList(),
                ProviderID = id
            };
            return View("AddressForm", viewModel);
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
                return View("AddressForm", viewModel);
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

            if (viewModel.ProviderID != null)
            {
                var facility = _context.Facilities.Single(a => a.ID == viewModel.ProviderID);
                facility.AddressID = address.ID;
                _context.SaveChanges();
                return RedirectToAction("Details", "Facility", new { id = viewModel.ProviderID });
            }

            if (viewModel.ChildID != null)
            {
                var child = _context.Children.Find( viewModel.ChildID);
                child.Address_ID = address.ID;
                _context.SaveChanges();
                return RedirectToAction("Details", "Child", new { id = viewModel.ChildID });
            }

            return RedirectToAction("Index", "Address");
        }

        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var address = _context.Addresses.Single(a => a.ID == id);
            if (address == null)
            {
                return HttpNotFound();
            }
            return View( address);
        }


        /**
         * Take id, retreive address, build and pass viewmodel
         * */
        [Authorize]
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var address = _context.Addresses.Single(a => a.ID == id);
            var viewModel = new AddressViewModel
            {
                AddressTypes = _context.Code_AddressType.ToList(),
                States = _context.LU_State.ToList(),
                ID = address.ID,
                AddressType_ID = address.AddressType_ID.GetValueOrDefault(),
                Address1 = address.Address1,
                Address2 = address.Address2,
                Address3 = address.Address3,
                City = address.City,
                ZipCode = address.ZipCode,
                County = address.County,
                State_ID = address.State_ID,
                LU_State = address.LU_State
            };
            return View("AddressForm", viewModel);
        }

        /**
         * Take viewmodel, retreive address, update and SaveChanges
         * */
        [Authorize]
        [HttpPost]
        public ActionResult Update(AddressViewModel viewModel)
        {
            var address = _context.Addresses.Single(a => a.ID == viewModel.ID);

            address.AddressType_ID = viewModel.AddressType_ID;
            address.Address1 = viewModel.Address1;
            address.Address2 = viewModel.Address2;
            address.Address3 = viewModel.Address3;
            address.City = viewModel.City;
            address.ZipCode = viewModel.ZipCode;
            address.County = viewModel.County;
            address.State_ID = viewModel.State_ID;
            address.LU_State = viewModel.LU_State;

            _context.SaveChanges();

            var provider = _context.Facilities.FirstOrDefault(p => p.AddressID == address.ID);
            if (provider != null)
            {
                return RedirectToAction("Details", "Facility", new { id = provider.ID });
            }

            return RedirectToAction("Index", "Address");
        }

        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var address = _context.Addresses.Single(a => a.ID == id);
            if (address == null)
            {
                return HttpNotFound();
            }
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
            var viewModel = _context.Addresses.ToList();
            return View(viewModel);
        }
    }
}