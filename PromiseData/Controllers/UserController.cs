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
using System.Security.Claims;

namespace PromiseData.Controllers
{
    public class UserController : Controller
    {
        private ApplicationDbContext App_context;

        private IdentityStoreDbContext _context;
        private UserManager<ApplicationUser> UserManager;
        private RoleManager<IdentityRole> RoleManager;

        public UserController()
        {
            App_context = new ApplicationDbContext();

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
            var user = UserManager.FindById( userAndRole.Id); //UserManager.Users.Single(a => a.Id == userAndRole.Id);
            
            /*foreach (IdentityUserRole role in user.Roles)
            {
                UserManager.RemoveFromRoleAsync(userAndRole.Id, role.RoleId);
            }*/
            UserManager.RemoveFromRoles(user.Id, UserManager.GetRoles(user.Id).ToArray());
            //UserManager.UpdateAsync(user);
            UserManager.AddToRoles(userAndRole.Id, userAndRole.SelectedRoleNames);
            /*foreach (string roleid in userAndRole.SelectedRoleNames)
            {
                UserManager.AddToRole(user.Id, roleid);
            }*/

            UserManager.Update(user);

            return RedirectToAction("List", "User");
        }

        [Authorize(Roles = "System Administrator, Administrator")]
        [HttpGet]
        public ActionResult AssignInstitution(string id)
        {
            //var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context));
            var userAndInstitution = new UserInstitutionViewModel();
            var user = new ApplicationUser();
            try
            {
                user = UserManager.Users.Single(a => a.Id == id);
                var identity = User.Identity as ClaimsIdentity;

                var claim = (from c in user.Claims
                             where c.ClaimType == "Institution"
                             select c).Single();

                userAndInstitution.User = user;

                //Institution where user claim matches id
                userAndInstitution.InstitutionId = claim.ClaimValue;

                userAndInstitution.UserName = user.UserName;
                userAndInstitution.UserId = user.Id;
                userAndInstitution.Institutions = App_context.Institutions.ToList();

                userAndInstitution.ListInstitutionNames = new String[userAndInstitution.Institutions.ToArray().Length];

                int i = 0;
                foreach (Institution institution in userAndInstitution.Institutions)
                {
                    userAndInstitution.ListInstitutionNames[i] = institution.LegalName;
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

        /**
         * remove Institution/ then add selected institution
         */
        [Authorize(Roles = "System Administrator, Administrator")]
        [HttpPost]
        public ActionResult AssignInstitution(UserInstitutionViewModel userAndInstitution)
        {
            if (userAndInstitution != null)
            {
                var user = UserManager.FindById(userAndInstitution.UserId); //UserManager.Users.Single(a => a.Id == userAndRole.Id);
                ClaimsIdentity identity = UserManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);

                var claims = (from c in identity.Claims
                             where c.Type == "Institution"
                             select c);

                foreach (Claim claim in claims)
                {
                    UserManager.RemoveClaim(user.Id, claim);
                }

                UserManager.AddClaim(user.Id, new Claim("Institution", userAndInstitution.InstitutionId));


                UserManager.Update(user);
                return RedirectToAction("List", "User");
            }

            // If we got this far, something failed, redisplay form
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
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email, Name = model.Name };
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