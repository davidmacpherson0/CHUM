namespace DAL.Database.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Classes_Users_Bridge
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }

        [StringLength(100)]
        public string Classes_Class_Code { get; set; }

        [StringLength(20)]
        public string Users_ID { get; set; }

        public virtual Class Class { get; set; }

        public virtual User User { get; set; }

    }
}
