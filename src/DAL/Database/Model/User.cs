namespace DAL.Database.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            Classes_Users_Bridge = new HashSet<Classes_Users_Bridge>();
            Filter_Users_Bridge = new HashSet<Filter_Users_Bridge>();
        }

        [Key]
        [StringLength(20)]
        public string ID { get; set; }

        [StringLength(256)]
        public string First_Name { get; set; }

        [StringLength(256)]
        public string Barcode { get; set; }

        [StringLength(256)]
        public string RFID { get; set; }

        [StringLength(256)]
        public string Last_Name { get; set; }

        [StringLength(256)]
        public string Form_Class { get; set; }

        [StringLength(256)]
        public string Preferred_First_Name { get; set; }

        [StringLength(256)]
        public string Preferred_Last_Name { get; set; }

        public DateTime? DOB { get; set; }

        [StringLength(5)]
        public string Year_Level { get; set; }

        [StringLength(10)]
        public string UserName { get; set; }

        [StringLength(1)]
        public string Sex { get; set; }

        [StringLength(2)]
        public string Enrolment_Status { get; set; }

        public DateTime? Date_Enrolled { get; set; }

        public DateTime? Exit_Date { get; set; }

        [StringLength(50)]
        public string House { get; set; }

        public long? Indigenous_Status { get; set; }

        [StringLength(1)]
        public string Independent_Status { get; set; }

        public long? User_Type_ID { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Classes_Users_Bridge> Classes_Users_Bridge { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Filter_Users_Bridge> Filter_Users_Bridge { get; set; }

        public virtual User_Type User_Type { get; set; }
    }
}
