namespace PromiseData.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Teacher")]
    public partial class Teacher
    {
        public int ID { get; set; }

        [StringLength(255)]
        public string TeacherIDNumber { get; set; }

        [StringLength(255)]
        public string TeacherType { get; set; }

        public DateTime? TeacherBirthdate { get; set; }

        public int? Gender_ID { get; set; }

        public int? TeacherRaceEthnicity { get; set; }

        [StringLength(255)]
        public string Languages_spoken_in_classroom { get; set; }

        [StringLength(255)]
        public string FluentLanguages { get; set; }

        public DateTime? StartDate { get; set; }

        [Column(TypeName = "money")]
        public decimal? TeacherSalary { get; set; }

        public int? Education_ID { get; set; }

        public bool CDA { get; set; }

        [StringLength(255)]
        public string DegreeField { get; set; }

        public int? PDStep { get; set; }

        public int? YearsExperience { get; set; }

        public DateTime? EndDate { get; set; }

        [StringLength(255)]
        public string ReasonForleaving { get; set; }

        public virtual ICollection<Classroom> Classrooms { get; set; }
    }
}
