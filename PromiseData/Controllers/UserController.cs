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

        [HttpPost]
        [Authorize]
        public ActionResult Search(UsersAdminViewModel viewModel)
        {
            return RedirectToAction("List", "User", new { query = viewModel.SearchTerm });
        }

        [Authorize(Roles = "System Administrator, Administrator")]
        public ActionResult List(string query = null)
        {

            UsersAdminViewModel userListViewModel = new UsersAdminViewModel();
            userListViewModel.Users = UserManager.Users.ToList();
            if (!String.IsNullOrWhiteSpace(query))
            {
                var blurb = userListViewModel.Users.Where(i =>
                                            i.UserName.Contains(query) ||
                                            (i.Name ?? "").Contains(query) ||
                                            (i.PhoneNumber ?? "").Contains(query) ||
                                            (i.Email ?? "").Contains(query));

                userListViewModel.Users = blurb.ToList();
            }

            return View( userListViewModel);
        }

        [HttpGet]
        [Authorize(Roles = "System Administrator, Administrator")]
        public ActionResult Details(String id)
        {
            var userViewModel = new UserFormViewModel();
            var user = UserManager.Users.Single(a => a.Id == id);
            userViewModel.User = user;

            return View( userViewModel);
        }

        [HttpGet]
        [Authorize(Roles = "System Administrator, Administrator")]
        public ActionResult Edit(String id)
        {
            var userViewModel = new UserFormViewModel();
            var user = UserManager.Users.Single(a => a.Id == id);
            userViewModel.User = user;

            return View(userViewModel);
        }
        
        [HttpPost]
        [Authorize(Roles = "System Administrator, Administrator")]
        public ActionResult Edit(UserFormViewModel userViewModel)
        {
            //var user = UserManager.Users.Single(a => a.Id == id);
            //userViewModel.User = user;

            var user = UserManager.FindById( userViewModel.User.Id);

            user.Name = userViewModel.User.Name;
            user.UserName = userViewModel.User.UserName;
            user.PhoneNumber = userViewModel.User.PhoneNumber;
            user.LockoutEnabled = userViewModel.User.LockoutEnabled;

            UserManager.Update( user);

            return View( userViewModel);
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
                if(User.IsInRole("System Administrator"))
                {
                    //System Administrator can assign users to all roles including System Administrator
                    userAndRole.Roles = _context.Roles.ToList();//.Where(u => !u.Name.Contains("System"));
                }
                else
                {
                    //Administrator can assign non-admin roles: View, Provider, Hub
                    userAndRole.Roles = _context.Roles.ToList().Where(u => !u.Name.Contains("Admin"));
                }

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
            var userAndInstitution = new UserFormViewModel();
            var user = new ApplicationUser();
            try
            {
                user = UserManager.Users.Single(a => a.Id == id);
                var identity = User.Identity as ClaimsIdentity;
                IdentityUserClaim claim = new IdentityUserClaim();

                if (user.Claims.Any())
                {
                    claim = (from c in user.Claims
                                 where c.ClaimType == "Institution"
                                 select c).Single();
                }

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
        public ActionResult AssignInstitution(UserFormViewModel userAndInstitution)
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
            if (User.IsInRole("System Administrator"))
            {
                //System Administrator can assign users to all roles including System Administrator
                ViewBag.Roles = new SelectList(_context.Roles.ToList(), "Name", "Name");//.Where(u => !u.Name.Contains("System"));
            }
            else
            {
                //Administrator can assign non-admin roles: View, Provider, Hub
                ViewBag.Roles = new SelectList(_context.Roles.Where(u => !u.Name.Contains("Admin")).ToList(), "Name", "Name");
            }
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [HttpGet]
        [AllowAnonymous]
        public ActionResult ChangePassword(String id)
        {
            var changePasswordModel = new ResetPasswordViewModel();
            var user = UserManager.FindById( id);
            //var user = UserManager.Users.Single(a => a.Id == id);
            changePasswordModel.Email = user.Email;
            return View(changePasswordModel);
        }

        //
        // POST: /Account/ResetPassword
        //Should be async?
        [HttpPost]
        [AllowAnonymous]
        public ActionResult ChangePassword(ResetPasswordViewModel changePasswordModel)
        {
            if (!ModelState.IsValid)
            {
                return View(changePasswordModel);
            }
            //var user = UserManager.FindById( userId);
            var user = UserManager.Users.Single(a => a.Email == changePasswordModel.Email);
            UserManager.RemovePassword(user.Id);
            var result = UserManager.AddPassword( user.Id, changePasswordModel.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("List", "User");
            }
            return View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [Authorize(Roles = "System Administrator, Administrator")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            //AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
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