namespace PromiseData.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Screening")]
    public partial class Screening
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Screening()
        {
            ChildScreenings = new HashSet<ChildScreening>();
        }

        public int ID { get; set; }

        [StringLength(255)]
        public string ScreenName { get; set; }

        [StringLength(255)]
        public string ScreenDate { get; set; }

        [StringLength(255)]
        public string Communication_Score { get; set; }

        [StringLength(255)]
        public string Communication_Determination { get; set; }

        [StringLength(255)]
        public string Gross_Motor_Score { get; set; }

        [StringLength(255)]
        public string Gross_Motor_Determination { get; set; }

        [StringLength(255)]
        public string FineMotorScore { get; set; }

        [StringLength(255)]
        public string FineMotorDetermination { get; set; }

        [StringLength(255)]
        public string ProblemSolvingScore { get; set; }

        [StringLength(255)]
        public string ProblemSolvingDetermination { get; set; }

        [StringLength(255)]
        public string PersonalSocialScore { get; set; }

        [StringLength(255)]
        public string PersonalSocialDetermination { get; set; }

        [StringLength(255)]
        public string EIECSE_ReferralDate { get; set; }

        [StringLength(255)]
        public string EIECSE_Services_Date { get; set; }

        [StringLength(255)]
        public string EIECSE_PrimaryDisabilityType { get; set; }

        [StringLength(255)]
        public string EIECSE_Secondary_Type { get; set; }

        [StringLength(255)]
        public string Assessment_Name { get; set; }

        [StringLength(255)]
        public string Initial_Assessment_Result { get; set; }

        [StringLength(255)]
        public string Midyear_Assessment_Result { get; set; }

        [StringLength(255)]
        public string Endofyear_Assessment_Result { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChildScreening> ChildScreenings { get; set; }
    }
}
