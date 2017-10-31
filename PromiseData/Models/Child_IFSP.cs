namespace PromiseData.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Child_IFSP")]
    public partial class Child_IFSP
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ChildID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IFSP_Code { get; set; }

        [ForeignKey("ChildID")]
        public virtual Child Child { get; set; }

        [ForeignKey("IFSP_Code")]
        public virtual Code_IFSP IFSP { get; set; }
    }
}
