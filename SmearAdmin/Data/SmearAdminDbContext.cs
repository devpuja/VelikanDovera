using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SmearAdmin.Models;

namespace SmearAdmin.Data
{
    public partial class SmearAdminDbContext : IdentityDbContext<AppUser>
    {
        public SmearAdminDbContext()
        {
        }

        //public SmearAdminDbContext(DbContextOptions<SmearAdminDbContext> options)
        //    : base(options)
        //{
        //}

        public virtual DbSet<AuditableEntity> AuditableEntity { get; set; }
        public virtual DbSet<Chemist> Chemist { get; set; }
        public virtual DbSet<ChemistStockistResourse> ChemistStockistResourse { get; set; }
        public virtual DbSet<Community> Community { get; set; }
        public virtual DbSet<ContactResourse> ContactResourse { get; set; }
        public virtual DbSet<Doctor> Doctor { get; set; }
        public virtual DbSet<Expenses> Expenses { get; set; }
        public virtual DbSet<ExpensesStatus> ExpensesStatus { get; set; }
        public virtual DbSet<HolidayList> HolidayList { get; set; }
        public virtual DbSet<Hqregion> Hqregion { get; set; }
        public virtual DbSet<MasterKeyValue> MasterKeyValue { get; set; }
        public virtual DbSet<Patient> Patient { get; set; }
        public virtual DbSet<Smslogger> Smslogger { get; set; }
        public virtual DbSet<Stockist> Stockist { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AuditableEntity>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.CommunityId).HasColumnName("CommunityID");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.CreatedBy).IsUnicode(false);

                entity.Property(e => e.FoundationDay).HasColumnType("datetime");

                entity.Property(e => e.RefTableId)
                    .HasColumnName("RefTableID")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.RefTableName)
                    .HasMaxLength(500)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Chemist>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.Class).IsUnicode(false);

                entity.Property(e => e.MedicalName).IsUnicode(false);
            });

            modelBuilder.Entity<ChemistStockistResourse>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.BestTimeToMeet).IsUnicode(false);

                entity.Property(e => e.ContactPersonDateOfAnniversary).HasColumnType("datetime");

                entity.Property(e => e.ContactPersonDateOfBirth).HasColumnType("datetime");

                entity.Property(e => e.ContactPersonMobileNumber).IsUnicode(false);

                entity.Property(e => e.ContactPersonName).IsUnicode(false);

                entity.Property(e => e.DrugLicenseNo).IsUnicode(false);

                entity.Property(e => e.FoodLicenseNo).IsUnicode(false);

                entity.Property(e => e.Gstno)
                    .HasColumnName("GSTNo")
                    .IsUnicode(false);

                entity.Property(e => e.RefTableId)
                    .HasColumnName("RefTableID")
                    .HasMaxLength(500);

                entity.Property(e => e.RefTableName)
                    .HasMaxLength(500)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Community>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CommunityName).IsUnicode(false);
            });

            modelBuilder.Entity<ContactResourse>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.Address).IsUnicode(false);

                entity.Property(e => e.Area).IsUnicode(false);

                entity.Property(e => e.City).IsUnicode(false);

                entity.Property(e => e.EmailId)
                    .HasColumnName("EmailID")
                    .IsUnicode(false);

                entity.Property(e => e.MobileNumber).IsUnicode(false);

                entity.Property(e => e.OfficeNumber).IsUnicode(false);

                entity.Property(e => e.PinCode).IsUnicode(false);

                entity.Property(e => e.RefTableId)
                    .HasColumnName("RefTableID")
                    .HasMaxLength(500);

                entity.Property(e => e.RefTableName)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.ResidenceNumber).IsUnicode(false);

                entity.Property(e => e.State).IsUnicode(false);
            });

            modelBuilder.Entity<Doctor>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.BestDayToMeet).IsUnicode(false);

                entity.Property(e => e.BestTimeToMeet).IsUnicode(false);

                entity.Property(e => e.Brand).IsUnicode(false);

                entity.Property(e => e.Class).IsUnicode(false);

                entity.Property(e => e.Gender)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Name).IsUnicode(false);

                entity.Property(e => e.Qualification).IsUnicode(false);

                entity.Property(e => e.RegistrationNo).IsUnicode(false);

                entity.Property(e => e.Speciality).IsUnicode(false);

                entity.Property(e => e.VisitFrequency).IsUnicode(false);

                entity.Property(e => e.VisitPlan).IsUnicode(false);
            });

            modelBuilder.Entity<Expenses>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ApprovedBy)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.ApproverRemark)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.EmployeeRemark)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.ExpenseMonth)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Hq).HasColumnName("HQ");

                entity.Property(e => e.PresentType)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Region).IsUnicode(false);

                entity.Property(e => e.UserName)
                    .HasMaxLength(500)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ExpensesStatus>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ExpenseMonth)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserName)
                    .HasMaxLength(500)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<HolidayList>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.FestivalDate).HasColumnType("date");

                entity.Property(e => e.FestivalDay)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FestivalDescription).IsUnicode(false);

                entity.Property(e => e.FestivalName).IsUnicode(false);
            });

            modelBuilder.Entity<Hqregion>(entity =>
            {
                entity.ToTable("HQRegion");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Hqid).HasColumnName("HQID");

                entity.Property(e => e.RegionId).HasColumnName("RegionID");

                entity.Property(e => e.UserId).HasColumnName("UserID");
            });

            modelBuilder.Entity<MasterKeyValue>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Type).IsUnicode(false);

                entity.Property(e => e.Value).IsUnicode(false);
            });

            modelBuilder.Entity<Patient>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.Gender).IsUnicode(false);

                entity.Property(e => e.PatientName).IsUnicode(false);

                entity.Property(e => e.Type).IsUnicode(false);
            });

            modelBuilder.Entity<Smslogger>(entity =>
            {
                entity.ToTable("SMSLogger");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ErrorCode)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ErrorMessage)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.JobId)
                    .HasColumnName("JobID")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.MessageData).IsUnicode(false);

                entity.Property(e => e.MobileNumber)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Occasion).HasMaxLength(50);

                entity.Property(e => e.SendSmsdate)
                    .HasColumnName("SendSMSDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.SendSmsto)
                    .HasColumnName("SendSMSTo")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Smstext)
                    .HasColumnName("SMSText")
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Stockist>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.StockistName).IsUnicode(false);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
