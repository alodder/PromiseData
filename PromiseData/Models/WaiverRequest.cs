namespace PromiseData.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("WaiverRequest")]
    public class WaiverRequest
    {
        public int WaiverRequestID { get; set; }

        [DisplayName("Waiver Type")]
        public string WaiverType { get; set; }

        [DisplayName("Site")]
        public int? SiteID { get; set; }

        //Sites current rating
        [DisplayName("Current Spark Rating")]
        public string SparkCurrent { get; set; }

        //Staff Waiver
        [DisplayName("Staff")]
        public int? StaffID { get; set; }

        public string Qualification { get; set; }

        [DataType(DataType.MultilineText)]
        [DisplayName("Additional Comments")]
        public string AdditionalComments { get; set; }

        public virtual Teacher Staff { get; set; }

        public virtual Facility Site { get; set; }

    }
}