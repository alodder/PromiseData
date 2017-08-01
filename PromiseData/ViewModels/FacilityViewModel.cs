using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PromiseData.Models;
using System.ComponentModel;

namespace PromiseData.ViewModels
{

    public class FacilityViewModel
    {
        public int ID { get; set; }

        public string Heading { get; set; }

        [DisplayName("Provider Facility Type")]
        public string ProviderFacilityType { get; set; }

        public List<String> FacilityTypes { get; set; }

        [DisplayName("Turnover - Non-PPStaff")]
        public string Turnover_NonPPStaff { get; set; }

        [DisplayName("Turnover Reasons - Non-PPStaff")]
        public string TurnoverReasons_NonPPStaff { get; set; }

        [DisplayName("Transportation Services Offered")]
        public bool Transportation_services_offered { get; set; }

        [DisplayName("Number Children Receiving Transportation Service")]
        public int ChildrenReceivingTransportationServices { get; set; }

        [DisplayName("Additional Family Supports")]
        public int AdditionalChildFamilySupports_ID { get; set; }

        [DisplayName("Additional Family Supports")]
        public IEnumerable<Code_AdditionalSupportTypes> SupportTypes { get; set; }
        
        //dictionary for checkbox values
        public Dictionary<int, bool> SupportDictionary { get; set; }

        [DisplayName("Monitoring Visit Date #1")]
        public DateTime MonitoringVisit1Date { get; set; }

        [DisplayName("Monitoring Visit Result #1")]
        public string MonitoringVisit1Result { get; set; }

        [DisplayName("Monitoring Visit Date #2")]
        public DateTime MonitoringVisit2Date { get; set; }

        [DisplayName("Monitoring Visit Result #2")]
        public string MonitoringVisit2Result { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }
    }
}