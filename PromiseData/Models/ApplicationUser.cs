using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Web;
using PromiseData.Repositories;
using System.Web.Security;

namespace PromiseData.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [StringLength(88)]
        public string Name { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

    }

    /**
     * Action filter ton add User institution information into viewbag/viewdata
     */
    public class UserClaimsActionFilter : ActionFilterAttribute
    {
        
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            ApplicationDbContext _context = new ApplicationDbContext();
            UserRepository _userRepository = new UserRepository( _context);
            InstitutionRepository _institutionRepository = new InstitutionRepository(_context);
            var User = HttpContext.Current.User;

            filterContext.Controller.ViewBag.UserIsAuthenticated = User.Identity.IsAuthenticated;
            if( User.Identity.IsAuthenticated)
            {
                filterContext.Controller.ViewBag.UserRoles = _userRepository.GetUserRoles( (ClaimsPrincipal)User);
                filterContext.Controller.ViewBag.UserIsAdmin = ((User.IsInRole("System Administrator")
                    || User.IsInRole("Administrator")));

                if(!filterContext.Controller.ViewBag.UserIsAdmin)
                {
                    var InstitutionId = _userRepository.GetUserInstitutionID( (ClaimsPrincipal)User);
                    filterContext.Controller.ViewBag.UserInstitutionId = InstitutionId;
                    filterContext.Controller.ViewBag.UserInstitutionName = _institutionRepository.GetInstitutionName( InstitutionId);
                    filterContext.Controller.ViewBag.UserInstitutionIsHub = _institutionRepository.IsHub( InstitutionId);
                    filterContext.Controller.ViewBag.UserInstitutionisProvider = _institutionRepository.IsProvider( InstitutionId);
                }               
            }
            
        }

    }
}