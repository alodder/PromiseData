using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace PromiseData.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        //public virtual DbSet<Code_Gender> CodeGender { get; set; }
        //public virtual DbSet<Child> Children { get; set; }

        public ApplicationDbContext()
            : base("DefaultConnection")
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public virtual DbSet<Code_Gender> CodeGender { get; set; }
        public virtual DbSet<RaceEthnicity> RaceEthnic { get; set; }
        public virtual DbSet<Code_Language> CodeLanguage { get; set; }
        public virtual DbSet<Child> Children { get; set; }

        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<Adult> Adults { get; set; }
        public virtual DbSet<Classroom> Classrooms { get; set; }
        public virtual DbSet<Code_AdditionalSupportTypes> Code_AdditionalSupportTypes { get; set; }
        public virtual DbSet<Code_AddressType> Code_AddressType { get; set; }
        public virtual DbSet<Code_Education> Code_Education { get; set; }
        public virtual DbSet<Code_ExitReason> Code_ExitReason { get; set; }
        public virtual DbSet<Code_GenerationCode> Code_GenerationCode { get; set; }
        public virtual DbSet<Code_OtherNameType> Code_OtherNameType { get; set; }
        public virtual DbSet<Code_ProgramSessionType> Code_ProgramSessionType { get; set; }
        public virtual DbSet<Code_ProgramType> Code_ProgramType { get; set; }
        public virtual DbSet<ELD_ID> ELD_ID { get; set; }
        public virtual DbSet<Facility> Facilities { get; set; }
        public virtual DbSet<Family> Families { get; set; }
        public virtual DbSet<LU_State> LU_State { get; set; }
        public virtual DbSet<Screening> Screenings { get; set; }
        public virtual DbSet<Service> Services { get; set; }
        public virtual DbSet<AdultRace> AdultRaces { get; set; }
        public virtual DbSet<Child_Services_Enrollment> Child_Services_Enrollment { get; set; }
        public virtual DbSet<ChildRace> ChildRaces { get; set; }
        public virtual DbSet<ChildScreening> ChildScreenings { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Adult>()
                .HasMany(e => e.AdultRaces)
                .WithRequired(e => e.Adult)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Classroom>()
                .Property(e => e.upsize_ts)
                .IsFixedLength();

            modelBuilder.Entity<Code_AdditionalSupportTypes>()
                .HasMany(e => e.Facilities)
                .WithOptional(e => e.Code_AdditionalSupportTypes)
                .HasForeignKey(e => e.AdditionalChildFamilySupports_ID);

            modelBuilder.Entity<Code_AddressType>()
                .HasMany(e => e.Addresses)
                .WithOptional(e => e.Code_AddressType)
                .HasForeignKey(e => e.AddressType_ID);

            modelBuilder.Entity<Code_Education>()
                .HasMany(e => e.Adults)
                .WithOptional(e => e.Code_Education)
                .HasForeignKey(e => e.Education_ID);

            modelBuilder.Entity<Code_ProgramSessionType>()
                .HasMany(e => e.Classrooms)
                .WithOptional(e => e.Code_ProgramSessionType)
                .HasForeignKey(e => e.ProgramSessionType_ID);

            modelBuilder.Entity<ELD_ID>()
                .HasMany(e => e.Adults)
                .WithOptional(e => e.ELD_ID1)
                .HasForeignKey(e => e.ELD_ID);

            modelBuilder.Entity<Facility>()
                .HasMany(e => e.Classrooms)
                .WithOptional(e => e.Facility)
                .HasForeignKey(e => e.Facility_ID);

            modelBuilder.Entity<Family>()
                .Property(e => e.Income)
                .HasPrecision(19, 4);

            modelBuilder.Entity<LU_State>()
                .HasMany(e => e.Addresses)
                .WithOptional(e => e.LU_State)
                .HasForeignKey(e => e.State_ID);

            modelBuilder.Entity<Screening>()
                .HasMany(e => e.ChildScreenings)
                .WithRequired(e => e.Screening)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Service>()
                .HasMany(e => e.Classrooms)
                .WithOptional(e => e.Service)
                .HasForeignKey(e => e.Program_ID);

            modelBuilder.Entity<Service>()
                .HasMany(e => e.Child_Services_Enrollment)
                .WithRequired(e => e.Service)
                .HasForeignKey(e => e.ServicesID)
                .WillCascadeOnDelete(false);

            base.OnModelCreating(modelBuilder);
        }

        public System.Data.Entity.DbSet<PromiseData.ViewModels.AdultFormViewModel> AdultFormViewModels { get; set; }
    }
}