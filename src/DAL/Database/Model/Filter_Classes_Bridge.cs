namespace DAL.Database.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class Filter_Classes_Bridge
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }

        public long? Exports_ID { get; set; }

        [StringLength(100)]
        public string Classes_Class_Code { get; set; }

        public virtual Export Export { get; set; }
        public virtual Class Class { get; set; }

        
    }
}
