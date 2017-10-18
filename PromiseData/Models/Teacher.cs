namespace PromiseData.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Teacher")]
    public partial class Teacher
    {
        public int ID { get; set; }

        [DisplayName("Teacher ID")]
        [StringLength(255)]
        public string TeacherIDNumber { get; set; }

        [DisplayName("Staff Type")]
        [StringLength(255)]
        public string TeacherType { get; set; }

        [DisplayName("Birthdate")]
        public DateTime? TeacherBirthdate { get; set; }

        [DisplayName("Gender")]
        public char Gender_ID { get; set; }

        [DisplayName("Race/Ethnicity")]
        public int? TeacherRaceEthnicity { get; set; }

        [DisplayName("Classroom Languages")]
        [StringLength(255)]
        public string Languages_spoken_in_classroom { get; set; }

        [DisplayName("Language Fluency")]
        [StringLength(255)]
        public string FluentLanguages { get; set; }

        [DisplayName("Start Date")]
        public DateTime? StartDate { get; set; }

        [DisplayName("Salary")]
        [Column(TypeName = "money")]
        public decimal? TeacherSalary { get; set; }

        [DisplayName("Education")]
        public int? Education_ID { get; set; }

        public bool CDA { get; set; }

        [DisplayName("Degree Field")]
        [StringLength(255)]
        public string DegreeField { get; set; }

        public int? PDStep { get; set; }

        [DisplayName("Years of Experience")]
        public int? YearsExperience { get; set; }

        [DisplayName("End Date")]
        public DateTime? EndDate { get; set; }

        [DisplayName("Reason For Leaving")]
        [StringLength(255)]
        public string ReasonForleaving { get; set; }

        [DisplayName("First Name")]
        [StringLength(50)]
        public string NameFirst { get; set; }

        [DisplayName("Last Name")]
        [StringLength(50)]
        public string NameLast { get; set; }

        [DisplayName("Classes")]
        public virtual ICollection<TeacherClass> TeacherClasses { get; set; }
    }
}
