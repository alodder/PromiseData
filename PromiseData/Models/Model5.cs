namespace PromiseData.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Model5 : DbContext
    {
        public Model5()
            : base("name=Model5")
        {
        }

        public virtual DbSet<Institution> Institutions { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
