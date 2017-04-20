namespace PromiseData.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Hub
    {
        public Hub()
        {
            Providers = new HashSet<Provider>();
        }

        public int ID { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Provider> Providers { get; set; }
    }
}
