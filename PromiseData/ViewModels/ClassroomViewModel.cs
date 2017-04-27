using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PromiseData.Models;

namespace PromiseData.ViewModels
{
    public class ClassroomViewModel
    {
        public int ID { get; set; }

        public int Facility_ID { get; set; }

        public int Program_ID { get; set; }

        public int ProgramSessionType_ID { get; set; }

        public string NewOrExpandedClass { get; set; }

        public double SessionHours { get; set; }

        public int SessionDays { get; set; }

        public int SessionWeeks { get; set; }

        public int PPStudents { get; set; }

        public int NonPPStudentsHSOPK { get; set; }

        public int NonPPStudentsThirdParty { get; set; }

        public int NonPPStudentsParentPay { get; set; }

        public int PPSlotsUnfilled { get; set; }

        public int CLASSScore_EmotionalSupport { get; set; }

        public int CLASSScore_ClassroomOrganization { get; set; }

        public int CLASSScore_InstructionalSupport { get; set; }

        public byte[] upsize_ts { get; set; }

        public IEnumerable<Facility> Facilities { get; set; }

        public IEnumerable<Code_ProgramSessionType> SessionTypes { get; set; }

        public IEnumerable<Service> Services { get; set; }
    }
}