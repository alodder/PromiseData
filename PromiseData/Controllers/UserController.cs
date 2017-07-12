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
using System.Threading.Tasks;

namespace PromiseData.Controllers
{
    public class UserController : Controller
    {
        private IdentityStoreDbContext _context;
        private UserManager<ApplicationUser> UserManager;
        private RoleManager<IdentityRole> RoleManager;

        public UserController()
        {
            _context = new IdentityStoreDbContext();
            RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new IdentityStoreDbContext()));
            UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context));
        }

        [Authorize(Roles = "System Administrator, Administrator")]
        public ActionResult List()
        {
            //_context = new ApplicationDbContext();
            //var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context));
            var userList = UserManager.Users.ToList();
            
            return View(userList);
        }

        [Authorize(Roles = "System Administrator, Administrator")]
        public ActionResult Edit(String id)
        {
            //var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context));
            var user = UserManager.Users.Single(a => a.Id == id);

            return View(user);
        }

        [Authorize(Roles = "System Administrator, Administrator")]
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

                userAndRole.Id = user.Id;
                userAndRole.Roles = _context.Roles.ToList().Where(u => !u.Name.Contains("Admin"));//admin can assign other admin?

                userAndRole.RoleNames = new String[user.Roles.ToArray().Length];

                int i = 0;
                foreach (IdentityUserRole role in user.Roles)
                {
                    userAndRole.RoleNames[i] = _context.Roles.Single(a => a.Id == role.RoleId).Name;
                    i++;
                }
            } catch (Exception e)
            {
                ViewBag.Error = true;
                ViewBag.ErrorMessage = e.ToString();
            }
            return View(userAndRole);
        }

        /**
         * remove all roles/ then add selected roles
         */
        [Authorize(Roles = "System Administrator, Administrator")]
        [HttpPost]
        public ActionResult AssignRole(UserRoleViewModel userAndRole)
        {
            var user = UserManager.Users.Single(a => a.Id == userAndRole.Id);
            foreach (IdentityUserRole role in user.Roles)
            {
                UserManager.RemoveFromRoleAsync(userAndRole.Id, role.RoleId);
            }
            foreach (string roleid in userAndRole.SelectedRoleNames)
            {
                UserManager.AddToRoleAsync(userAndRole.Id, roleid);
            }

            return RedirectToAction("List", "User");
        }

        [Authorize(Roles = "System Administrator, Administrator")]
        [HttpGet]
        public ActionResult AssignInstitution(string id)
        {
            //var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context));
            var userAndInstitution = new InstitutionUser();
            var user = new ApplicationUser();
            try
            {
                user = UserManager.Users.Single(a => a.Id == id);

                userAndInstitution.UserID = user.Id;
                userAndInstitution.InstitutionID = _context.Roles.ToList().Where(u => !u.Name.Contains("Admin"));//admin can assign other admin?

                userAndRole.RoleNames = new String[user.Roles.ToArray().Length];

                int i = 0;
                foreach (IdentityUserRole role in user.Roles)
                {
                    userAndRole.RoleNames[i] = _context.Roles.Single(a => a.Id == role.RoleId).Name;
                    i++;
                }
            }
            catch (Exception e)
            {
                ViewBag.Error = true;
                ViewBag.ErrorMessage = e.ToString();
            }
            return View(userAndInstitution);
        }

        //
        // GET: /User/CreateUser
        [Authorize(Roles = "System Administrator, Administrator")]
        public ActionResult CreateUser()
        {
            ViewBag.Name = new SelectList(_context.Roles.Where(u => !u.Name.Contains("Admin")).ToList(), "Name", "Name");
            return View();
        }

        //
        // POST: /User/CreateUser
        [HttpPost]
        [Authorize(Roles = "System Administrator, Administrator")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateUser(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    //Assign Role to user Here      
                    await this.UserManager.AddToRoleAsync(user.Id, model.UserRoles);

                    //await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    return RedirectToAction("List", "User");
                }
                //AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
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

                if (User.IsInRole("System Administrator"))
                {
                    ViewBag.displayAdminMenu = true;
                    ViewBag.displayHubMenu = true;
                    ViewBag.displayProviderMenu = true;
                }
                if (User.IsInRole("Administrator"))
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