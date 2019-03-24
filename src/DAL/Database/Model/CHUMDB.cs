namespace DAL.Database.Model
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class CHUMDB : DbContext
    {
        public CHUMDB()
            : base("name=CHUMDB")
        {
            this.Configuration.ProxyCreationEnabled = false;
        }

        public virtual DbSet<DAL.Database.Model.Class> Classes { get; set; }
        public virtual DbSet<Classes_Users_Bridge> Classes_Users_Bridge { get; set; }
        public virtual DbSet<Subject> Subjects { get; set; }
        public virtual DbSet<User_Type> User_Type { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Export> Export { get; set; }
        public virtual DbSet<Filter_Users_Bridge> Filter_Users_Bridge { get; set; }
        public virtual DbSet<Filter_Classes_Bridge> Filter_Classes_Bridge { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            #region Data Tables

            modelBuilder.Entity<User>()
                .Property(e => e.ID)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.First_Name)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Barcode)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.RFID)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Last_Name)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Preferred_First_Name)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Preferred_Last_Name)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.UserName)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Sex)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Enrolment_Status)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.House)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Independent_Status)
                .IsUnicode(false);

            modelBuilder.Entity<Subject>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Subject>()
                .Property(e => e.Prefix)
                .IsUnicode(false);

            modelBuilder.Entity<Class>()
                .Property(e => e.Class_Code)
                .IsUnicode(false);

            modelBuilder.Entity<Class>()
                .Property(e => e.Subject_Prefix)
                .IsUnicode(false);

            modelBuilder.Entity<Class>()
                .Property(e => e.Class_Level)
                .IsUnicode(false);

            modelBuilder.Entity<Class>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Class>()
                .Property(e => e.Year_Level)
                .IsUnicode(false);

            modelBuilder.Entity<User_Type>()
                .Property(e => e.Label)
                .IsUnicode(false);

            modelBuilder.Entity<Export>()
                .Property(e => e.Name)
                .IsUnicode(false);

            #endregion

            #region Bridgeing Tables

            modelBuilder.Entity<Classes_Users_Bridge>()
                .Property(e => e.Classes_Class_Code)
                .IsUnicode(false);

            modelBuilder.Entity<Classes_Users_Bridge>()
                .Property(e => e.Users_ID)
                .IsUnicode(false);

            modelBuilder.Entity<Filter_Users_Bridge>()
                .Property(e => e.Users_ID)
                .IsUnicode(false);

            modelBuilder.Entity<Filter_Classes_Bridge>()
                .Property(e => e.Classes_Class_Code)
                .IsUnicode(false);
            #endregion

            #region Foreign Keys

            modelBuilder.Entity<Subject>()
                .HasMany(e => e.Classes)
                .WithOptional(e => e.Subject)
                .HasForeignKey(e => e.Subject_Prefix);

            modelBuilder.Entity<User_Type>()
                .HasMany(e => e.Users)
                .WithOptional(e => e.User_Type)
                .HasForeignKey(e => e.User_Type_ID);

            modelBuilder.Entity<Class>()
                .HasMany(e => e.Classes_Users_Bridge)
                .WithOptional(e => e.Class)
                .HasForeignKey(e => e.Classes_Class_Code);

            modelBuilder.Entity<Class>()
                .HasMany(e => e.Filter_Classes_Bridge)
                .WithOptional(e => e.Class)
                .HasForeignKey(e => e.Classes_Class_Code);

            modelBuilder.Entity<User>()
                 .HasMany(e => e.Classes_Users_Bridge)
                 .WithOptional(e => e.User)
                 .HasForeignKey(e => e.Users_ID);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Filter_Users_Bridge)
                .WithOptional(e => e.User)
                .HasForeignKey(e => e.Users_ID);

            modelBuilder.Entity<Export>()
                .HasMany(e => e.Filter_Users_Bridge)
                .WithOptional(e => e.Export)
                .HasForeignKey(e => e.Exports_ID);

            modelBuilder.Entity<Export>()
                .HasMany(e => e.Filter_Classes_Bridge)
                .WithOptional(e => e.Export)
                .HasForeignKey(e => e.Exports_ID);
            #endregion
        }
    }
}
