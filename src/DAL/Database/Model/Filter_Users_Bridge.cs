namespace DAL.Database.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class Filter_Users_Bridge
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }

        public long? Exports_ID { get; set; }

        [StringLength(20)]
        public string Users_ID { get; set; }



        public virtual User User { get; set; }

        public virtual Export Export { get; set; }

    }
}
