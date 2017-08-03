using PromiseData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PromiseData.ViewModels
{
    public class InstitutionsViewModel
    {
        public IEnumerable<Institution> Institutions { get; set; }
        public bool ShowActions { get; set; }
        public string Heading { get; set; }
        public string SearchTerm { get; set; }
    }
}