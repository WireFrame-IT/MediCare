using Microsoft.EntityFrameworkCore;

namespace MediCare.Models
{
	public class MediCareDbContext : DbContext
	{
		private readonly IConfiguration _configuration;
		public MediCareDbContext(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public MediCareDbContext(DbContextOptions<MediCareDbContext> options, IConfiguration configuration) : base(options)
		{
			_configuration = configuration;
		}

		public DbSet<User> Users { get; set; }
		public DbSet<Admin> Admins { get; set; }
		public DbSet<Doctor> Doctors { get; set; }
		public DbSet<Patient> Patients { get; set; }
		public DbSet<Log> Logs { get; set; }
		public DbSet<Role> Roles { get; set; }
		public DbSet<Permission> Permissions { get; set; }
		public DbSet<Report> Reports { get; set; }
		public DbSet<Speciality> Specialities { get; set; }
		public DbSet<Feedback> Feedbacks { get; set; }
		public DbSet<Appointment> Appointments { get; set; }
		public DbSet<Service> Services { get; set; }
		public DbSet<Prescription> Prescriptions { get; set; }
		public DbSet<Medicament> Medicaments { get; set; }
		public DbSet<PrescriptionMedicament> PrescriptionMedicaments { get; set; }
		public DbSet<RolePermission> RolePermissions { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Service>()
				.Property(x => x.Price)
				.HasPrecision(18, 2);

			modelBuilder.Entity<Appointment>()
				.HasOne(x => x.Doctor)
				.WithMany(x => x.Appointments)
				.HasForeignKey(x => x.DoctorId)
				.OnDelete(DeleteBehavior.NoAction);

			modelBuilder.Entity<Appointment>()
				.HasOne(x => x.Patient)
				.WithMany(x => x.Appointments)
				.HasForeignKey(x => x.PatientId)
				.OnDelete(DeleteBehavior.NoAction);

			modelBuilder.Entity<RolePermission>()
				.HasOne(x => x.Role)
				.WithMany(x => x.RolePermissions)
				.HasForeignKey(x => x.RoleId);

			modelBuilder.Entity<RolePermission>()
				.HasOne(x => x.Permission)
				.WithMany(x => x.PermissionRoles)
				.HasForeignKey(x => x.PermissionId);

			modelBuilder.Entity<PrescriptionMedicament>()
				.HasOne(x => x.Medicament)
				.WithMany(x => x.MedicamentPrescriptions)
				.HasForeignKey(x => x.MedicamentId);

			modelBuilder.Entity<PrescriptionMedicament>()
				.HasOne(x => x.Prescription)
				.WithMany(x => x.PrescriptionMedicaments)
				.HasForeignKey(x => x.PrescriptionId);
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			base.OnConfiguring(optionsBuilder);
			optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));
		}
	}
}
