using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PromiseData.Models
{
    [Table("WaiverCurrent")]
    public class WaiverCurrent
    {
        public int WaiverCurrentID { get; set; }


        public string WaiverType { get; set; }

        //Site Waiver
        public int? SiteID { get; set; }

        //Sites current rating
        [DisplayName("Current Spark Level")]//waiver, 4, or 5
        public string SparkCurrent { get; set; }


        //
        //Staff Waiver
        [DisplayName("Staff")]
        public int? StaffID { get; set; }

        //for Lead - obtained BA
        //for support - obtainedd Step advancement
        //made progress
        [DisplayName("Personnel Qualification")]
        public string Qualification { get; set; }

        //training/college classes
        [DisplayName("Professional Development")]
        public string Development { get; set; }

        //Credit Hours Earned
        [DisplayName("Credit Hours")]
        public int Credits { get; set; }

        //Training hours earned
        [DisplayName("Training Hours Earned")]
        public int TrainingHours { get; set; }

        //
        //900 Service Hours 
        [DisplayName("900 Service Hours")]
        public bool NineHundredServiceHours { get; set; }

        [DisplayName("Impact Service Hour")]
        public string ServiceHourImpact { get; set; }

        public string ServiceHourImpactOther { get; set; }

        public int ServiceHourCount { get; set; }

        [DataType(DataType.MultilineText)]
        [DisplayName("Additional Comments")]
        public string AdditionalComments { get; set; }

        [DisplayName("Waiver Expiration")]
        public DateTime? Expiration { get; set; }
        
        //Waiver Unsatisfied
        public bool Unsatisfied { get; set; }

        public virtual Teacher Staff { get; set; }

        public virtual Facility Site { get; set; }

    }
}