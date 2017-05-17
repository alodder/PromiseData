using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PromiseData.Models;

namespace PromiseData.ViewModels
{
    public class ChildRaceViewModel
    {

        public int ChildID { get; set; }

        public int RaceID { get; set; }

        public Dictionary<int, bool> RaceDictionary;
    }
}