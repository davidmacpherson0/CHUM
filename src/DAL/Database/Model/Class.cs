namespace DAL.Database.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Class
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Class()
        {
            Classes_Users_Bridge = new HashSet<Classes_Users_Bridge>();
            Filter_Classes_Bridge = new HashSet<Filter_Classes_Bridge>();
        }

        [Key]
        [StringLength(100)]
        public string Class_Code { get; set; }

        [StringLength(512)]
        public string Name { get; set; }

        [StringLength(10)]
        public string Subject_Prefix { get; set; }

        [StringLength(5)]
        public string Year_Level { get; set; }

        public long? Semeseter_ID { get; set; }

        [StringLength(1)]
        public string Class_Level { get; set; }

        public virtual Subject Subject { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Classes_Users_Bridge> Classes_Users_Bridge { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Filter_Classes_Bridge> Filter_Classes_Bridge { get; set; }
    }
}
