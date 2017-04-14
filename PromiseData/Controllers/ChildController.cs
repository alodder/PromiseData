using System;
using System.Linq;
using System.Web.Mvc;
using PromiseData.Models;
using PromiseData.ViewModels;

namespace PromiseData.Controllers
{
    public class ChildController : Controller
    {
        private Model1 _context; 

        public ChildController()
        {
            _context = new Model1();
        }

        [Authorize]
        public ActionResult Add()
        {
            var viewModel = new ChildFormViewModel
            {
                Genders = _context.CodeGender.ToList()
            };
            return View(viewModel);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Add(ChildFormViewModel viewmodel)
        {
            var child = new Child
            {
                LastName = viewmodel.LastName,
                OtherLastName = viewmodel.OtherLastName,
                FirstName = viewmodel.FirstName,
                MiddleName = viewmodel.MiddleName,
                OtherMiddleName = viewmodel.OtherMiddleName,
                Birthdate = (DateTime?)viewmodel.Birthdate,
                Gender_ID = viewmodel.GenderID
            };

            _context.Children.Add(child);
            _context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }
    }
}