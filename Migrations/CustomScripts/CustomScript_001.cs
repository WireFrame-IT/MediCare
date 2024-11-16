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
					{ 1, RoleType.Patient },
					{ 2, RoleType.Doctor },
					{ 3, RoleType.Admin }
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
					{ 10, "Oncology", "Specialty focused on diagnosing and treating cancer." }
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
					{5, "Confirm appointment" },
					{6, "Choose doctors" },
					{7, "Select service" },
					{8, "Record diagnosis" },
					{9, "Issue prescription" },
					{10, "Manage user accounts" },
					{11, "Assign roles" },
					{12, "Generate reports" },
					{13, "Manage clinic services" },
					{14, "View system logs" },
					{15, "Manage permissions" }
				});

			migrationBuilder.InsertData(
				table: "Medicaments",
				columns: new[] { "Id", "Name", "Description", "MedicamentType", "PrescriptionRequired" },
				values: new object[,]
				{
					{ 1, "Aspirin", "Pain reliever, anti-inflammatory, and antipyretic effects.", MedicamentType.Analgesic, true },
					{ 2, "Amoxicillin", "Antibiotic used for bacterial infections.", MedicamentType.Antibiotic, true },
					{ 3, "Ibuprofen", "NSAID for pain relief, inflammation, and fever reduction.", MedicamentType.AntiInflammatory, true },
					{ 4, "Oseltamivir", "Antiviral for influenza treatment.", MedicamentType.Antiviral, true },
					{ 5, "Clotrimazole", "Antifungal for skin and mucous membrane infections.", MedicamentType.Antifungal, true },
					{ 6, "Diphenhydramine", "Antihistamine for allergy relief and sleep aid.", MedicamentType.Antihistamine, true },
					{ 7, "Paracetamol", "Common pain reliever and fever reducer.", MedicamentType.Antipyretic, false },
					{ 8, "Vitamin C", "Vitamin supplement used to boost immunity.", MedicamentType.VitaminsAndSupplements, false },
					{ 9, "Lorazepam", "Sedative for anxiety relief and sleep disorders.", MedicamentType.Sedative, true },
					{ 10, "Dextromethorphan", "Cough suppressant for dry cough relief.", MedicamentType.CoughSuppressant, false },
					{ 11, "Lisinopril", "Antihypertensive for blood pressure management.", MedicamentType.Hypertension, true },
					{ 12, "Metformin", "Antidiabetic medication for type 2 diabetes.", MedicamentType.Antidiabetic, true },
					{ 13, "Estradiol", "Hormonal therapy for estrogen replacement.", MedicamentType.Hormonal, true },
					{ 14, "Cyclosporine", "Immunosuppressant to prevent organ rejection.", MedicamentType.Immunosuppressant, true },
					{ 15, "Omeprazole", "Antacid for acid reflux and GERD treatment.", MedicamentType.Antacid, false },
					{ 16, "Tobramycin", "Antibiotic used for respiratory tract infections.", MedicamentType.Antibiotic, true },
					{ 17, "Timolol", "Ophthalmic beta-blocker for glaucoma treatment.", MedicamentType.Ophthalmic, true },
					{ 18, "Hydrocortisone", "Anti-inflammatory corticosteroid for skin and joint inflammation.", MedicamentType.AntiInflammatory, true },
					{ 19, "Nystatin", "Antifungal for oral and vaginal infections.", MedicamentType.Antifungal, true },
					{ 20, "Fexofenadine", "Non-drowsy antihistamine for seasonal allergies.", MedicamentType.Antihistamine, false },
					{ 21, "Melatonin", "Sleep aid and hormone supplement.", MedicamentType.Sedative, false },
					{ 22, "Warfarin", "Antiplatelet medication to prevent blood clotting.", MedicamentType.Antiplatelet, true },
					{ 23, "Calcium", "Mineral supplement for bone health.", MedicamentType.MineralSupplement, false },
					{ 24, "Hyoscyamine", "Antispasmodic for gastrointestinal disorders.", MedicamentType.Antispasmodic, true },
					{ 25, "Cephalexin", "Antibiotic for bacterial skin and urinary infections.", MedicamentType.Antibiotic, true },
					{ 26, "Simvastatin", "Statin used to lower cholesterol and reduce heart disease risk.", MedicamentType.Hypertension, true },
					{ 27, "Prednisone", "Corticosteroid for inflammation and immune disorders.", MedicamentType.AntiInflammatory, true },
					{ 28, "Baclofen", "Muscle relaxant for spasticity and muscle spasms.", MedicamentType.Antispasmodic, true },
					{ 29, "Pantoprazole", "Proton pump inhibitor for acid reflux and gastric ulcers.", MedicamentType.Antacid, true },
					{ 30, "Dabigatran", "Antiplatelet medication to prevent blood clots in atrial fibrillation.", MedicamentType.Antiplatelet, true }
				});
		}

		public static void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DeleteData(
				table: "Roles",
				keyColumn: "Id",
				keyValues: new object[] { 1, 2, 3});

			migrationBuilder.DeleteData(
				table: "Specialities",
				keyColumn: "Id",
				keyValues: new object[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });

			migrationBuilder.DeleteData(
				table: "Permissions",
				keyColumn: "Id",
				keyValues: new object[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 });

			migrationBuilder.DeleteData(
				table: "Medicaments",
				keyColumn: "Id",
				keyValues: new object[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30 });
		}
	}
}
