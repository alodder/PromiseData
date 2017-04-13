using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace PromiseData.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        //public virtual DbSet<Code_Gender> CodeGender { get; set; }
        //public virtual DbSet<Child> Children { get; set; }

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}