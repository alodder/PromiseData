using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PromiseData.Models;

namespace PromiseData.Controllers
{
    public class UserController : Controller
    {
        private ApplicationDbContext _context;

        public UserController()
        {
            _context = new ApplicationDbContext();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult List()
        {
            //_context = new ApplicationDbContext();
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context));
            var userList = UserManager.Users.ToList();
            
            return View(userList);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Edit(String id)
        {
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context));
            var user = UserManager.Users.Single(a => a.Id == id);

            return View(user);
        }

        // GET: User
        [Authorize]
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = User.Identity;
                ViewBag.Name = user.Name;

                //ViewBag.displayMenu = false;
                ViewBag.displayAdminMenu = false;
                ViewBag.displayHubMenu = false;
                ViewBag.displayProviderMenu = false;

                if (User.IsInRole("Admin"))
                {
                    ViewBag.displayAdminMenu = true;
                    ViewBag.displayHubMenu = true;
                    ViewBag.displayProviderMenu = true;
                }
                if (User.IsInRole("Hub"))
                {
                    ViewBag.displayHubMenu = true;
                    ViewBag.displayProviderMenu = true;
                }
                if (User.IsInRole("Provider"))
                {
                    ViewBag.displayProviderMenu = true;
                }

                return View();
            }
            else
            {
                ViewBag.Name = "Not Logged In";
            }
            return View();
        }

        
    }
}