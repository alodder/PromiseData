namespace PromiseData.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Service
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Service()
        {
            Classrooms = new HashSet<Classroom>();
            Child_Services_Enrollment = new HashSet<Child_Services_Enrollment>();
        }

        public int ID { get; set; }

        [DisplayName("Early Childhood Services Received")]
        [StringLength(255)]
        public string Early_Childhood_Services_Received { get; set; }

        [DisplayName("Enrollment Year Start Date")]
        [StringLength(255)]
        public DateTime PP_Program_Enrollment_Year_Start_Date { get; set; }

        [DisplayName("Expected Annual Attendance Days")]
        public int Expected_Annual_Attendance_Days { get; set; }

        [DisplayName("Class Session Attendance Quantity")]
        public int Class_Session_Attendance_Quantity { get; set; }

        [DisplayName("Class Session Attendance Units")]
        [StringLength(255)]
        public string Class_Session_Attendance_Units { get; set; }

        [DisplayName("Exit Date")]
        public DateTime PP_Exit_Date { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Classroom> Classrooms { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Child_Services_Enrollment> Child_Services_Enrollment { get; set; }
    }
}
