using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PromiseData.Models;
using PromiseData.ViewModels;

namespace PromiseData.Controllers
{
    public class UserController : Controller
    {
        private ApplicationDbContext _context;
        private UserManager<ApplicationUser> UserManager;
        private RoleManager<IdentityRole> RoleManager;

        public UserController()
        {
            _context = new ApplicationDbContext();
            RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
            UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context));
        }

        [Authorize(Roles = "Admin")]
        public ActionResult List()
        {
            //_context = new ApplicationDbContext();
            //var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context));
            var userList = UserManager.Users.ToList();
            
            return View(userList);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Edit(String id)
        {
            //var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context));
            var user = UserManager.Users.Single(a => a.Id == id);

            return View(user);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult AssignRole(string id)
        {
            //var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context));
            var userAndRole = new UserRoleViewModel();
            var user = new ApplicationUser();
            try
            {
                user = UserManager.Users.Single(a => a.Id == id);
                userAndRole.UserName = user.UserName;
                //new SelectList(Model.CountryList, "Value", "Text", Model.CountryList.SelectedValue)
                userAndRole.CurrentRoles = user.Roles;
                userAndRole.Id = user.Id;
                userAndRole.Roles = _context.Roles.ToList().Where(u => !u.Name.Contains("Admin"));//admin can assign other admin?
            } catch (Exception e)
            {
                ViewBag.Error = true;
                ViewBag.ErrorMessage = e.ToString();
            }
            return View(userAndRole);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult AssignRole(UserRoleViewModel userAndRole)
        {
            foreach (var role in userAndRole.CurrentRoles)
            {
                UserManager.AddToRole(userAndRole.Id, role.RoleId);
            }

            return RedirectToAction("List", "User");
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