using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using PromiseData.Models;

namespace PromiseData.ViewModels
{
    public class ContactAgentViewModel
    {
        public int id { get; set; }

        [DisplayName("Name")]
        public String Name { get; set; }
    }
}