using PromiseData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PromiseData.ViewModels
{
    public class WaiversProcessViewModel
    {
        public IEnumerable<WaiverCurrent> WaiverCurrents { get; set; }
        public IEnumerable<WaiverRequest> WaiverRequests { get; set; }

        public bool ShowActions { get; set; }
        public string Heading { get; set; }
        public string SearchTerm { get; set; }

        public Boolean CanAdd { get; set; }
        public Boolean CanEdit { get; set; }
        public Boolean CanDelete { get; set; }
        public Boolean CanView { get; set; }
    }
}