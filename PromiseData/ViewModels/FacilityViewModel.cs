using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PromiseData.Models;

namespace PromiseData.ViewModels
{

    public class FacilityViewModel
    {
        public int ID { get; set; }

        public string ProviderFacilityType { get; set; }
        public List<String> FacilityTypes { get; set; }

        public string Turnover_NonPPStaff { get; set; }

        public string TurnoverReasons_NonPPStaff { get; set; }

        public bool Transportation_services_offered { get; set; }

        public int ChildrenReceivingTransportationServices { get; set; }

        public int AdditionalChildFamilySupports_ID { get; set; }
        public IEnumerable<Code_AdditionalSupportTypes> SupportTypes { get; set; }

        public DateTime MonitoringVisit1Date { get; set; }

        public string MonitoringVisit1Result { get; set; }

        public DateTime MonitoringVisit2Date { get; set; }

        public string MonitoringVisit2Result { get; set; }

        public string Description { get; set; }
    }
}