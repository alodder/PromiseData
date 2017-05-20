﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PromiseData.Models;
using System.ComponentModel;

namespace PromiseData.ViewModels
{
    public class ClassroomViewModel
    {
        public int ID { get; set; }

        [DisplayName("Facility")]
        public int Facility_ID { get; set; }

        [DisplayName("Program")]
        public int Program_ID { get; set; }

        [DisplayName("Program Session Type")]
        public int ProgramSessionType_ID { get; set; }

        [DisplayName("New or Expanded Class")]
        public string NewOrExpandedClass { get; set; }

        [DisplayName("Session Hours")]
        public double SessionHours { get; set; }

        [DisplayName("Session Days")]
        public int SessionDays { get; set; }

        [DisplayName("Session Weeks")]
        public int SessionWeeks { get; set; }

        [DisplayName("Preschool Promise Students")]
        public int PPStudents { get; set; }

        [DisplayName("Non-PP HSOPK")]
        public int NonPPStudentsHSOPK { get; set; }

        [DisplayName("Non-PP 3rd Party")]
        public int NonPPStudentsThirdParty { get; set; }

        [DisplayName("Non-PP Parent Pay")]
        public int NonPPStudentsParentPay { get; set; }

        [DisplayName("PP Slots Unfilled")]
        public int PPSlotsUnfilled { get; set; }

        [DisplayName("CLASSScore Emotional Support")]
        public int CLASSScore_EmotionalSupport { get; set; }

        [DisplayName("CLASSScore Classroom Organization")]
        public int CLASSScore_ClassroomOrganization { get; set; }

        [DisplayName("CLASSScore Instructional Support")]
        public int CLASSScore_InstructionalSupport { get; set; }

        [DisplayName("Upsize")]
        public byte[] upsize_ts { get; set; }

        public IEnumerable<Facility> Facilities { get; set; }

        public IEnumerable<Code_ProgramSessionType> SessionTypes { get; set; }

        public IEnumerable<Service> Services { get; set; }
    }
}