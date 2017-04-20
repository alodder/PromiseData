namespace PromiseData.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Provider
    {
        public Provider()
        {
            Classrooms = new HashSet<Classroom>();
            Students = new HashSet<Child>();
        }

        public int ID { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Classroom> Classrooms { get; set; }

        public virtual ICollection<Child> Students { get; set; }
    }
}
