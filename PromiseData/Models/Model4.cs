namespace PromiseData.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Model4 : DbContext
    {
        public Model4()
            : base("name=Model4")
        {
        }

        public virtual DbSet<TeacherLanguageClassroom> TeacherLanguageClassrooms { get; set; }
        public virtual DbSet<TeacherLanguageFluency> TeacherLanguageFluencies { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
