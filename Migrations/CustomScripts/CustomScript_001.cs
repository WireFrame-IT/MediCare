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
					{ 10, "Oncology", "Specialty focused on diagnosing and treating cancer." }
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
		}

		public static void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DeleteData(
				table: "RolePermissions",
				keyColumn: "Id",
				keyValues: new object[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19 });

			migrationBuilder.DeleteData(
				table: "Roles",
				keyColumn: "Id",
				keyValues: new object[] { 1, 2, 3 });

			migrationBuilder.DeleteData(
				table: "Permissions",
				keyColumn: "Id",
				keyValues: new object[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 });

			migrationBuilder.DeleteData(
				table: "Specialities",
				keyColumn: "Id",
				keyValues: new object[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });

			migrationBuilder.DeleteData(
				table: "Medicaments",
				keyColumn: "Id",
				keyValues: new object[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30 });
		}
	}
}
