using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using PromiseData.Models;

[assembly: OwinStartupAttribute(typeof(PromiseData.Startup))]
namespace PromiseData
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            createRolesandUsers();
        }

        private void createRolesandUsers()
        {
            IdentityStoreDbContext context = new IdentityStoreDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));


            // In Startup iam creating first Admin Role and creating a default Admin User    
            if (!roleManager.RoleExists("System Administrator"))
            {

                // first we create Admin rool   
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "System Administrator";
                roleManager.Create(role);

                //Here we create a Admin super user who will maintain the website                  

                var user = new ApplicationUser();
                user.UserName = "systemadmin@email.com";
                user.Email = "systemadmin@email.com";
                user.Name = "System Administrator";

                string userPWD = "P@ssw0rd";

                var chkUser = UserManager.Create(user, userPWD);

                //Add default User to Role Admin   
                if (chkUser.Succeeded)
                {
                    var result1 = UserManager.AddToRole(user.Id, "System Administrator");

                }
            }

            // creating Creating Administrator role    
            if (!roleManager.RoleExists("Administrator"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Administrator";
                roleManager.Create(role);

            }

            // creating Creating Hub role    
            if (!roleManager.RoleExists("Hub"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Hub";
                roleManager.Create(role);

            }

            // creating Creating Hub role    
            if (!roleManager.RoleExists("Provider"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Provider";
                roleManager.Create(role);
            }

            // creating Creating View role    
            if (!roleManager.RoleExists("View"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "View";
                roleManager.Create(role);
            }
        }
    }
}
