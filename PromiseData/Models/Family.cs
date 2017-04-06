using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PromiseData.Models
{
    public class Family
    {
        public int Id { get; set; }
        public int HouseholdSize { get; set; }
        public int ChildrenInHome { get; set; }
        public int Income { get; set; }
        public Boolean SNAP { get; set; }
        public Boolean WIC { get; set; }
        public Boolean TANF { get; set; }
        public Boolean SSI { get; set; }
        public int MonthlyCostAdjust { get; set; } //perhaps decimal type, database is int
    }
}