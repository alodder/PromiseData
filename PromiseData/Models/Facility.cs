using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PromiseData.Models
{
    public class Facility
    {
        public int Id { get; set; }
        public String ProviderFacilityType { get; set; } // integer typeId
        public String TurnoverNonPPStaff { get; set; }
        public String TurnoverNonPPStaffReasons { get; set; }
        public bool TransportationOffered { get; set; }
        public int TransportationCount { get; set; }
        public int[] AdditionalSupports { get; set; } // size 18, should be separate table/class
        public DateTime MonitoringVisit1 { get; set; }
        public String MonitoringResult1 { get; set; }
        public DateTime MonitoringVisit2 { get; set; }
        public String MonitoringResult2 { get; set; }
    }
}