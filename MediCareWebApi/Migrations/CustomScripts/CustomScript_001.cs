using MediCare.Enums;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MediCare.Migrations.CustomScripts
{
	public class CustomScript_001
	{
		public static void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.InsertData(
				table: "Roles",
				columns: new[] { "Id", "RoleType" },
				values: new object[,]
				{
					{ 1, (int)RoleType.Patient },
					{ 2, (int)RoleType.Doctor },
					{ 3, (int)RoleType.Admin }
				});

			migrationBuilder.InsertData(
				table: "Permissions",
				columns: new[] { "Id", "Description"},
				values: new object[,]
				{
					{1, "Schedule appointment" },
					{2, "View own appointments" },
					{3, "View all appointments" },
					{4, "Cancel appointment" },
					{5, "Accept appointment" },
					{6, "Confirm attendance" },
					{7, "Choose doctors" },
					{8, "Select service" },
					{9, "Record diagnosis" },
					{10, "Issue prescription" },
					{11, "Manage user accounts" },
					{12, "Assign roles" },
					{13, "Generate reports" },
					{14, "Manage clinic services" },
					{15, "View system logs" },
					{16, "Manage permissions" }
				});

			migrationBuilder.InsertData(
				table: "RolePermissions",
				columns: new [] { "Id", "RoleId", "PermissionId" },
				values: new object[,]
				{
					{ 1, 1, 1 },
					{ 2, 1, 2 },
					{ 3, 1, 4 },
					{ 4, 1, 6 },
					{ 5, 1, 7 },
					{ 6, 2, 2 },
					{ 7, 2, 4 },
					{ 8, 2, 5 },
					{ 9, 2, 6 },
					{ 10, 2, 9 },
					{ 11, 2, 10 },
					{ 12, 3, 3 },
					{ 13, 3, 4 },
					{ 14, 3, 11 },
					{ 15, 3, 12 },
					{ 16, 3, 13 },
					{ 17, 3, 14 },
					{ 18, 3, 15 },
					{ 19, 3, 16 },
				});

			migrationBuilder.InsertData(
				table: "Specialities",
				columns: new[] { "Id", "Name", "Description" },
				values: new object[,]
				{
					{ 1, "Cardiology", "Specialty related to the treatment of heart and circulatory system diseases." },
					{ 2, "Neurology", "Specialty that deals with diagnosing and treating diseases of the nervous system." },
					{ 3, "Orthopedics", "Specialty that focuses on treating musculoskeletal disorders." },
					{ 4, "Pediatrics", "Specialty focused on the treatment of children." },
					{ 5, "Gastroenterology", "Specialty focused on diagnosing and treating diseases of the digestive system." },
					{ 6, "Dermatology", "Specialty focused on diagnosing and treating skin diseases." },
					{ 7, "Psychiatry", "Specialty that deals with diagnosing and treating mental health disorders." },
					{ 8, "Gynecology", "Specialty that focuses on women's health, including the diagnosis and treatment of reproductive system diseases." },
					{ 9, "Endocrinology", "Specialty focused on diagnosing and treating hormonal disorders." },
					{ 10, "Oncology", "Specialty focused on diagnosing and treating cancer." },
					{ 11, "Family Medicine", "Specialty that provides primary healthcare services and treats a wide range of conditions for patients of all ages." }
				});

			migrationBuilder.InsertData(
				table: "Medicaments",
				columns: new[] { "Id", "Name", "Description", "MedicamentType", "PrescriptionRequired" },
				values: new object[,]
				{
					{ 1, "Aspirin", "Pain reliever, anti-inflammatory, and antipyretic effects.", (int)MedicamentType.Analgesic, true },
					{ 2, "Amoxicillin", "Antibiotic used for bacterial infections.", (int)MedicamentType.Antibiotic, true },
					{ 3, "Ibuprofen", "NSAID for pain relief, inflammation, and fever reduction.", (int)MedicamentType.AntiInflammatory, true },
					{ 4, "Oseltamivir", "Antiviral for influenza treatment.", (int)MedicamentType.Antiviral, true },
					{ 5, "Clotrimazole", "Antifungal for skin and mucous membrane infections.", (int)MedicamentType.Antifungal, true },
					{ 6, "Diphenhydramine", "Antihistamine for allergy relief and sleep aid.", (int)MedicamentType.Antihistamine, true },
					{ 7, "Paracetamol", "Common pain reliever and fever reducer.", (int)MedicamentType.Antipyretic, false },
					{ 8, "Vitamin C", "Vitamin supplement used to boost immunity.", (int)MedicamentType.VitaminsAndSupplements, false },
					{ 9, "Lorazepam", "Sedative for anxiety relief and sleep disorders.", (int)MedicamentType.Sedative, true },
					{ 10, "Dextromethorphan", "Cough suppressant for dry cough relief.", (int)MedicamentType.CoughSuppressant, false },
					{ 11, "Lisinopril", "Antihypertensive for blood pressure management.", (int)MedicamentType.Hypertension, true },
					{ 12, "Metformin", "Antidiabetic medication for type 2 diabetes.", (int)MedicamentType.Antidiabetic, true },
					{ 13, "Estradiol", "Hormonal therapy for estrogen replacement.", (int)MedicamentType.Hormonal, true },
					{ 14, "Cyclosporine", "Immunosuppressant to prevent organ rejection.", (int)MedicamentType.Immunosuppressant, true },
					{ 15, "Omeprazole", "Antacid for acid reflux and GERD treatment.", (int)MedicamentType.Antacid, false },
					{ 16, "Tobramycin", "Antibiotic used for respiratory tract infections.", (int)MedicamentType.Antibiotic, true },
					{ 17, "Timolol", "Ophthalmic beta-blocker for glaucoma treatment.", (int)MedicamentType.Ophthalmic, true },
					{ 18, "Hydrocortisone", "Anti-inflammatory corticosteroid for skin and joint inflammation.", (int)MedicamentType.AntiInflammatory, true },
					{ 19, "Nystatin", "Antifungal for oral and vaginal infections.", (int)MedicamentType.Antifungal, true },
					{ 20, "Fexofenadine", "Non-drowsy antihistamine for seasonal allergies.", (int)MedicamentType.Antihistamine, false },
					{ 21, "Melatonin", "Sleep aid and hormone supplement.", (int)MedicamentType.Sedative, false },
					{ 22, "Warfarin", "Antiplatelet medication to prevent blood clotting.", (int)MedicamentType.Antiplatelet, true },
					{ 23, "Calcium", "Mineral supplement for bone health.", (int)MedicamentType.MineralSupplement, false },
					{ 24, "Hyoscyamine", "Antispasmodic for gastrointestinal disorders.", (int)MedicamentType.Antispasmodic, true },
					{ 25, "Cephalexin", "Antibiotic for bacterial skin and urinary infections.", (int)MedicamentType.Antibiotic, true },
					{ 26, "Simvastatin", "Statin used to lower cholesterol and reduce heart disease risk.", (int)MedicamentType.Hypertension, true },
					{ 27, "Prednisone", "Corticosteroid for inflammation and immune disorders.", (int)MedicamentType.AntiInflammatory, true },
					{ 28, "Baclofen", "Muscle relaxant for spasticity and muscle spasms.", (int)MedicamentType.Antispasmodic, true },
					{ 29, "Pantoprazole", "Proton pump inhibitor for acid reflux and gastric ulcers.", (int)MedicamentType.Antacid, true },
					{ 30, "Dabigatran", "Antiplatelet medication to prevent blood clots in atrial fibrillation.", (int)MedicamentType.Antiplatelet, true }
				});

			migrationBuilder.InsertData(
				table: "Services",
				columns: new[] { "Id", "Name", "Description", "Price", "DurationMinutes", "SpecialityId" },
				values: new object[,]
				{
					{ 1, "Heart Checkup", "Comprehensive evaluation of heart health.", 200.00m, 30, 1 },
					{ 2, "Neurological Consultation", "Assessment of nervous system disorders.", 250.00m, 45, 2 },
					{ 3, "Bone Fracture Treatment", "Diagnosis and treatment of bone fractures.", 300.00m, 60, 3 },
					{ 4, "Pediatric General Checkup", "Routine health checkup for children.", 100.00m, 30, 4 },
					{ 5, "Digestive System Analysis", "Comprehensive analysis of digestive health.", 180.00m, 45, 5 },
					{ 6, "Skin Condition Diagnosis", "Evaluation of skin issues and conditions.", 120.00m, 30, 6 },
					{ 7, "Mental Health Therapy", "Therapeutic session for mental well-being.", 150.00m, 60, 7 },
					{ 8, "Prenatal Checkup", "Regular checkup for pregnant women.", 130.00m, 30, 8 },
					{ 9, "Hormonal Therapy Session", "Management of hormonal imbalances.", 160.00m, 45, 9 },
					{ 10, "Cancer Screening", "Early detection and diagnosis of cancer.", 400.00m, 60, 10 },
					{ 11, "ECG Test", "Electrocardiogram to monitor heart activity.", 80.00m, 15, 1 },
					{ 12, "EEG Test", "Electroencephalogram for brain activity analysis.", 150.00m, 30, 2 },
					{ 13, "Physical Therapy Session", "Therapy for musculoskeletal rehabilitation.", 200.00m, 45, 3 },
					{ 14, "Child Vaccination", "Vaccination for common childhood diseases.", 50.00m, 15, 4 },
					{ 15, "Endoscopy", "Examination of the digestive tract.", 500.00m, 60, 5 },
					{ 16, "Acne Treatment", "Comprehensive treatment for acne.", 100.00m, 30, 6 },
					{ 17, "Psychiatric Evaluation", "Initial evaluation for mental health issues.", 120.00m, 30, 7 },
					{ 18, "Gynecological Consultation", "Women's health consultation.", 140.00m, 30, 8 },
					{ 19, "Diabetes Management", "Treatment plan for diabetes patients.", 150.00m, 30, 9 },
					{ 20, "Chemotherapy Session", "Cancer treatment with chemotherapy.", 1000.00m, 90, 10 },
					{ 21, "Family Medicine", "Appointment with a family doctor.", 50.00m, 15, 11 }
				});

			migrationBuilder.InsertData(
				table: "Users",
				columns: new[] { "Id", "Email", "Password", "Salt", "Name", "Surname", "Pesel", "PhoneNumber", "RoleId" },
				values: new object[,]
				{
					{ 1, "s26028@pjwstk.edu.pl", "jZs/vfkieZcdBngxPAHzXuEDi5XZg0tOXXdtUooa1ag=", "mZ5bf60ttVt+4Xx6FHpvFHx+Vx/pPUoYql9QO+G9t3Y=", "Bohdan", "Sternytskyi", "00210816473", "+123456789", 3 },
					{ 2, "john.smith@outlook.com", "jZs/vfkieZcdBngxPAHzXuEDi5XZg0tOXXdtUooa1ag=", "mZ5bf60ttVt+4Xx6FHpvFHx+Vx/pPUoYql9QO+G9t3Y=", "John", "Smith", "12345678901", "+48123456789", 2 },
					{ 3, "emily.brown@outlook.com", "jZs/vfkieZcdBngxPAHzXuEDi5XZg0tOXXdtUooa1ag=", "mZ5bf60ttVt+4Xx6FHpvFHx+Vx/pPUoYql9QO+G9t3Y=", "Emily", "Brown", "98765432109", "+48111222333", 2 },
					{ 4, "michael.green@outlook.com", "jZs/vfkieZcdBngxPAHzXuEDi5XZg0tOXXdtUooa1ag=", "mZ5bf60ttVt+4Xx6FHpvFHx+Vx/pPUoYql9QO+G9t3Y=", "Michael", "Green", "56789012345", "+48765432101", 2 },
					{ 5, "sophia.wilson@outlook.com", "jZs/vfkieZcdBngxPAHzXuEDi5XZg0tOXXdtUooa1ag=", "mZ5bf60ttVt+4Xx6FHpvFHx+Vx/pPUoYql9QO+G9t3Y=", "Sophia", "Wilson", "12309876543", "+48234567890", 2 },
					{ 6, "james.taylor@outlook.com", "jZs/vfkieZcdBngxPAHzXuEDi5XZg0tOXXdtUooa1ag=", "mZ5bf60ttVt+4Xx6FHpvFHx+Vx/pPUoYql9QO+G9t3Y=", "James", "Taylor", "90123456789", "+48987654321", 2 },
					{ 7, "olivia.miller@outlook.com", "jZs/vfkieZcdBngxPAHzXuEDi5XZg0tOXXdtUooa1ag=", "mZ5bf60ttVt+4Xx6FHpvFHx+Vx/pPUoYql9QO+G9t3Y=", "Olivia", "Miller", "65432198700", "+48555222334", 2 },
					{ 8, "william.moore@outlook.com", "jZs/vfkieZcdBngxPAHzXuEDi5XZg0tOXXdtUooa1ag=", "mZ5bf60ttVt+4Xx6FHpvFHx+Vx/pPUoYql9QO+G9t3Y=", "William", "Moore", "34567290123", "+48770011223", 2 },
					{ 9, "ava.johnson@outlook.com", "jZs/vfkieZcdBngxPAHzXuEDi5XZg0tOXXdtUooa1ag=", "mZ5bf60ttVt+4Xx6FHpvFHx+Vx/pPUoYql9QO+G9t3Y=", "Ava", "Johnson", "12309865789", "+48660123456", 2 },
					{ 10, "logan.white@outlook.com", "jZs/vfkieZcdBngxPAHzXuEDi5XZg0tOXXdtUooa1ag=", "mZ5bf60ttVt+4Xx6FHpvFHx+Vx/pPUoYql9QO+G9t3Y=", "Logan", "White", "89012345678", "+48770033445", 2 },
					{ 11, "isabella.anderson@outlook.com", "jZs/vfkieZcdBngxPAHzXuEDi5XZg0tOXXdtUooa1ag=", "mZ5bf60ttVt+4Xx6FHpvFHx+Vx/pPUoYql9QO+G9t3Y=", "Isabella", "Anderson", "67890123456", "+48555444678", 2 },
					{ 12, "lucas.king@outlook.com", "jZs/vfkieZcdBngxPAHzXuEDi5XZg0tOXXdtUooa1ag=", "mZ5bf60ttVt+4Xx6FHpvFHx+Vx/pPUoYql9QO+G9t3Y=", "Lucas", "King", "45678907234", "+48990011223", 2 },
					{ 13, "mia.scott@outlook.com", "jZs/vfkieZcdBngxPAHzXuEDi5XZg0tOXXdtUooa1ag=", "mZ5bf60ttVt+4Xx6FHpvFHx+Vx/pPUoYql9QO+G9t3Y=", "Mia", "Scott", "23456783012", "+48123499900", 2 },
					{ 14, "ethan.clark@outlook.com", "jZs/vfkieZcdBngxPAHzXuEDi5XZg0tOXXdtUooa1ag=", "mZ5bf60ttVt+4Xx6FHpvFHx+Vx/pPUoYql9QO+G9t3Y=", "Ethan", "Clark", "79901234567", "+48765543322", 2 },
					{ 15, "emma.lewis@outlook.com", "jZs/vfkieZcdBngxPAHzXuEDi5XZg0tOXXdtUooa1ag=", "mZ5bf60ttVt+4Xx6FHpvFHx+Vx/pPUoYql9QO+G9t3Y=", "Emma", "Lewis", "56789012340", "+48660077888", 2 },
					{ 16, "alexander.young@outlook.com", "jZs/vfkieZcdBngxPAHzXuEDi5XZg0tOXXdtUooa1ag=", "mZ5bf60ttVt+4Xx6FHpvFHx+Vx/pPUoYql9QO+G9t3Y=", "Alexander", "Young", "89012346789", "+48778999001", 2 },
					{ 17, "sophie.wright@outlook.com", "jZs/vfkieZcdBngxPAHzXuEDi5XZg0tOXXdtUooa1ag=", "mZ5bf60ttVt+4Xx6FHpvFHx+Vx/pPUoYql9QO+G9t3Y=", "Sophie", "Wright", "45612378900", "+48550123456", 2 },
					{ 18, "benjamin.hall@outlook.com", "jZs/vfkieZcdBngxPAHzXuEDi5XZg0tOXXdtUooa1ag=", "mZ5bf60ttVt+4Xx6FHpvFHx+Vx/pPUoYql9QO+G9t3Y=", "Benjamin", "Hall", "12345678090", "+48660112233", 2 },
					{ 19, "lily.harris@outlook.com", "jZs/vfkieZcdBngxPAHzXuEDi5XZg0tOXXdtUooa1ag=", "mZ5bf60ttVt+4Xx6FHpvFHx+Vx/pPUoYql9QO+G9t3Y=", "Lily", "Harris", "78945612345", "+48559988444", 2 },
					{ 20, "noah.robinson@outlook.com", "jZs/vfkieZcdBngxPAHzXuEDi5XZg0tOXXdtUooa1ag=", "mZ5bf60ttVt+4Xx6FHpvFHx+Vx/pPUoYql9QO+G9t3Y=", "Noah", "Robinson", "65432198765", "+48990044556", 2 },
					{ 21, "charlotte.walker@outlook.com", "jZs/vfkieZcdBngxPAHzXuEDi5XZg0tOXXdtUooa1ag=", "mZ5bf60ttVt+4Xx6FHpvFHx+Vx/pPUoYql9QO+G9t3Y=", "Charlotte", "Walker", "12389045678", "+48771122334", 2 },
					{ 22, "daniel.thompson@outlook.com", "jZs/vfkieZcdBngxPAHzXuEDi5XZg0tOXXdtUooa1ag=", "mZ5bf60ttVt+4Xx6FHpvFHx+Vx/pPUoYql9QO+G9t3Y=", "Daniel", "Thompson", "98765432111", "+48667899811", 2 },
					{ 23, "john.doe@outlook.com", "jZs/vfkieZcdBngxPAHzXuEDi5XZg0tOXXdtUooa1ag=", "mZ5bf60ttVt+4Xx6FHpvFHx+Vx/pPUoYql9QO+G9t3Y=", "John", "Doe", "12345678912", "+48123456790", 2 },
					{ 24, "mary.jones@outlook.com", "jZs/vfkieZcdBngxPAHzXuEDi5XZg0tOXXdtUooa1ag=", "mZ5bf60ttVt+4Xx6FHpvFHx+Vx/pPUoYql9QO+G9t3Y=", "Mary", "Jones", "23456789012", "+48234567891", 2 },
					{ 25, "james.brown@outlook.com", "jZs/vfkieZcdBngxPAHzXuEDi5XZg0tOXXdtUooa1ag=", "mZ5bf60ttVt+4Xx6FHpvFHx+Vx/pPUoYql9QO+G9t3Y=", "James", "Brown", "34567890163", "+48345678902", 2 },
					{ 26, "patricia.smith@outlook.com", "jZs/vfkieZcdBngxPAHzXuEDi5XZg0tOXXdtUooa1ag=", "mZ5bf60ttVt+4Xx6FHpvFHx+Vx/pPUoYql9QO+G9t3Y=", "Patricia", "Smith", "45678901234", "+48456789013", 2 },
					{ 27, "robert.johnson@outlook.com", "jZs/vfkieZcdBngxPAHzXuEDi5XZg0tOXXdtUooa1ag=", "mZ5bf60ttVt+4Xx6FHpvFHx+Vx/pPUoYql9QO+G9t3Y=", "Robert", "Johnson", "56789012245", "+48567890124", 2 },
					{ 28, "linda.miller@outlook.com", "jZs/vfkieZcdBngxPAHzXuEDi5XZg0tOXXdtUooa1ag=", "mZ5bf60ttVt+4Xx6FHpvFHx+Vx/pPUoYql9QO+G9t3Y=", "Linda", "Miller", "67890193456", "+48678901235", 2 },
					{ 29, "michael.garcia@outlook.com", "jZs/vfkieZcdBngxPAHzXuEDi5XZg0tOXXdtUooa1ag=", "mZ5bf60ttVt+4Xx6FHpvFHx+Vx/pPUoYql9QO+G9t3Y=", "Michael", "Garcia", "78901234567", "+48789012346", 2 },
					{ 30, "elizabeth.rodriguez@outlook.com", "jZs/vfkieZcdBngxPAHzXuEDi5XZg0tOXXdtUooa1ag=", "mZ5bf60ttVt+4Xx6FHpvFHx+Vx/pPUoYql9QO+G9t3Y=", "Elizabeth", "Rodriguez", "89012325678", "+48890123457", 2 },
					{ 31, "william.davis@outlook.com", "jZs/vfkieZcdBngxPAHzXuEDi5XZg0tOXXdtUooa1ag=", "mZ5bf60ttVt+4Xx6FHpvFHx+Vx/pPUoYql9QO+G9t3Y=", "William", "Davis", "90128456789", "+48901234568", 2 },
					{ 32, "susan.martinez@outlook.com", "jZs/vfkieZcdBngxPAHzXuEDi5XZg0tOXXdtUooa1ag=", "mZ5bf60ttVt+4Xx6FHpvFHx+Vx/pPUoYql9QO+G9t3Y=", "Susan", "Martinez", "12345678921", "+48012345679", 2 },
					{ 33, "charles.hernandez@outlook.com", "jZs/vfkieZcdBngxPAHzXuEDi5XZg0tOXXdtUooa1ag=", "mZ5bf60ttVt+4Xx6FHpvFHx+Vx/pPUoYql9QO+G9t3Y=", "Charles", "Hernandez", "23456789022", "+48123456780", 2 },
					{ 34, "joseph.lopez@outlook.com", "jZs/vfkieZcdBngxPAHzXuEDi5XZg0tOXXdtUooa1ag=", "mZ5bf60ttVt+4Xx6FHpvFHx+Vx/pPUoYql9QO+G9t3Y=", "Joseph", "Lopez", "33567890123", "+48234567881", 2 }
				});

			migrationBuilder.InsertData(
				table: "Admins",
				columns: new[] { "Id", "LastLogin", "Status", "UserId" },
				values: new object[]
				{ 1, DateTime.Now, (int)AdminStatus.Active, 1 });

			migrationBuilder.InsertData(
				table: "Doctors",
				columns: new[] { "Id", "EmploymentDate", "IsAvailable", "SpecialityId", "UserId" },
				values: new object[,]
				{
					{ 1, new DateTime(2023, 7, 15), true, 1, 2 },
					{ 2, new DateTime(2024, 3, 22), true, 2, 3 },
					{ 3, new DateTime(2022, 5, 10), true, 3, 4 },
					{ 4, new DateTime(2021, 8, 25), true, 4, 5 },
					{ 5, new DateTime(2023, 12, 30), true, 5, 6 },
					{ 6, new DateTime(2020, 2, 17), true, 6, 7 },
					{ 7, new DateTime(2024, 1, 9), true, 7, 8 },
					{ 8, new DateTime(2023, 6, 1), true, 8, 9 },
					{ 9, new DateTime(2022, 9, 12), true, 9, 10 },
					{ 10, new DateTime(2021, 11, 5), true, 10, 11 },
					{ 11, new DateTime(2023, 4, 19), true, 11, 12 },
					{ 12, new DateTime(2020, 7, 22), true, 1, 13 },
					{ 13, new DateTime(2022, 3, 15), true, 2, 14 },
					{ 14, new DateTime(2024, 5, 6), true, 3, 15 },
					{ 15, new DateTime(2021, 10, 11), true, 4, 16 },
					{ 16, new DateTime(2023, 8, 30), true, 5, 17 },
					{ 17, new DateTime(2022, 12, 18), true, 6, 18 },
					{ 18, new DateTime(2020, 4, 14), true, 7, 19 },
					{ 19, new DateTime(2024, 2, 27), true, 8, 20 },
					{ 20, new DateTime(2021, 9, 9), true, 9, 21 },
					{ 21, new DateTime(2023, 1, 20), true, 10, 22 },
					{ 22, new DateTime(2024, 3, 5), true, 11, 23 },
					{ 23, new DateTime(2023, 5, 17), true, 1, 24 },
					{ 24, new DateTime(2022, 11, 9), true, 2, 25 },
					{ 25, new DateTime(2023, 7, 10), true, 3, 26 },
					{ 26, new DateTime(2021, 10, 25), true, 4, 27 },
					{ 27, new DateTime(2024, 1, 8), true, 5, 28 },
					{ 28, new DateTime(2023, 2, 13), true, 6, 29 },
					{ 29, new DateTime(2021, 6, 18), true, 7, 30 },
					{ 30, new DateTime(2022, 9, 3), true, 8, 31 },
					{ 31, new DateTime(2024, 4, 14), true, 9, 32 },
					{ 32, new DateTime(2021, 5, 7), true, 10, 33 },
					{ 33, new DateTime(2022, 12, 23), true, 11, 34 }
				});
		}

		public static void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DeleteData(
				table: "Roles",
				keyColumn: "Id",
				keyValues: new object[] { 1, 2, 3 });

			migrationBuilder.DeleteData(
				table: "Permissions",
				keyColumn: "Id",
				keyValues: new object[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 });

			migrationBuilder.DeleteData(
				table: "RolePermissions",
				keyColumn: "Id",
				keyValues: new object[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19 });

			migrationBuilder.DeleteData(
				table: "Specialities",
				keyColumn: "Id",
				keyValues: new object[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 });

			migrationBuilder.DeleteData(
				table: "Medicaments",
				keyColumn: "Id",
				keyValues: new object[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30 });

			migrationBuilder.DeleteData(
				table: "Services",
				keyColumn: "Id",
				keyValues: new object[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21 });

			migrationBuilder.DeleteData(
				table: "Users",
				keyColumn: "Id",
				keyValues: new object[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34 });

			migrationBuilder.DeleteData(
				table: "Admins",
				keyColumn: "Id",
				keyValues: new object[] { 1 });

			migrationBuilder.DeleteData(
				table: "Doctors",
				keyColumn: "Id",
				keyValues: new object[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33 });
		}
	}
}
