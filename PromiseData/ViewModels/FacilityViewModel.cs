using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using PromiseData.Models;
using PromiseData.Controllers;
using System.ComponentModel.DataAnnotations;

namespace PromiseData.ViewModels
{

    public class FacilityViewModel
    {
        public Boolean CanView { get; set; }
        public Boolean CanEdit { get; set; }
        public Boolean CanDelete { get; set; }

        public int ID { get; set; }

        public String Action
        {
            get
            {
                Expression<Func<FacilityController, ActionResult>> update =
                    (c => c.Update(this));
                Expression<Func<FacilityController, ActionResult>> create =
                    (c => c.Create(this));

                var action = (ID != 0) ? update : create;
                return (action.Body as MethodCallExpression).Method.Name;
            }

        }

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

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DisplayName("Monitoring Visit Date #1")]
        public DateTime? MonitoringVisit1Date { get; set; }

        [DisplayName("Monitoring Visit Result #1")]
        public string MonitoringVisit1Result { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DisplayName("Monitoring Visit Date #2")]
        public DateTime? MonitoringVisit2Date { get; set; }

        [DisplayName("Monitoring Visit Result #2")]
        public string MonitoringVisit2Result { get; set; }

        [Required]
        [DisplayName("Name")]
        public string Description { get; set; }

        [DisplayName("Parent Operator")]
        public int OperatorId { get; set; }

        [Required]
        [StringLength(10)]
        [DisplayName("Child Care License")]
        public string LicenseNumber { get; set; }

        [DisplayName("Unlicensed")]
        public bool Unlicensed { get; set; }

        [StringLength(15)]
        [DisplayName("Phone")]
        public string Phone { get; set; }

        [StringLength(50)]
        [DisplayName("Email")]
        public string Email { get; set; }

        [DisplayName("Operator")]
        public Institution Operator { get; set; }

        [DisplayName("Address")]
        public Address Address { get; set; }

        [DisplayName("ContactAgent")]
        public ContactAgent ContactAgent { get; set; }

        [DisplayName("Current Spark Rating")]
        public string SparkRating { get; set; }

        public List<String> SparkRatingList
        {
            get
            {
                List<String> types = new List<String>();
                types.Add("Unlicensed");
                types.Add("Licensed");
                types.Add("Commitment to Quality status(C2Q)");
                types.Add("Portfolio Submitted(awaiting star designation)");
                types.Add("3 Star rated");
                types.Add("4 Star rated");
                types.Add("5 Star rated");
                return types;
            }
        }

        public IEnumerable<Code_AdditionalSupportTypes> Supports { get; set; }

        public IEnumerable<Code_AdditionalSupportTypes> SupportsList { get; set; }

        public string[] SupportsCodeList { get; set; }

        public string[] SelectedSupports { get; set; }

        public IEnumerable<WaiverCurrent> WaiversCurrent { get; set; }

        public IEnumerable<WaiverRequest> WaiverRequests { get; set; }

        public IEnumerable<Classroom> Classrooms { get; set; }

        public IEnumerable<Institution> Institutions { get; set; }

        public IEnumerable<ProgramYear> ProgramYears { get; set; }
    }
}