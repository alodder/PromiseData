using System.Data.Entity;
//using Microsoft.AspNet.Identity.EntityFramework;

namespace PromiseData.Models
{
    public class ApplicationDbContext : DbContext
    {

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
        public virtual DbSet<Facility> Facilities { get; set; }
        public virtual DbSet<FacilitySupport> FacilitySupports { get; set; }
        public virtual DbSet<Family> Families { get; set; }
        public virtual DbSet<LU_State> LU_State { get; set; }
        public virtual DbSet<Screening> Screenings { get; set; }

        public virtual DbSet<AdultRace> AdultRaces { get; set; }
        public virtual DbSet<Child_Classroom_Enrollment> Child_Classroom_Enrollments { get; set; }
        public virtual DbSet<ChildRace> ChildRaces { get; set; }
        public virtual DbSet<ChildScreening> ChildScreenings { get; set; }
        public virtual DbSet<Teacher> Teachers { get; set; }
        public virtual DbSet<TeacherClass> TeacherClasses { get; set; }

        public virtual DbSet<Child_Facility> ChildFacilities { get; set; }

        public virtual DbSet<Special_Needs> SpecialNeeds { get; set; }
        public virtual DbSet<Child_Special_Needs> Child_Special_Needs { get; set; }

        public virtual DbSet<Code_IFSP> Code_IFSPs { get; set; }
        public virtual DbSet<Child_IFSP> Child_IFSPs { get; set; }

        public virtual DbSet<TeacherLanguageClassroom> TeacherLanguageClassrooms { get; set; }
        public virtual DbSet<TeacherLanguageFluency> TeacherLanguageFluencies { get; set; }

        public virtual DbSet<ContactAgent> ContactAgents { get; set; }
        public virtual DbSet<Institution> Institutions { get; set; }

        public virtual DbSet<WaiverRequest> WaiverRequests { get; set; }
        public virtual DbSet<WaiverCurrent> WaiverCurrents { get; set; }

        public virtual DbSet<Curricula> Curricula { get; set; }
        public virtual DbSet<AssessmentTools> AssessmentTools { get; set; }
        public virtual DbSet<ScreeningTools> ScreeningTools { get; set; }

        public virtual DbSet<ClassroomCurricula> ClassroomCurricula { get; set; }
        public virtual DbSet<ClassroomAssessment> ClassroomAssessments { get; set; }
        public virtual DbSet<ClassroomScreening> ClassroomScreenings { get; set; }

        public virtual DbSet<CLASS_Score> ClassScores { get; set; }

        public virtual DbSet<InstitutionUser> InstitutionUsers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Adult>()
                .HasMany(e => e.AdultRaces)
                .WithRequired(e => e.Adult)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Classroom>()
                .Property(e => e.upsize_ts)
                .IsFixedLength();

            /*modelBuilder.Entity<Classroom>()
                .HasMany(e => e.Teachers)
                .WithMany(e => e.Classrooms)
                .Map(m => m.ToTable("TeacherClass").MapLeftKey("ClassID").MapRightKey("TeacherID"));*/

            modelBuilder.Entity<ClassroomCurricula>()
                .HasKey(t => new { t.CurriculaCode, t.ClassroomID });

            modelBuilder.Entity<Curricula>()
                .HasMany(c => c.ClassroomCurricula)
                .WithRequired()
                .HasForeignKey(c => c.CurriculaCode);

            modelBuilder.Entity<Classroom>()
                .HasMany(c => c.ClassroomCurricula)
                .WithRequired()
                .HasForeignKey(c => c.ClassroomID);

            modelBuilder.Entity<ClassroomAssessment>()
                .HasKey(t => new { t.AssessmentCode, t.ClassroomID });

            modelBuilder.Entity<AssessmentTools>()
                .HasMany(c => c.ClassroomAssessments)
                .WithRequired()
                .HasForeignKey(c => c.AssessmentCode);

            modelBuilder.Entity<Classroom>()
                .HasMany(c => c.ClassroomAssessments)
                .WithRequired()
                .HasForeignKey(c => c.ClassroomID);

            modelBuilder.Entity<ClassroomScreening>()
                .HasKey(t => new { t.ScreeningCode, t.ClassroomID });

            modelBuilder.Entity<ScreeningTools>()
                .HasMany(c => c.ClassroomScreenings)
                .WithRequired()
                .HasForeignKey(c => c.ScreeningCode);

            modelBuilder.Entity<Classroom>()
                .HasMany(c => c.ClassroomScreenings)
                .WithRequired()
                .HasForeignKey(c => c.ClassroomID);

            //Teacher to Class Many to Many with tertiary table TeacherClass
            /*-------------------------------------------------------*/
            modelBuilder.Entity<TeacherClass>()
                .HasKey(t => new { t.TeacherID, t.ClassroomID });

            modelBuilder.Entity<Teacher>()
                .HasMany(c => c.TeacherClasses)
                .WithRequired()
                .HasForeignKey(c => c.TeacherID);

            modelBuilder.Entity<Classroom>()
                .HasMany(c => c.TeacherClasses)
                .WithRequired()
                .HasForeignKey(c => c.ClassroomID);

            /*------------------------------------------------------*/


            //Child to Facility Many to Many with tertiary table Child_Facility
            /*-------------------------------------------------------*/
            modelBuilder.Entity<Child_Facility>()
                .HasKey(t => new { t.FacilityID, t.ChildID });

            modelBuilder.Entity<Facility>()
                .HasMany(c => c.Child_Facilities)
                .WithRequired()
                .HasForeignKey(c => c.FacilityID);

            modelBuilder.Entity<Child>()
                .HasMany(c => c.Child_Facilities)
                .WithRequired()
                .HasForeignKey(c => c.ChildID);

            /*------------------------------------------------------*/

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

            modelBuilder.Entity<Facility>()
                .HasMany(e => e.WaiverCurrents)
                .WithOptional(e => e.Site)
                .HasForeignKey(e => e.SiteID);

            modelBuilder.Entity<Facility>()
                .HasMany(e => e.WaiverRequests)
                .WithOptional(e => e.Site)
                .HasForeignKey(e => e.SiteID);

            modelBuilder.Entity<Teacher>()
                .HasMany(e => e.WaiverCurrents)
                .WithOptional(e => e.Staff)
                .HasForeignKey(e => e.StaffID);

            modelBuilder.Entity<Teacher>()
                .HasMany(e => e.WaiverRequests)
                .WithOptional(e => e.Staff)
                .HasForeignKey(e => e.StaffID);

            modelBuilder.Entity<Facility>()
                .HasRequired(e => e.Provider);

            modelBuilder.Entity<Family>()
                .Property(e => e.Income)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Family>()
                .HasMany(e => e.Children)
                .WithOptional(e => e.Family)
                .HasForeignKey(e => e.FamilyID);

            modelBuilder.Entity<Family>()
                .HasMany(e => e.Adults)
                .WithOptional(e => e.Family)
                .HasForeignKey(e => e.FamilyID);

            modelBuilder.Entity<LU_State>()
                .HasMany(e => e.Addresses)
                .WithOptional(e => e.LU_State)
                .HasForeignKey(e => e.State_ID);

            modelBuilder.Entity<Screening>()
                .HasMany(e => e.ChildScreenings)
                .WithRequired(e => e.Screening)
                .WillCascadeOnDelete(false);

            /*modelBuilder.Entity<Service>()
                .HasMany(e => e.Classrooms)
                .WithOptional(e => e.Service)
                .HasForeignKey(e => e.Program_ID);*/

            modelBuilder.Entity<Child_Classroom_Enrollment>()
                .HasKey(t => new { t.ChildID, t.ClassroomID });

            //classroom has many enrollments

            //child has many enrollments

            modelBuilder.Entity<Teacher>()
                .Property(e => e.TeacherSalary)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Address>()
                .HasMany(e => e.Institutions)
                .WithOptional(e => e.Address)
                .HasForeignKey(e => e.MailingAddressId);

            modelBuilder.Entity<Address>()
                .HasMany(e => e.Institutions1)
                .WithOptional(e => e.Address1)
                .HasForeignKey(e => e.MailingAddressId);

            modelBuilder.Entity<Child>()
                .HasOptional(e => e.Address);

            modelBuilder.Entity<Child>()
                .HasOptional(e => e.Generation);

            modelBuilder.Entity<Child>()
                .HasOptional(e => e.OtherNameType);

            modelBuilder.Entity<Child>()
                .HasOptional(e => e.Gender);

            modelBuilder.Entity<Child>()
                .HasOptional(e => e.Language);

            modelBuilder.Entity<ContactAgent>()
                .HasMany(e => e.Institutions)
                .WithOptional(e => e.ContactAgent)
                .HasForeignKey(e => e.ContactAgentId);

            modelBuilder.Entity<ContactAgent>()
                .HasMany(e => e.Institutions1)
                .WithOptional(e => e.ContactAgent1)
                .HasForeignKey(e => e.DirectorAgentId);

            modelBuilder.Entity<Institution>()
                .HasOptional(e => e.ParentHub);

            modelBuilder.Entity<CLASS_Score>()
                .HasRequired(e => e.Classroom);

            modelBuilder.Entity<Institution>()
                .HasMany(e => e.ContactAgents)
                .WithOptional(e => e.Institution)
                .HasForeignKey(e => e.InstitutionId);

            modelBuilder.Entity<Classroom>()
                .HasMany(e => e.CLASSScores)
                .WithRequired(e => e.Classroom)
                .HasForeignKey(e => e.Classroom_id);

            base.OnModelCreating(modelBuilder);
        }

        public System.Data.Entity.DbSet<PromiseData.ViewModels.AdultFormViewModel> AdultFormViewModels { get; set; }

        public System.Data.Entity.DbSet<PromiseData.ViewModels.FacilityViewModel> FacilityViewModels { get; set; }

        public System.Data.Entity.DbSet<PromiseData.ViewModels.AddressViewModel> AddressViewModels { get; set; }

        public System.Data.Entity.DbSet<PromiseData.ViewModels.FamilyViewModel> FamilyViewModels { get; set; }

        public System.Data.Entity.DbSet<PromiseData.ViewModels.InstitutionFormViewModel> InstitutionViewModels { get; set; }

        public System.Data.Entity.DbSet<PromiseData.ViewModels.ChildDetailsViewModel> ChildDetailsViewModels { get; set; }
    }
}