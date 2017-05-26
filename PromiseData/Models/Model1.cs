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

        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<ContactAgent> ContactAgents { get; set; }
        public virtual DbSet<Institution> Institutions { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Address>()
                .HasMany(e => e.Institutions)
                .WithOptional(e => e.Address)
                .HasForeignKey(e => e.MailingAddressId);

            modelBuilder.Entity<Address>()
                .HasMany(e => e.Institutions1)
                .WithOptional(e => e.Address1)
                .HasForeignKey(e => e.MailingAddressId);

            modelBuilder.Entity<ContactAgent>()
                .HasMany(e => e.Institutions)
                .WithOptional(e => e.ContactAgent)
                .HasForeignKey(e => e.ContactAgentId);

            modelBuilder.Entity<ContactAgent>()
                .HasMany(e => e.Institutions1)
                .WithOptional(e => e.ContactAgent1)
                .HasForeignKey(e => e.DirectorAgentId);
        }
    }
}
