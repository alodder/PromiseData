using PromiseData.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PromiseData.ViewModels
{
    public class InstitutionViewModel
    {
        public int Id { get; set; }

        [Required]

        public string Heading { get; set; }

        public string SearchTerm { get; set; }

        public string LegalName { get; set; }

        public string Region { get; set; }

        public string BackboneOrg { get; set; }

        public string WebAddress { get; set; }

        public int? DirectorAgentId { get; set; }

        public int? ContactAgentId { get; set; }

        public int? LocationAddressId { get; set; }

        public int? MailingAddressId { get; set; }

        public DateTime ActiveDate { get; set; }

        public DateTime? EndDate { get; set; }

        public bool isHub { get; set; }

        public bool isProvider { get; set; }

        public IEnumerable<LU_State> States { get; set; }

        public virtual ContactAgent DirectorAgent { get; set; }

        public virtual ContactAgent ContactAgent { get; set; }

        public virtual Address AddressMail { get; set; }

        public virtual Address AddressPhysical { get; set; }
    }
}