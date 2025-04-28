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
				columns: new[] { "Id", "Description", "DoctorOnly", "PatientOnly", "PermissionType"},
				values: new object[,]
				{
					{1, "View all appointments", true, false, (int)PermissionType.ViewAllAppointments },
					{2, "Cancel appointment", false, false, (int)PermissionType.CancelAppointment },
					{3, "Choose a doctor", false, true, (int)PermissionType.ChooseDoctor },
					{4, "Generate reports", true, false, (int)PermissionType.GenerateReports },
					{5, "Manage services", true, false, (int)PermissionType.ManageServices }
				});

			migrationBuilder.InsertData(
				table: "RolePermissions",
				columns: new [] { "RoleId", "PermissionId" },
				values: new object[,]
				{
					{ 1, 2 },
					{ 1, 3 },
					{ 2, 1 },
					{ 2, 2 },
					{ 2, 4 },
					{ 2, 5 }
				});

			migrationBuilder.InsertData(
				table: "Specialities",
				columns: new[] { "Id", "Name", "Description" },
				values: new object[,]
				{
					{ 1, "Family Medicine", "Comprehensive primary care for patients of all ages." },
					{ 2, "Cardiology", "Diagnosis and treatment of heart and circulatory system conditions." },
					{ 3, "Neurology", "Care for disorders of the brain, spine, and nervous system." },
					{ 4, "Orthopedics", "Treatment of bones, joints, muscles, and related injuries." },
					{ 5, "Pediatrics", "Medical care for infants, children, and adolescents." },
					{ 6, "Gastroenterology", "Diagnosis and treatment of digestive system disorders." },
					{ 7, "Dermatology", "Care for skin, hair, and nail conditions." },
					{ 8, "Psychiatry", "Diagnosis and management of mental health disorders." },
					{ 9, "Gynecology", "Women's reproductive health, including diagnostics and treatment." },
					{ 10, "Endocrinology", "Treatment of hormonal and metabolic disorders." },
					{ 11, "Oncology", "Diagnosis and care for patients with cancer." },
					{ 12, "Laboratory Medicine", "Diagnostic testing and analysis of blood, tissue, and other samples."},
					{ 13, "Dentistry", "Preventive and restorative dental care."},
					{ 14, "Anesthesiology", "Pain management and anesthesia for surgical procedures."},
					{ 15, "Radiology", "Imaging diagnostics including X-rays, ultrasound, CT, and MRI."},
					{ 16, "Urology", "Diagnosis and treatment of urinary tract and male reproductive system disorders."},
					{ 17, "Ophthalmology", "Eye exams, vision care, and treatment of eye diseases."},
					{ 18, "Pulmonology", "Care for lung and respiratory system disorders."},
					{ 19, "Rheumatology", "Diagnosis and treatment of autoimmune and joint diseases."},
					{ 20, "Allergy and Immunology", "Diagnosis and treatment of allergies and immune-related hypersensitivities."}
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
				{ 9, "Lorazepam", "Sedative for anxiety relief and sleep disorders.", (int)MedicamentType.SedativeOrAnxiolytic, true },
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
				{ 21, "Melatonin", "Sleep aid and hormone supplement.", (int)MedicamentType.SedativeOrAnxiolytic, false },
				{ 22, "Warfarin", "Antiplatelet medication to prevent blood clotting.", (int)MedicamentType.Antiplatelet, true },
				{ 23, "Calcium", "Mineral supplement for bone health.", (int)MedicamentType.MineralSupplement, false },
				{ 24, "Hyoscyamine", "Antispasmodic for gastrointestinal disorders.", (int)MedicamentType.Antispasmodic, true },
				{ 25, "Cephalexin", "Antibiotic for bacterial skin and urinary infections.", (int)MedicamentType.Antibiotic, true },
				{ 26, "Simvastatin", "Statin used to lower cholesterol and reduce heart disease risk.", (int)MedicamentType.Hypertension, true },
				{ 27, "Prednisone", "Corticosteroid for inflammation and immune disorders.", (int)MedicamentType.AntiInflammatory, true },
				{ 28, "Baclofen", "Muscle relaxant for spasticity and muscle spasms.", (int)MedicamentType.Antispasmodic, true },
				{ 29, "Pantoprazole", "Proton pump inhibitor for acid reflux and gastric ulcers.", (int)MedicamentType.Antacid, true },
				{ 30, "Dabigatran", "Antiplatelet medication to prevent blood clots in atrial fibrillation.", (int)MedicamentType.Antiplatelet, true },
				{ 31, "Sertraline", "Antidepressant used for major depressive disorder and anxiety.", (int)MedicamentType.Antidepressant, true },
				{ 32, "Olanzapine", "Antipsychotic used for schizophrenia and bipolar disorder.", (int)MedicamentType.Antipsychotic, true },
				{ 33, "Salbutamol", "Bronchodilator used in asthma and COPD treatment.", (int)MedicamentType.Bronchodilator, true },
				{ 34, "Ondansetron", "Antiemetic used to prevent nausea and vomiting.", (int)MedicamentType.Antiemetic, true },
				{ 35, "Apixaban", "Anticoagulant used to prevent stroke and treat DVT.", (int)MedicamentType.Anticoagulant, true },
				{ 36, "Lactulose", "Laxative used to treat constipation.", (int)MedicamentType.Laxative, false }
			});


			migrationBuilder.InsertData(
				table: "Services",
				columns: new[] { "Id", "Name", "Description", "Price", "DurationMinutes", "SpecialityId" },
				values: new object[,]
				{
			        { 1, "Family Medicine", "Appointment with a family doctor.", 50.00m, 15, 1 },
			        { 2, "Heart Checkup", "Comprehensive evaluation of heart health.", 200.00m, 30, 2 },
					{ 3, "Neurological Consultation", "Assessment of nervous system disorders.", 250.00m, 45, 3 },
					{ 4, "Bone Fracture Treatment", "Diagnosis and treatment of bone fractures.", 300.00m, 60, 4 },
					{ 5, "Pediatric General Checkup", "Routine health checkup for children.", 100.00m, 30, 5 },
					{ 6, "Digestive System Analysis", "Comprehensive analysis of digestive health.", 180.00m, 45, 6 },
					{ 7, "Skin Condition Diagnosis", "Evaluation of skin issues and conditions.", 120.00m, 30, 7 },
					{ 8, "Mental Health Therapy", "Therapeutic session for mental well-being.", 150.00m, 60, 8 },
					{ 9, "Prenatal Checkup", "Regular checkup for pregnant women.", 130.00m, 30, 9 },
					{ 10, "Hormonal Therapy Session", "Management of hormonal imbalances.", 160.00m, 45, 10 },
					{ 11, "Cancer Screening", "Early detection and diagnosis of cancer.", 400.00m, 60, 11 },
					{ 12, "ECG Test", "Electrocardiogram to monitor heart activity.", 80.00m, 15, 2 },
					{ 13, "EEG Test", "Electroencephalogram for brain activity analysis.", 150.00m, 30, 3 },
					{ 14, "Physical Therapy Session", "Therapy for musculoskeletal rehabilitation.", 200.00m, 45, 4 },
					{ 15, "Child Vaccination", "Vaccination for common childhood diseases.", 50.00m, 15, 5 },
					{ 16, "Endoscopy", "Examination of the digestive tract.", 500.00m, 60, 6 },
					{ 17, "Acne Treatment", "Comprehensive treatment for acne.", 100.00m, 30, 7 },
					{ 18, "Psychiatric Evaluation", "Initial evaluation for mental health issues.", 120.00m, 30, 8 },
					{ 19, "Gynecological Consultation", "Women's health consultation.", 140.00m, 30, 9 },
					{ 20, "Diabetes Management", "Treatment plan for diabetes patients.", 150.00m, 30, 10 },
					{ 21, "Chemotherapy Session", "Cancer treatment with chemotherapy.", 1000.00m, 90, 11 },
					{ 22, "Diagnostic Testing", "Comprehensive diagnostic tests including blood, urine, and tissue samples analysis.", 100.00m, 30, 12 },
					{ 23, "Blood Sample Collection", "Service for drawing blood for various tests.", 30.00m, 15, 12 },
					{ 24, "Preoperative Anesthesia Consultation", "Assessment and planning for anesthesia before surgery.", 150.00m, 30, 13 },
					{ 25, "General Anesthesia Administration", "Administration of anesthesia during surgical procedures.", 300.00m, 45, 13 },
					{ 26, "X-ray Imaging", "Diagnostic imaging service using X-rays to assess bones, lungs, and other areas.", 120.00m, 30, 15 },
					{ 27, "CT Scan", "Detailed imaging to examine the internal structures of the body.", 400.00m, 60, 15 },
					{ 28, "Urinary Tract Infection Treatment", "Diagnosis and treatment of UTIs.", 150.00m, 30, 16 },
					{ 29, "Prostate Health Consultation", "Evaluation and management of prostate health.", 180.00m, 45, 16 },
					{ 30, "Eye Examination", "Comprehensive exam to assess vision and detect eye diseases.", 100.00m, 30, 17 },
					{ 31, "Cataract Surgery Consultation", "Consultation regarding cataract surgery options.", 200.00m, 45, 17 },
					{ 32, "Lung Function Test", "Test to evaluate lung health and function.", 180.00m, 30, 18 },
					{ 33, "Asthma Management", "Diagnosis and treatment plan for managing asthma.", 150.00m, 30, 18 },
					{ 34, "Arthritis Consultation", "Evaluation and treatment of joint pain and inflammation.", 120.00m, 30, 19 },
					{ 35, "Osteoporosis Treatment", "Diagnosis and management of osteoporosis.", 200.00m, 45, 19 },
					{ 36, "Allergy Testing", "Skin or blood tests to identify allergies.", 100.00m, 30, 20 },
					{ 37, "Immunotherapy", "Treatment aimed at desensitizing the immune system to allergens.", 150.00m, 45, 20 }
				});

			migrationBuilder.InsertData(
				table: "Users",
				columns: new[] { "Id", "Email", "Password", "Salt", "Name", "Surname", "Pesel", "PhoneNumber", "RoleId" },
				values: new object[,]
				{
					{ 1, "s26028@pjwstk.edu.pl", "9KSiVzvollBIW96nZlfhUEfMxRpxaP8Du2gsx5l0xhQ=", "43VkiaMpsLfNtTKbAfFhRVB7wLgWwc858dEarZn6K1w=", "Bogdan", "Sternicki", "00210816473", "+48123456789", 3 },
					{ 2, "john.smith@outlook.com", "9KSiVzvollBIW96nZlfhUEfMxRpxaP8Du2gsx5l0xhQ=", "43VkiaMpsLfNtTKbAfFhRVB7wLgWwc858dEarZn6K1w=", "John", "Smith", "12345678901", "+48123456789", 2 },
					{ 3, "emily.brown@outlook.com", "9KSiVzvollBIW96nZlfhUEfMxRpxaP8Du2gsx5l0xhQ=", "43VkiaMpsLfNtTKbAfFhRVB7wLgWwc858dEarZn6K1w=", "Emily", "Brown", "98765432109", "+48111222333", 2 },
					{ 4, "michael.green@outlook.com", "9KSiVzvollBIW96nZlfhUEfMxRpxaP8Du2gsx5l0xhQ=", "43VkiaMpsLfNtTKbAfFhRVB7wLgWwc858dEarZn6K1w=", "Michael", "Green", "56789012345", "+48765432101", 2 },
					{ 5, "sophia.wilson@outlook.com", "9KSiVzvollBIW96nZlfhUEfMxRpxaP8Du2gsx5l0xhQ=", "43VkiaMpsLfNtTKbAfFhRVB7wLgWwc858dEarZn6K1w=", "Sophia", "Wilson", "12309876543", "+48234567890", 2 },
					{ 6, "james.taylor@outlook.com", "9KSiVzvollBIW96nZlfhUEfMxRpxaP8Du2gsx5l0xhQ=", "43VkiaMpsLfNtTKbAfFhRVB7wLgWwc858dEarZn6K1w=", "James", "Taylor", "90123456789", "+48987654321", 2 },
					{ 7, "olivia.miller@outlook.com", "9KSiVzvollBIW96nZlfhUEfMxRpxaP8Du2gsx5l0xhQ=", "43VkiaMpsLfNtTKbAfFhRVB7wLgWwc858dEarZn6K1w=", "Olivia", "Miller", "65432198700", "+48555222334", 2 },
					{ 8, "william.moore@outlook.com", "9KSiVzvollBIW96nZlfhUEfMxRpxaP8Du2gsx5l0xhQ=", "43VkiaMpsLfNtTKbAfFhRVB7wLgWwc858dEarZn6K1w=", "William", "Moore", "34567290123", "+48770011223", 2 },
					{ 9, "ava.johnson@outlook.com", "9KSiVzvollBIW96nZlfhUEfMxRpxaP8Du2gsx5l0xhQ=", "43VkiaMpsLfNtTKbAfFhRVB7wLgWwc858dEarZn6K1w=", "Ava", "Johnson", "12309865789", "+48660123456", 2 },
					{ 10, "logan.white@outlook.com", "9KSiVzvollBIW96nZlfhUEfMxRpxaP8Du2gsx5l0xhQ=", "43VkiaMpsLfNtTKbAfFhRVB7wLgWwc858dEarZn6K1w=", "Logan", "White", "89012345678", "+48770033445", 2 },
					{ 11, "isabella.anderson@outlook.com", "9KSiVzvollBIW96nZlfhUEfMxRpxaP8Du2gsx5l0xhQ=", "43VkiaMpsLfNtTKbAfFhRVB7wLgWwc858dEarZn6K1w=", "Isabella", "Anderson", "67890123456", "+48555444678", 2 },
					{ 12, "lucas.king@outlook.com", "9KSiVzvollBIW96nZlfhUEfMxRpxaP8Du2gsx5l0xhQ=", "43VkiaMpsLfNtTKbAfFhRVB7wLgWwc858dEarZn6K1w=", "Lucas", "King", "45678907234", "+48990011223", 2 },
					{ 13, "mia.scott@outlook.com", "9KSiVzvollBIW96nZlfhUEfMxRpxaP8Du2gsx5l0xhQ=", "43VkiaMpsLfNtTKbAfFhRVB7wLgWwc858dEarZn6K1w=", "Mia", "Scott", "23456783012", "+48123499900", 2 },
					{ 14, "ethan.clark@outlook.com", "9KSiVzvollBIW96nZlfhUEfMxRpxaP8Du2gsx5l0xhQ=", "43VkiaMpsLfNtTKbAfFhRVB7wLgWwc858dEarZn6K1w=", "Ethan", "Clark", "79901234567", "+48765543322", 2 },
					{ 15, "emma.lewis@outlook.com", "9KSiVzvollBIW96nZlfhUEfMxRpxaP8Du2gsx5l0xhQ=", "43VkiaMpsLfNtTKbAfFhRVB7wLgWwc858dEarZn6K1w=", "Emma", "Lewis", "56789012340", "+48660077888", 2 },
					{ 16, "alexander.young@outlook.com", "9KSiVzvollBIW96nZlfhUEfMxRpxaP8Du2gsx5l0xhQ=", "43VkiaMpsLfNtTKbAfFhRVB7wLgWwc858dEarZn6K1w=", "Alexander", "Young", "89012346789", "+48778999001", 2 },
					{ 17, "sophie.wright@outlook.com", "9KSiVzvollBIW96nZlfhUEfMxRpxaP8Du2gsx5l0xhQ=", "43VkiaMpsLfNtTKbAfFhRVB7wLgWwc858dEarZn6K1w=", "Sophie", "Wright", "45612378900", "+48550123456", 2 },
					{ 18, "benjamin.hall@outlook.com", "9KSiVzvollBIW96nZlfhUEfMxRpxaP8Du2gsx5l0xhQ=", "43VkiaMpsLfNtTKbAfFhRVB7wLgWwc858dEarZn6K1w=", "Benjamin", "Hall", "12345678090", "+48660112233", 2 },
					{ 19, "lily.harris@outlook.com", "9KSiVzvollBIW96nZlfhUEfMxRpxaP8Du2gsx5l0xhQ=", "43VkiaMpsLfNtTKbAfFhRVB7wLgWwc858dEarZn6K1w=", "Lily", "Harris", "78945612345", "+48559988444", 2 },
					{ 20, "noah.robinson@outlook.com", "9KSiVzvollBIW96nZlfhUEfMxRpxaP8Du2gsx5l0xhQ=", "43VkiaMpsLfNtTKbAfFhRVB7wLgWwc858dEarZn6K1w=", "Noah", "Robinson", "65432198765", "+48990044556", 2 },
					{ 21, "charlotte.walker@outlook.com", "9KSiVzvollBIW96nZlfhUEfMxRpxaP8Du2gsx5l0xhQ=", "43VkiaMpsLfNtTKbAfFhRVB7wLgWwc858dEarZn6K1w=", "Charlotte", "Walker", "12389045678", "+48771122334", 2 },
					{ 22, "daniel.thompson@outlook.com", "9KSiVzvollBIW96nZlfhUEfMxRpxaP8Du2gsx5l0xhQ=", "43VkiaMpsLfNtTKbAfFhRVB7wLgWwc858dEarZn6K1w=", "Daniel", "Thompson", "98765432111", "+48667899811", 2 },
					{ 23, "john.doe@outlook.com", "9KSiVzvollBIW96nZlfhUEfMxRpxaP8Du2gsx5l0xhQ=", "43VkiaMpsLfNtTKbAfFhRVB7wLgWwc858dEarZn6K1w=", "John", "Doe", "12345678912", "+48123456790", 2 },
					{ 24, "mary.jones@outlook.com", "9KSiVzvollBIW96nZlfhUEfMxRpxaP8Du2gsx5l0xhQ=", "43VkiaMpsLfNtTKbAfFhRVB7wLgWwc858dEarZn6K1w=", "Mary", "Jones", "23456789012", "+48234567891", 2 },
					{ 25, "james.brown@outlook.com", "9KSiVzvollBIW96nZlfhUEfMxRpxaP8Du2gsx5l0xhQ=", "43VkiaMpsLfNtTKbAfFhRVB7wLgWwc858dEarZn6K1w=", "James", "Brown", "34567890163", "+48345678902", 2 },
					{ 26, "patricia.smith@outlook.com", "9KSiVzvollBIW96nZlfhUEfMxRpxaP8Du2gsx5l0xhQ=", "43VkiaMpsLfNtTKbAfFhRVB7wLgWwc858dEarZn6K1w=", "Patricia", "Smith", "45678901234", "+48456789013", 2 },
					{ 27, "robert.johnson@outlook.com", "9KSiVzvollBIW96nZlfhUEfMxRpxaP8Du2gsx5l0xhQ=", "43VkiaMpsLfNtTKbAfFhRVB7wLgWwc858dEarZn6K1w=", "Robert", "Johnson", "56789012245", "+48567890124", 2 },
					{ 28, "linda.miller@outlook.com", "9KSiVzvollBIW96nZlfhUEfMxRpxaP8Du2gsx5l0xhQ=", "43VkiaMpsLfNtTKbAfFhRVB7wLgWwc858dEarZn6K1w=", "Linda", "Miller", "67890193456", "+48678901235", 2 },
					{ 29, "michael.garcia@outlook.com", "9KSiVzvollBIW96nZlfhUEfMxRpxaP8Du2gsx5l0xhQ=", "43VkiaMpsLfNtTKbAfFhRVB7wLgWwc858dEarZn6K1w=", "Michael", "Garcia", "78901234567", "+48789012346", 2 },
					{ 30, "elizabeth.rodriguez@outlook.com", "9KSiVzvollBIW96nZlfhUEfMxRpxaP8Du2gsx5l0xhQ=", "43VkiaMpsLfNtTKbAfFhRVB7wLgWwc858dEarZn6K1w=", "Elizabeth", "Rodriguez", "89012325678", "+48890123457", 2 },
					{ 31, "william.davis@outlook.com", "9KSiVzvollBIW96nZlfhUEfMxRpxaP8Du2gsx5l0xhQ=", "43VkiaMpsLfNtTKbAfFhRVB7wLgWwc858dEarZn6K1w=", "William", "Davis", "90128456789", "+48901234568", 2 },
					{ 32, "susan.martinez@outlook.com", "9KSiVzvollBIW96nZlfhUEfMxRpxaP8Du2gsx5l0xhQ=", "43VkiaMpsLfNtTKbAfFhRVB7wLgWwc858dEarZn6K1w=", "Susan", "Martinez", "12345678921", "+48012345679", 2 },
					{ 33, "charles.hernandez@outlook.com", "9KSiVzvollBIW96nZlfhUEfMxRpxaP8Du2gsx5l0xhQ=", "43VkiaMpsLfNtTKbAfFhRVB7wLgWwc858dEarZn6K1w=", "Charles", "Hernandez", "23456789022", "+48123456780", 2 },
					{ 34, "joseph.lopez@outlook.com", "9KSiVzvollBIW96nZlfhUEfMxRpxaP8Du2gsx5l0xhQ=", "43VkiaMpsLfNtTKbAfFhRVB7wLgWwc858dEarZn6K1w=", "Joseph", "Lopez", "33567890123", "+48234567881", 2 },
					{ 35, "jk@pjwstk.edu.pl", "9KSiVzvollBIW96nZlfhUEfMxRpxaP8Du2gsx5l0xhQ=", "43VkiaMpsLfNtTKbAfFhRVB7wLgWwc858dEarZn6K1w=", "Jan", "Kowalski", "56395736192", "+48926384057", 1 },
					{ 36, "ak@pjwstk.edu.pl", "9KSiVzvollBIW96nZlfhUEfMxRpxaP8Du2gsx5l0xhQ=", "43VkiaMpsLfNtTKbAfFhRVB7wLgWwc858dEarZn6K1w=", "Adam", "Kowalski", "46152430983", "+48123456780", 1 },
					{ 37, "mk@pjwstk.edu.pl", "9KSiVzvollBIW96nZlfhUEfMxRpxaP8Du2gsx5l0xhQ=", "43VkiaMpsLfNtTKbAfFhRVB7wLgWwc858dEarZn6K1w=", "Michał", "Kowalski", "60937462539", "+48698714231", 1 }
				});

			migrationBuilder.InsertData(
				table: "Admins",
				columns: new[] { "UserId", "LastLogin", "Status" },
				values: new object[]
				{ 1, DateTime.Now, (int)AdminStatus.Active });

			migrationBuilder.InsertData(
				table: "Doctors",
				columns: new[] { "UserId", "EmploymentDate", "SpecialityId" },
				values: new object[,]
				{
					{ 2, new DateTime(2023, 7, 15), 1 },
					{ 3, new DateTime(2024, 3, 22), 2 },
					{ 4, new DateTime(2022, 5, 10), 3 },
					{ 5, new DateTime(2021, 8, 25), 4 },
					{ 6, new DateTime(2023, 12, 30), 5 },
					{ 7, new DateTime(2020, 2, 17), 6 },
					{ 8, new DateTime(2024, 1, 9), 7 },
					{ 9, new DateTime(2023, 6, 1), 8 },
					{ 10, new DateTime(2022, 9, 12), 9 },
					{ 11, new DateTime(2021, 11, 5), 10 },
					{ 12, new DateTime(2023, 4, 19), 11 },
					{ 13, new DateTime(2020, 7, 22), 12 },
					{ 14, new DateTime(2022, 3, 15), 13 },
					{ 15, new DateTime(2024, 5, 6), 14 },
					{ 16, new DateTime(2021, 10, 11), 15 },
					{ 17, new DateTime(2023, 8, 30), 16 },
					{ 18, new DateTime(2022, 12, 18), 17 },
					{ 19, new DateTime(2020, 4, 14), 18 },
					{ 20, new DateTime(2024, 2, 27), 19 },
					{ 21, new DateTime(2021, 9, 9), 20 },
					{ 22, new DateTime(2023, 1, 20), 1 },
					{ 23, new DateTime(2024, 3, 5), 2 },
					{ 24, new DateTime(2023, 5, 17), 4 },
					{ 25, new DateTime(2022, 11, 9), 5 },
					{ 26, new DateTime(2023, 7, 10), 6 },
					{ 27, new DateTime(2021, 10, 25), 7 },
					{ 28, new DateTime(2024, 1, 8), 8 },
					{ 29, new DateTime(2023, 2, 13), 9 },
					{ 30, new DateTime(2021, 6, 18), 10 },
					{ 31, new DateTime(2022, 9, 3), 11 },
					{ 32, new DateTime(2024, 4, 14), 13 },
					{ 33, new DateTime(2021, 5, 7), 15 },
					{ 34, new DateTime(2022, 12, 23), 18 }
				});

			migrationBuilder.InsertData(
				table: "Patients",
				columns: new[] { "UserId", "RegisterDate", "BirthDate", "PatientCard" },
				values: new object[,]
				{
					{ 35, DateTime.Now, new DateTime(1996, 4, 16), "56395764759913205892" },
					{ 36, DateTime.Now, new DateTime(1993, 10, 5), "46152446285731205482" },
					{ 37, DateTime.Now, new DateTime(2002, 8, 25), "60937462549387123046" }
				});

			migrationBuilder.InsertData(
				table: "DoctorsAvailabilities",
				columns: new[] { "Id", "From", "To", "DoctorsUserId" },
				values: new object[,]
				{
					{ 1, new DateTime(2025, 3, 3, 8, 0, 0), new DateTime(2025, 6, 18, 16, 0, 0), 2 },
					{ 2, new DateTime(2025, 3, 7, 8, 0, 0), new DateTime(2025, 7, 2, 16, 0, 0), 3 },
					{ 3, new DateTime(2025, 3, 12, 8, 0, 0), new DateTime(2025, 6, 30, 16, 0, 0), 4 },
					{ 4, new DateTime(2025, 3, 5, 8, 0, 0), new DateTime(2025, 7, 15, 16, 0, 0), 5 },
					{ 5, new DateTime(2025, 3, 9, 8, 0, 0), new DateTime(2025, 6, 25, 16, 0, 0), 6 },
					{ 6, new DateTime(2025, 3, 18, 8, 0, 0), new DateTime(2025, 7, 9, 16, 0, 0), 7 },
					{ 7, new DateTime(2025, 3, 22, 8, 0, 0), new DateTime(2025, 6, 20, 16, 0, 0), 8 },
					{ 8, new DateTime(2025, 4, 2, 8, 0, 0), new DateTime(2025, 7, 28, 16, 0, 0), 9 },
					{ 9, new DateTime(2025, 3, 13, 8, 0, 0), new DateTime(2025, 8, 10, 16, 0, 0), 10 },
					{ 10, new DateTime(2025, 3, 26, 8, 0, 0), new DateTime(2025, 9, 3, 16, 0, 0), 11 },
					{ 11, new DateTime(2025, 3, 17, 8, 0, 0), new DateTime(2025, 7, 20, 16, 0, 0), 12 },
					{ 12, new DateTime(2025, 4, 1, 8, 0, 0), new DateTime(2025, 8, 1, 16, 0, 0), 13 },
					{ 13, new DateTime(2025, 3, 6, 8, 0, 0), new DateTime(2025, 7, 5, 16, 0, 0), 14 },
					{ 14, new DateTime(2025, 4, 8, 8, 0, 0), new DateTime(2025, 6, 28, 16, 0, 0), 15 },
					{ 15, new DateTime(2025, 3, 15, 8, 0, 0), new DateTime(2025, 8, 7, 16, 0, 0), 16 },
					{ 16, new DateTime(2025, 4, 3, 8, 0, 0), new DateTime(2025, 7, 16, 16, 0, 0), 17 },
					{ 17, new DateTime(2025, 3, 29, 8, 0, 0), new DateTime(2025, 8, 3, 16, 0, 0), 18 },
					{ 18, new DateTime(2025, 3, 4, 8, 0, 0), new DateTime(2025, 9, 2, 16, 0, 0), 19 },
					{ 19, new DateTime(2025, 3, 19, 8, 0, 0), new DateTime(2025, 7, 18, 16, 0, 0), 20 },
					{ 20, new DateTime(2025, 3, 30, 8, 0, 0), new DateTime(2025, 9, 10, 16, 0, 0), 21 },
					{ 21, new DateTime(2025, 4, 4, 8, 0, 0), new DateTime(2025, 6, 21, 16, 0, 0), 22 },
					{ 22, new DateTime(2025, 3, 21, 8, 0, 0), new DateTime(2025, 8, 25, 16, 0, 0), 23 },
					{ 23, new DateTime(2025, 4, 7, 8, 0, 0), new DateTime(2025, 9, 5, 16, 0, 0), 24 },
					{ 24, new DateTime(2025, 3, 11, 8, 0, 0), new DateTime(2025, 7, 14, 16, 0, 0), 25 },
					{ 25, new DateTime(2025, 4, 5, 8, 0, 0), new DateTime(2025, 9, 1, 16, 0, 0), 26 },
					{ 26, new DateTime(2025, 4, 9, 8, 0, 0), new DateTime(2025, 8, 22, 16, 0, 0), 27 },
					{ 27, new DateTime(2025, 3, 4, 8, 0, 0), new DateTime(2025, 6, 26, 16, 0, 0), 28 },
					{ 28, new DateTime(2025, 3, 27, 8, 0, 0), new DateTime(2025, 8, 8, 16, 0, 0), 29 },
					{ 29, new DateTime(2025, 3, 23, 8, 0, 0), new DateTime(2025, 9, 4, 16, 0, 0), 30 },
					{ 30, new DateTime(2025, 4, 6, 8, 0, 0), new DateTime(2025, 7, 23, 16, 0, 0), 31 },
					{ 31, new DateTime(2025, 4, 2, 8, 0, 0), new DateTime(2025, 8, 15, 16, 0, 0), 32 },
					{ 32, new DateTime(2025, 3, 10, 8, 0, 0), new DateTime(2025, 9, 8, 16, 0, 0), 33 },
					{ 33, new DateTime(2025, 3, 25, 8, 0, 0), new DateTime(2025, 6, 30, 16, 0, 0), 34 }
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
				keyValues: new object[] { 1, 2, 3, 4, 5 });

			migrationBuilder.DeleteData(
				table: "RolePermissions",
				keyColumns: new[] { "RoleId", "PermissionId" },
				keyValues: new object[,]
				{
					{ 1, 2 },
					{ 1, 3 },
					{ 2, 1 },
					{ 2, 2 },
					{ 2, 4 },
					{ 2, 5 }
				});

			migrationBuilder.DeleteData(
				table: "Specialities",
				keyColumn: "Id",
				keyValues: new object[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 });

			migrationBuilder.DeleteData(
				table: "Medicaments",
				keyColumn: "Id",
				keyValues: new object[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36 });

			migrationBuilder.DeleteData(
				table: "Services",
				keyColumn: "Id",
				keyValues: new object[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37 });

			migrationBuilder.DeleteData(
				table: "Users",
				keyColumn: "Id",
				keyValues: new object[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37 });

			migrationBuilder.DeleteData(
				table: "Admins",
				keyColumn: "UserId",
				keyValues: new object[] { 1 });

			migrationBuilder.DeleteData(
				table: "Doctors",
				keyColumn: "UserId",
				keyValues: new object[] { 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34 });

			migrationBuilder.DeleteData(
				table: "Patients",
				keyColumn: "UserId",
				keyValues: new object[] { 35, 36, 37 });

			migrationBuilder.DeleteData(
				table: "DoctorsAvailabilities",
				keyColumn: "Id",
				keyValues: new object[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33 });
		}
	}
}
