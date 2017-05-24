namespace PromiseData.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using Microsoft.AspNet.Identity.EntityFramework;

    public partial class IdentityStoreDbContext : IdentityDbContext<ApplicationUser>
    {
        public IdentityStoreDbContext()
            : base("name=IdentityConnection", throwIfV1Schema: false)
        {
        }

        public static IdentityStoreDbContext Create()
        {
            return new IdentityStoreDbContext();
        }
    }
}
