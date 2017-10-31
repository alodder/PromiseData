namespace PromiseData.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Child_Special_Needs")]
    public partial class Child_Special_Needs
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ChildID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int SpecialNeedsCode { get; set; }

        [ForeignKey("ChildID")]
        public virtual Child Child { get; set; }

        [ForeignKey("SpecialNeedsCode")]
        public virtual Special_Needs SpecialNeed { get; set; }
    }
}
