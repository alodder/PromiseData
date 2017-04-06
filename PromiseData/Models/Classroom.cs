using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PromiseData.Models
{
    public class Classroom
    {
        public int ClassroomId { get; set; }
        public int FacilityId { get; set; }
        public int ProgramId { get; set; }
        public int ProgramSessionId { get; set; }
        public String NewOrExpandedClass { get; set; }
        public float SessionHours { get; set; }
        public int SessionDays { get; set; }
        public int SessionWeeks  { get; set; }
        public int PPStudentCount { get; set; } //Calculated? max 50
        public int NonPPStudentCount { get; set; } //
        public int NonPPStudentParentsPayCount { get; set; } //
        public int PPSlotsUnfilled { get; set; }  //max 50
        public int ClassScoreEmotionalSupport { get; set; }
        public int ClassScoreEmotionalOrganization { get; set; }
        public int ClassScoreInstructionalSupport { get; set; }
        //timestamp
    }
}