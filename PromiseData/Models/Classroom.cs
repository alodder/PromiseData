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

        [StringLength(10)]
        public string NewOrExpandedClass { get; set; }

        public double? SessionHours { get; set; }

        public int? SessionDays { get; set; }

        public int? SessionWeeks { get; set; }

        public int? PPStudents { get; set; }

        public int? NonPPStudentsHSOPK { get; set; }

        public int? NonPPStudentsThirdParty { get; set; }

        public int? NonPPStudentsParentPay { get; set; }

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

        public virtual ICollection<TeacherClass> TeacherClasses { get; set; }
    }
}
