﻿using System;
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
using System.Net;

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
            UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new IdentityStoreDbContext()));
        }

        [HttpPost]
        [Authorize]
        public ActionResult Search(UsersAdminViewModel viewModel)
        {
            return RedirectToAction("List", "User", new { query = viewModel.SearchTerm });
        }

        [HttpGet]
        [Authorize(Roles = "System Administrator, Administrator")]
        public ActionResult List(string query = null)
        {
            UsersAdminViewModel userListViewModel = new UsersAdminViewModel();

            userListViewModel = UserRoleCheck( userListViewModel);

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

            var identity = User.Identity as ClaimsIdentity;
            IdentityUserClaim claim = new IdentityUserClaim();

            if (user.Claims.Any())
            {
                claim = (from c in user.Claims
                         where c.ClaimType == "Institution"
                         select c).Single();
                int institutionId;
                int.TryParse(claim.ClaimValue, out institutionId);
                userViewModel.Institution = App_context.Institutions.Single(a => a.Id == institutionId);
                userViewModel.InstitutionId = institutionId.ToString();
            }

            if (User.IsInRole("Administrator") || User.IsInRole("System Administrator"))
            {
                userViewModel.CanView = true;
                userViewModel.CanEdit = true;
            }

            userViewModel.User = user;
            userViewModel.UserName = user.UserName;
            userViewModel.UserId = user.Id;
            userViewModel.CurrentRoles = user.Roles;
            userViewModel.Roles = _context.Roles.ToList();

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
            var userAndRole = new UserFormViewModel();
            var user = new ApplicationUser();
            try
            {
                user = UserManager.Users.Single(a => a.Id == id);
                userAndRole.UserName = user.UserName;

                userAndRole.UserId = user.Id;
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
        public ActionResult AssignRole(UserFormViewModel userAndRole)
        {
            var user = UserManager.FindById( userAndRole.UserId); //UserManager.Users.Single(a => a.Id == userAndRole.Id);
            
            /*foreach (IdentityUserRole role in user.Roles)
            {
                UserManager.RemoveFromRoleAsync(userAndRole.Id, role.RoleId);
            }*/
            UserManager.RemoveFromRoles(user.Id, UserManager.GetRoles(user.Id).ToArray());
            //UserManager.UpdateAsync(user);
            UserManager.AddToRoles(userAndRole.UserId, userAndRole.SelectedRoleNames);
            /*foreach (string roleid in userAndRole.SelectedRoleNames)
            {
                UserManager.AddToRole(user.Id, roleid);
            }*/

            UserManager.Update(user);

            return RedirectToAction("List", "User");
        }


        /**
         *  Handle passing null!!
         * combine form for assigning institution(hub or operator) and provider based on user role
         * should first check if user role is assigned, then base form on role
         */
        [Authorize(Roles = "System Administrator, Administrator")]
        [HttpGet]
        public ActionResult AssignInstitution(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context));
            var userAndInstitution = new UserFormViewModel();
            var user = new ApplicationUser();

            user = UserManager.Users.Single(a => a.Id == id);
            if (user == null)
            {
                return HttpNotFound();
            }
            var providerRoleId = RoleManager.Roles.Where(r => r.Name == "Provider").Select(r => r.Id);
            var hubRoleId = RoleManager.Roles.Where(r => r.Name == "Hub").Select(r => r.Id);

            var roles = RoleManager.Roles;
            var identity = User.Identity as ClaimsIdentity;

            IdentityUserClaim claim = new IdentityUserClaim();

            if (user.Claims.Any())
            {
                    
                if (user.Roles.Select(r => r.RoleId).Intersect(roles.Select(r => r.Id)).Contains("Hub"))
                {
                    claim = (from c in user.Claims
                                    where c.ClaimType == "Institution"
                                    select c).Single();
                }
                else if (user.Roles.Select(r => r.RoleId).Intersect(roles.Select(r => r.Id)).Contains("Provider"))
                {
                    claim = (from c in user.Claims
                                where c.ClaimType == "Provider"
                                select c).Single();
                }
            }

            userAndInstitution.User = user;

            //Institution where user claim matches id
            if( claim.ClaimType == "Institution")
            {
                userAndInstitution.InstitutionId = claim.ClaimValue;
            }
            else if (claim.ClaimType == "Provider")
            {
                userAndInstitution.ProviderId = claim.ClaimValue;
            }

            userAndInstitution.UserName = user.UserName;
            userAndInstitution.UserId = user.Id;

            userAndInstitution.CurrentRoles = user.Roles;
            userAndInstitution.RoleNames = new String[user.Roles.Count()];

            int j = 0;
            foreach (IdentityUserRole role in userAndInstitution.CurrentRoles)
            {
                userAndInstitution.RoleNames[j] = roles.Single( r => r.Id == role.RoleId).Name;
                j++;
            }

            userAndInstitution.Institutions = App_context.Institutions.ToList();
            userAndInstitution.Providers = App_context.Facilities.ToList();

            userAndInstitution.ListInstitutionNames = new String[userAndInstitution.Institutions.ToArray().Length];
            userAndInstitution.ListProviderNames = new String[userAndInstitution.Providers.ToArray().Length];

            return View(userAndInstitution);
        }

        //[HttpGet]
        public JsonResult GetOperatorProviders(int id)
        {
            var providers = App_context.Facilities
                .Where(c => c.ProviderID == id)
                .Select(c => new {
                    ID = c.ID,
                    Description = c.Description
                })
                .ToList();
            return Json(providers, JsonRequestBehavior.AllowGet);
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

                var institutionClaims = (from c in identity.Claims
                                         where c.Type == "Institution" || c.Type == "Provider"
                                         select c);

                foreach (Claim claim in institutionClaims)
                {
                    UserManager.RemoveClaim(user.Id, claim);
                }

                UserManager.AddClaim(user.Id, new Claim("Institution", userAndInstitution.InstitutionId));
                if( userAndInstitution.ProviderId != null)
                {
                    UserManager.AddClaim(user.Id, new Claim("Provider", userAndInstitution.ProviderId));
                }

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

        [Authorize]
        [HttpGet]
        public ActionResult Delete(string id)
        {
            var user = UserManager.FindById( id);
            return View( user);
        }

        [Authorize]
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult ConfirmDelete(string id)
        {
            if (ModelState.IsValid)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                var user = UserManager.FindById( id);
                var logins = user.Logins;
                var rolesForUser = UserManager.GetRoles( id);

                using (var transaction = _context.Database.BeginTransaction())
                {
                    foreach (var login in logins.ToList())
                    {
                        UserManager.RemoveLogin(login.UserId, new UserLoginInfo(login.LoginProvider, login.ProviderKey));
                    }

                    if (rolesForUser.Count() > 0)
                    {
                        foreach (var item in rolesForUser.ToList())
                        {
                            // item should be the name of the role
                            var result = UserManager.RemoveFromRole(user.Id, item);
                        }
                    }

                    UserManager.Delete(user);
                    transaction.Commit();
                }

                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }

        // GET: User
        [Authorize]
        public ActionResult Index()
        {
            var viewModel = new UsersAdminViewModel();
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
                    viewModel.CanAdd = true;
                    viewModel.CanDelete = true;
                    viewModel.CanView = true;
                    ViewBag.displayAdminMenu = true;
                    ViewBag.displayHubMenu = true;
                    ViewBag.displayProviderMenu = true;
                }
                if (User.IsInRole("Administrator"))
                {
                    viewModel.CanAdd = true;
                    viewModel.CanDelete = true;
                    viewModel.CanView = true;
                    ViewBag.displayAdminMenu = true;
                    ViewBag.displayHubMenu = true;
                    ViewBag.displayProviderMenu = true;
                }
                if (User.IsInRole("Hub"))
                {
                    viewModel.CanAdd = true;
                    viewModel.CanView = true;
                    ViewBag.displayHubMenu = true;
                    ViewBag.displayProviderMenu = true;
                }
                if (User.IsInRole("Provider"))
                {
                    viewModel.CanView = true;
                    ViewBag.displayProviderMenu = true;
                }

                return View();
            }
            else
            {
                ViewBag.Name = "Not Logged In";
            }
            return View( viewModel);
        }

        private UsersAdminViewModel UserRoleCheck(UsersAdminViewModel viewModel)
        {
            if (User.IsInRole("System Administrator"))
            {
                viewModel.CanAdd = true;
                viewModel.CanDelete = true;
                viewModel.CanView = true;
            }
            if (User.IsInRole("Administrator"))
            {
                viewModel.CanAdd = true;
                viewModel.CanDelete = true;
                viewModel.CanView = true;
            }
            if (User.IsInRole("Hub"))
            {
                viewModel.CanAdd = true;
                viewModel.CanView = true;
            }
            if (User.IsInRole("Provider"))
            {
                viewModel.CanView = true;
            }

            return viewModel;
        }


    }
}