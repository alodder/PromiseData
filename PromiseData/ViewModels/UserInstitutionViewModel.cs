using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PromiseData.ViewModels
{
    public class UserInstitutionViewModel
    {
        public String UserName { get; set; }

        public String Id { get; set; }

        public PromiseData.Models.Institution CurrentInstitution { get; set; }

        public string[] ListInstitutionNames { get; set; }

        public string[] ListSelectedInstitutionNames { get; set; }

        public IEnumerable<PromiseData.Models.Institution> Roles { get; set; }
    }
}