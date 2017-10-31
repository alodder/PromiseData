using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.Linq;
using System.Web;
using PromiseData.Models;
using static System.DateTime;

namespace PromiseData.ViewModels
{
    public class ChildSpecialViewModel
    {
        public Boolean CanView { get; set; }
        public Boolean CanEdit { get; set; }
        public Boolean CanDelete { get; set; }

        [DisplayName("Child")]
        public int ChildID { get; set; }

        public Child Child { get; set; }

        public virtual ICollection<Child_Special_Needs> Child_Special_Needs { get; set; }

        public virtual ICollection<Child_IFSP> Child_IFSP { get; set; }

        //List of all Possible Special Needs
        public virtual ICollection<Special_Needs> Special_Needs { get; set; }

        [DisplayName("Child Special Needs")]
        //dictionary for checkbox values
        public Dictionary<int, bool> MySpecialNeeds { get; set; }

        //List of all Possible Code_IFSP
        public virtual ICollection<Code_IFSP> IFSPs { get; set; }

        [DisplayName("Child IFSP")]
        //dictionary for checkbox values
        public Dictionary<int, bool> MyIFSP { get; set; }
    }
}