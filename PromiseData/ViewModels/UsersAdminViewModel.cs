using PromiseData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PromiseData.ViewModels
{
    public class UsersAdminViewModel
    {
        public IEnumerable<ApplicationUser> Users { get; set; }
        public bool ShowActions { get; set; }
        public string Heading { get; set; }
        public string SearchTerm { get; set; }
        public Boolean CanAdd { get; set; }
        public Boolean CanEdit { get; set; }
        public Boolean CanDelete { get; set; }
        public Boolean CanView { get; set; }
    }
}