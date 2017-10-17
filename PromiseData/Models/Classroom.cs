namespace PromiseData.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Classroom")]
    public partial class Classroom
    {
        //[DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        public int? Facility_ID { get; set; }

        public int? Program_ID { get; set; }

        public int? ProgramSessionType_ID { get; set; }

        [DisplayName("New Or Expanded Class")]
        [StringLength(10)]
        public string NewOrExpandedClass { get; set; }

        [DisplayName("Session Hours")]
        public double? SessionHours { get; set; }

        [DisplayName("Session Days")]
        public int? SessionDays { get; set; }

        [DisplayName("Session Weeks")]
        public int? SessionWeeks { get; set; }

        [DisplayName("Preschool Promise Students")]
        public int? PPStudents { get; set; }

        [DisplayName("HSOPK")]
        public int? NonPPStudentsHSOPK { get; set; }

        [DisplayName("Third Party")]
        public int? NonPPStudentsThirdParty { get; set; }

        [DisplayName("Parent Pay")]
        public int? NonPPStudentsParentPay { get; set; }

        [DisplayName("Slots Unfilled")]
        public int? PPSlotsUnfilled { get; set; }

        [DisplayName("Emotional Support")]
        public int? CLASSScore_EmotionalSupport { get; set; }

        [DisplayName("Classroom Organization")]
        public int? CLASSScore_ClassroomOrganization { get; set; }

        [DisplayName("Instructional Support")]
        public int? CLASSScore_InstructionalSupport { get; set; }

        [Column(TypeName = "timestamp")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [MaxLength(8)]
        public byte[] upsize_ts { get; set; }

        public string Description { get; set; }

        public virtual Code_ProgramSessionType Code_ProgramSessionType { get; set; }

        public virtual Facility Facility { get; set; }

        public virtual Service Service { get; set; }

        public virtual ICollection<CLASS_Score> CLASSScores { get; set; }

        public virtual ICollection<TeacherClass> TeacherClasses { get; set; }
    }
}
