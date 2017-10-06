using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PromiseData.Models
{
    public class WaiverCurrent
    {
        public int WaiverCurrentID { get; set; }

        public int WaiverType { get; set; }

        public int SiteID { get; set; }

        //Sites current rating
        public string SparkCurrent { get; set; }

        //Staff Waiver
        public int StaffID { get; set; }

        public string Qualification { get; set; }

        public string Development { get; set; }

        public int Credits { get; set; }

        public int TrainingHours { get; set; }

        //900 Service Hours 
        public bool NineHundredServiceHours { get; set; }

        public string ServiceHourImpact { get; set; }

        public string ServiceHourImpactOther { get; set; }

        public int ServiceHourCount { get; set; }

        public string AdditionalComments { get; set; }

        //Waiver Unsatisfied
        public bool Unsatisfied { get; set; }

        public virtual Teacher Staff { get; set; }

        public virtual Facility Site { get; set; }

    }
}