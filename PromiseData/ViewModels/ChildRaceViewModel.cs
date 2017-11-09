using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PromiseData.Models;
using System.Linq.Expressions;
using PromiseData.Controllers;
using System.Web.Mvc;

namespace PromiseData.ViewModels
{
    public class ChildRaceViewModel
    {

        public int ChildID { get; set; }

        public String Action
        {
            get
            {
                Expression<Func<ChildController, ActionResult>> update =
                    (c => c.UpdateRace(this));
                Expression<Func<ChildController, ActionResult>> create =
                    (c => c.Race(this));

                var action = (Update) ? update : create;
                return (action.Body as MethodCallExpression).Method.Name;
            }

        }

        public Child Child { get; set; }

        public IEnumerable<RaceEthnicity> RaceEthnicityList { get; set; }

        public Dictionary<int, bool> RaceDictionary { get; set; }

        public bool Update { get; set; }
    }
}