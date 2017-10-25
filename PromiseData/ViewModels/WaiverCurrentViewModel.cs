using PromiseData.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PromiseData.ViewModels
{
    public class WaiverCurrentViewModel
    {
        public int WaiverCurrentID { get; set; }

        public int WaiverRequestID { get; set; }

        [DisplayName("Waiver Type")]
        public string WaiverType { get; set; }

        [DisplayName("Site")]
        public int SiteID { get; set; }

        [DisplayName("Current Spark Level")]//waiver, 4, or 5
        public string SparkCurrent { get; set; }

        //Staff Waiver
        [DisplayName("Staff")]
        public int StaffID { get; set; }

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

        [DisplayName("Service Hour Impairment")]
        public string ServiceHourImpact { get; set; }

        [DisplayName("If Other, please explain")]
        public string ServiceHourImpactOther { get; set; }

        [DisplayName("Service Hour Count")]
        public int ServiceHourCount { get; set; }

        [DisplayName("Additional Comments")]
        public string AdditionalComments { get; set; }

        //Waiver Unsatisfied
        public bool Unsatisfied { get; set; }

        public virtual WaiverRequest WaiverRequest { get; set; }

        public virtual Teacher Staff { get; set; }

        public virtual Facility Site { get; set; }

        public IEnumerable<Teacher> Staffs { get; set; }

        public IEnumerable<Facility> Sites { get; set; }
    }
}