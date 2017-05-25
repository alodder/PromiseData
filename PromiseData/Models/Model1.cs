namespace PromiseData.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=Model1")
        {
        }

        public virtual DbSet<ContactAgent> ContactAgents { get; set; }
        public virtual DbSet<Institution> Institutions { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ContactAgent>()
                .HasMany(e => e.Institutions)
                .WithOptional(e => e.ContactAgent)
                .HasForeignKey(e => e.DirectorAgentId);

            modelBuilder.Entity<ContactAgent>()
                .HasMany(e => e.Institutions1)
                .WithOptional(e => e.ContactAgent1)
                .HasForeignKey(e => e.ContactAgentId);
        }
    }
}
