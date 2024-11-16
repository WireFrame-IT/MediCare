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
		public DbSet<Medicament> Medications { get; set; }
		public DbSet<PrescriptionMedicament> PrescriptionMedicaments { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			base.OnConfiguring(optionsBuilder);
			optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));
		}
	}
}
