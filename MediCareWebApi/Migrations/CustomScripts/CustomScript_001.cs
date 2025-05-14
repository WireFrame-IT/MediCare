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
					{1, "Wyświetlanie wszystkich wizyt", true, false, (int)PermissionType.ViewAllAppointments },
					{2, "Anulowanie wizyty", false, false, (int)PermissionType.CancelAppointment },
					{3, "Wybór lekarza", false, true, (int)PermissionType.ChooseDoctor }
				});

			migrationBuilder.InsertData(
				table: "RolePermissions",
				columns: new [] { "RoleId", "PermissionId" },
				values: new object[,]
				{
					{ 1, 2 },
					{ 1, 3 },
					{ 2, 1 },
					{ 2, 2 }
				});

			migrationBuilder.InsertData(
				table: "Specialities",
				columns: new[] { "Id", "Name", "Description" },
				values: new object[,]
				{
					{ 1, "Medycyna rodzinna", "Kompleksowa opieka podstawowa dla pacjentów w każdym wieku." },
					{ 2, "Kardiologia", "Diagnoza i leczenie chorób serca i układu krążenia." },
					{ 3, "Neurologia", "Opieka nad zaburzeniami mózgu, rdzenia kręgowego i układu nerwowego." },
					{ 4, "Ortopedia", "Leczenie kości, stawów, mięśni i związanych z nimi urazów." },
					{ 5, "Pediatria", "Opieka medyczna dla niemowląt, dzieci i młodzieży." },
					{ 6, "Gastroenterologia", "Diagnoza i leczenie zaburzeń układu pokarmowego." },
					{ 7, "Dermatologia", "Opieka nad chorobami skóry, włosów i paznokci." },
					{ 8, "Psychiatria", "Diagnoza i leczenie zaburzeń zdrowia psychicznego." },
					{ 9, "Ginekologia", "Zdrowie reprodukcyjne kobiet, w tym diagnostyka i leczenie." },
					{ 10, "Endokrynologia", "Leczenie zaburzeń hormonalnych i metabolicznych." },
					{ 11, "Onkologia", "Diagnoza i opieka nad pacjentami z nowotworami." },
					{ 12, "Medycyna laboratoryjna", "Testy diagnostyczne i analiza krwi, tkanek oraz innych próbek." },
					{ 13, "Stomatologia", "Profilaktyczna i rekonstrukcyjna opieka dentystyczna." },
					{ 14, "Anestezjologia", "Zarządzanie bólem i znieczulenie podczas zabiegów chirurgicznych." },
					{ 15, "Radiologia", "Diagnostyka obrazowa, w tym zdjęcia rentgenowskie, ultradźwięki, TK i MRI." },
					{ 16, "Urologia", "Diagnoza i leczenie zaburzeń układu moczowego oraz męskiego układu rozrodczego." },
					{ 17, "Okulistyka", "Badania wzroku, opieka nad zdrowiem oczu i leczenie chorób oczu." },
					{ 18, "Pulmonologia", "Opieka nad chorobami płuc i układu oddechowego." },
					{ 19, "Reumatologia", "Diagnoza i leczenie chorób autoimmunologicznych oraz chorób stawów." },
					{ 20, "Alergologia i immunologia", "Diagnoza i leczenie alergii oraz nadwrażliwości związanych z układem odpornościowym." }
				});

			migrationBuilder.InsertData(
			table: "Medicaments",
			columns: new[] { "Id", "Name", "Description", "MedicamentType", "PrescriptionRequired" },
				values: new object[,]
				{
					{ 1, "Aspiryna", "Środek przeciwbólowy, przeciwzapalny i przeciwgorączkowy.", (int)MedicamentType.Analgesic, true },
					{ 2, "Amoksycylina", "Antybiotyk stosowany w leczeniu infekcji bakteryjnych.", (int)MedicamentType.Antibiotic, true },
					{ 3, "Ibuprofen", "Niesteroidowy lek przeciwzapalny (NSAID) łagodzący ból, stany zapalne i gorączkę.", (int)MedicamentType.AntiInflammatory, true },
					{ 4, "Oseltamiwir", "Lek przeciwwirusowy stosowany w leczeniu grypy.", (int)MedicamentType.Antiviral, true },
					{ 5, "Klotrimazol", "Środek przeciwgrzybiczy stosowany w leczeniu infekcji skóry i błon śluzowych.", (int)MedicamentType.Antifungal, true },
					{ 6, "Difenhydramina", "Antyhistamina stosowana w łagodzeniu objawów alergii i jako środek nasenny.", (int)MedicamentType.Antihistamine, true },
					{ 7, "Paracetamol", "Powszechny lek przeciwbólowy i przeciwgorączkowy.", (int)MedicamentType.Antipyretic, false },
					{ 8, "Witamina C", "Suplement witaminowy stosowany w celu wzmocnienia odporności.", (int)MedicamentType.VitaminsAndSupplements, false },
					{ 9, "Lorazepam", "Środek uspokajający stosowany w leczeniu lęków i zaburzeń snu.", (int)MedicamentType.SedativeOrAnxiolytic, true },
					{ 10, "Dekstrometorfan", "Środek przeciwkaszlowy stosowany w leczeniu suchego kaszlu.", (int)MedicamentType.CoughSuppressant, false },
					{ 11, "Lisinopryl", "Lek przeciwnadciśnieniowy stosowany w leczeniu wysokiego ciśnienia krwi.", (int)MedicamentType.Hypertension, true },
					{ 12, "Metformina", "Lek przeciwcukrzycowy stosowany w leczeniu cukrzycy typu 2.", (int)MedicamentType.Antidiabetic, true },
					{ 13, "Estradiol", "Terapia hormonalna stosowana w zastępowaniu estrogenów.", (int)MedicamentType.Hormonal, true },
					{ 14, "Cyklosporyna", "Immunosupresant stosowany w celu zapobiegania odrzutom przeszczepów organów.", (int)MedicamentType.Immunosuppressant, true },
					{ 15, "Omeprazol", "Lek zobojętniający kwas żołądkowy stosowany w leczeniu refluksu i choroby wrzodowej przełyku.", (int)MedicamentType.Antacid, false },
					{ 16, "Tobramycyna", "Antybiotyk stosowany w leczeniu infekcji dróg oddechowych.", (int)MedicamentType.Antibiotic, true },
					{ 17, "Timolol", "Beta-bloker stosowany w leczeniu jaskry.", (int)MedicamentType.Ophthalmic, true },
					{ 18, "Hydrokortyzon", "Przeciwzapalny kortykosteroid stosowany w leczeniu stanów zapalnych skóry i stawów.", (int)MedicamentType.AntiInflammatory, true },
					{ 19, "Nystatyna", "Środek przeciwgrzybiczy stosowany w leczeniu infekcji jamy ustnej i pochwy.", (int)MedicamentType.Antifungal, true },
					{ 20, "Fexofenadyna", "Antyhistamina, która nie powoduje senności, stosowana w leczeniu alergii sezonowych.", (int)MedicamentType.Antihistamine, false },
					{ 21, "Melatonina", "Środek nasenny i suplement hormonu melatoniny.", (int)MedicamentType.SedativeOrAnxiolytic, false },
					{ 22, "Warfaryna", "Lek przeciwpłytkowy zapobiegający krzepnięciu krwi.", (int)MedicamentType.Antiplatelet, true },
					{ 23, "Wapń", "Suplement mineralny stosowany w celu wzmocnienia kości.", (int)MedicamentType.MineralSupplement, false },
					{ 24, "Hyoscyamina", "Środek spazmolityczny stosowany w leczeniu zaburzeń przewodu pokarmowego.", (int)MedicamentType.Antispasmodic, true },
					{ 25, "Cefaleksyna", "Antybiotyk stosowany w leczeniu infekcji skóry i dróg moczowych.", (int)MedicamentType.Antibiotic, true },
					{ 26, "Simwastatyna", "Statyna stosowana w obniżaniu poziomu cholesterolu i redukcji ryzyka chorób serca.", (int)MedicamentType.Hypertension, true },
					{ 27, "Prednizolon", "Kortykosteroid stosowany w leczeniu stanów zapalnych i zaburzeń immunologicznych.", (int)MedicamentType.AntiInflammatory, true },
					{ 28, "Baclofen", "Środek rozluźniający mięśnie stosowany w leczeniu spastyczności i skurczów mięśni.", (int)MedicamentType.Antispasmodic, true },
					{ 29, "Pantoprazol", "Inhibitor pompy protonowej stosowany w leczeniu refluksu żołądkowo-przełykowego i wrzodów żołądka.", (int)MedicamentType.Antacid, true },
					{ 30, "Dabigatran", "Lek przeciwpłytkowy stosowany w zapobieganiu udarowi mózgu i leczeniu zakrzepicy żył głębokich.", (int)MedicamentType.Antiplatelet, true },
					{ 31, "Sertralina", "Lek przeciwdepresyjny stosowany w leczeniu dużych zaburzeń depresyjnych i lękowych.", (int)MedicamentType.Antidepressant, true },
					{ 32, "Olanzapina", "Lek przeciwpsychotyczny stosowany w leczeniu schizofrenii i choroby afektywnej dwubiegunowej.", (int)MedicamentType.Antipsychotic, true },
					{ 33, "Salbutamol", "Środek rozszerzający oskrzela stosowany w leczeniu astmy i POChP.", (int)MedicamentType.Bronchodilator, true },
					{ 34, "Ondansetron", "Środek przeciwwymiotny stosowany w zapobieganiu nudnościom i wymiotom.", (int)MedicamentType.Antiemetic, true },
					{ 35, "Apiksaban", "Lek przeciwzakrzepowy stosowany w zapobieganiu udarowi mózgu i leczeniu zakrzepicy żył głębokich.", (int)MedicamentType.Anticoagulant, true },
					{ 36, "Laktuloza", "Środek przeczyszczający stosowany w leczeniu zaparć.", (int)MedicamentType.Laxative, false }
				});

			migrationBuilder.InsertData(
				table: "Services",
				columns: new[] { "Id", "Name", "Description", "Price", "DurationMinutes", "SpecialityId" },
				values: new object[,]
				{
					{ 1, "Medycyna rodzinna", "Wizyta u lekarza rodzinnego.", 50.00m, 15, 1 },
					{ 2, "Badanie serca", "Kompleksowa ocena zdrowia serca.", 200.00m, 30, 2 },
					{ 3, "Konsultacja neurologiczna", "Ocena zaburzeń układu nerwowego.", 250.00m, 45, 3 },
					{ 4, "Leczenie złamań kości", "Diagnoza i leczenie złamań kości.", 300.00m, 60, 4 },
					{ 5, "Pediatryczne badanie ogólne", "Rutynowe badanie zdrowia dzieci.", 100.00m, 30, 5 },
					{ 6, "Analiza układu pokarmowego", "Kompleksowa analiza zdrowia układu pokarmowego.", 180.00m, 45, 6 },
					{ 7, "Diagnoza chorób skórnych", "Ocena problemów i chorób skórnych.", 120.00m, 30, 7 },
					{ 8, "Terapia zdrowia psychicznego", "Sesja terapeutyczna dla dobrostanu psychicznego.", 150.00m, 60, 8 },
					{ 9, "Badanie prenatalne", "Regularne badanie dla kobiet w ciąży.", 130.00m, 30, 9 },
					{ 10, "Sesja terapii hormonalnej", "Zarządzanie zaburzeniami hormonalnymi.", 160.00m, 45, 10 },
					{ 11, "Badanie wczesnej diagnostyki nowotworowej", "Wczesne wykrywanie i diagnoza nowotworów.", 400.00m, 60, 11 },
					{ 12, "Test EKG", "Elektrokardiogram do monitorowania aktywności serca.", 80.00m, 15, 2 },
					{ 13, "Test EEG", "Elektroencefalogram do analizy aktywności mózgu.", 150.00m, 30, 3 },
					{ 14, "Sesja terapii fizycznej", "Terapia rehabilitacyjna dla układu mięśniowo-szkieletowego.", 200.00m, 45, 4 },
					{ 15, "Szczepienie dzieci", "Szczepienie przeciw powszechnym chorobom dziecięcym.", 50.00m, 15, 5 },
					{ 16, "Endoskopia", "Badanie układu pokarmowego za pomocą endoskopu.", 500.00m, 60, 6 },
					{ 17, "Leczenie trądziku", "Kompleksowe leczenie trądziku.", 100.00m, 30, 7 },
					{ 18, "Ocena psychiatryczna", "Wstępna ocena problemów ze zdrowiem psychicznym.", 120.00m, 30, 8 },
					{ 19, "Konsultacja ginekologiczna", "Konsultacja zdrowia kobiet.", 140.00m, 30, 9 },
					{ 20, "Konsultacja diabetologiczna", "Plan leczenia dla pacjentów z cukrzycą.", 150.00m, 30, 10 },
					{ 21, "Sesja chemioterapii", "Leczenie nowotworów chemioterapią.", 1000.00m, 90, 11 },
					{ 22, "Badania diagnostyczne", "Kompleksowe testy diagnostyczne, w tym analiza krwi, moczu i próbek tkanek.", 100.00m, 30, 12 },
					{ 23, "Pobieranie próbki krwi", "Usługa pobierania krwi do różnych badań.", 30.00m, 15, 12 },
					{ 24, "Konsultacja przedoperacyjna anestezjologiczna", "Ocena i planowanie znieczulenia przed zabiegiem chirurgicznym.", 150.00m, 30, 13 },
					{ 25, "Podanie znieczulenia ogólnego", "Podanie znieczulenia podczas zabiegów chirurgicznych.", 300.00m, 45, 13 },
					{ 26, "Obrazowanie rentgenowskie", "Usługa diagnostyczna wykorzystująca zdjęcia rentgenowskie do oceny kości, płuc i innych obszarów.", 120.00m, 30, 15 },
					{ 27, "Tomografia komputerowa (CT)", "Szczegółowe obrazowanie do badania wewnętrznych struktur ciała.", 400.00m, 60, 15 },
					{ 28, "Leczenie infekcji dróg moczowych", "Diagnoza i leczenie infekcji dróg moczowych.", 150.00m, 30, 16 },
					{ 29, "Konsultacja zdrowia prostaty", "Ocena i zarządzanie zdrowiem prostaty.", 180.00m, 45, 16 },
					{ 30, "Badanie wzroku", "Kompleksowe badanie wzroku i wykrywanie chorób oczu.", 100.00m, 30, 17 },
					{ 31, "Konsultacja w sprawie operacji zaćmy", "Konsultacja dotycząca opcji operacyjnych przy zaćmie.", 200.00m, 45, 17 },
					{ 32, "Test funkcji płuc", "Test oceniający zdrowie i funkcjonowanie płuc.", 180.00m, 30, 18 },
					{ 33, "Konsultacja pulmonologiczna", "Diagnoza i plan leczenia astmy.", 150.00m, 30, 18 },
					{ 34, "Konsultacja w sprawie artretyzmu", "Ocena i leczenie bólu i stanów zapalnych stawów.", 120.00m, 30, 19 },
					{ 35, "Leczenie osteoporozy", "Diagnoza i zarządzanie osteoporozą.", 200.00m, 45, 19 },
					{ 36, "Testy alergiczne", "Testy skórne lub krwi do identyfikacji alergii.", 100.00m, 30, 20 },
					{ 37, "Immunoterapia", "Leczenie mające na celu odczulanie układu odpornościowego na alergeny.", 150.00m, 45, 20 }
				});

			migrationBuilder.InsertData(
				table: "Users",
				columns: new[] { "Id", "Email", "Password", "Salt", "Name", "Surname", "Pesel", "PhoneNumber", "RoleId" },
				values: new object[,]
				{
					{ 1, "s26028@pjwstk.edu.pl", "9KSiVzvollBIW96nZlfhUEfMxRpxaP8Du2gsx5l0xhQ=", "43VkiaMpsLfNtTKbAfFhRVB7wLgWwc858dEarZn6K1w=", "Bogdan", "Sternicki", "00210816473", "+48123456789", 3 },
					{ 2, "michal.wisniewski@pjwstk.edu.pl", "9KSiVzvollBIW96nZlfhUEfMxRpxaP8Du2gsx5l0xhQ=", "43VkiaMpsLfNtTKbAfFhRVB7wLgWwc858dEarZn6K1w=", "Michał", "Wiśniewski", "12345678901", "+48123456789", 2 },
					{ 3, "anna.nowak@pjwstk.edu.pl", "9KSiVzvollBIW96nZlfhUEfMxRpxaP8Du2gsx5l0xhQ=", "43VkiaMpsLfNtTKbAfFhRVB7wLgWwc858dEarZn6K1w=", "Anna", "Nowak", "98765432109", "+48111222333", 2 },
					{ 4, "marek.wisniewski@pjwstk.edu.pl", "9KSiVzvollBIW96nZlfhUEfMxRpxaP8Du2gsx5l0xhQ=", "43VkiaMpsLfNtTKbAfFhRVB7wLgWwc858dEarZn6K1w=", "Marek", "Wiśniewski", "56789012345", "+48765432101", 2 },
					{ 5, "katarzyna.wojciechowska@pjwstk.edu.pl", "9KSiVzvollBIW96nZlfhUEfMxRpxaP8Du2gsx5l0xhQ=", "43VkiaMpsLfNtTKbAfFhRVB7wLgWwc858dEarZn6K1w=", "Katarzyna", "Wojciechowska", "12309876543", "+48234567890", 2 },
					{ 6, "lukasz.kaczmarek@pjwstk.edu.pl", "9KSiVzvollBIW96nZlfhUEfMxRpxaP8Du2gsx5l0xhQ=", "43VkiaMpsLfNtTKbAfFhRVB7wLgWwc858dEarZn6K1w=", "Łukasz", "Kaczmarek", "90123456789", "+48987654321", 2 },
					{ 7, "magdalena.krawczyk@pjwstk.edu.pl", "9KSiVzvollBIW96nZlfhUEfMxRpxaP8Du2gsx5l0xhQ=", "43VkiaMpsLfNtTKbAfFhRVB7wLgWwc858dEarZn6K1w=", "Magdalena", "Krawczyk", "65432198700", "+48555222334", 2 },
					{ 8, "adam.mazur@pjwstk.edu.pl", "9KSiVzvollBIW96nZlfhUEfMxRpxaP8Du2gsx5l0xhQ=", "43VkiaMpsLfNtTKbAfFhRVB7wLgWwc858dEarZn6K1w=", "Adam", "Mazur", "34567290123", "+48770011223", 2 },
					{ 9, "agnieszka.kalinowska@pjwstk.edu.pl", "9KSiVzvollBIW96nZlfhUEfMxRpxaP8Du2gsx5l0xhQ=", "43VkiaMpsLfNtTKbAfFhRVB7wLgWwc858dEarZn6K1w=", "Agnieszka", "Kalinowska", "12309865789", "+48660123456", 2 },
					{ 10, "pawel.piotrowski@pjwstk.edu.pl", "9KSiVzvollBIW96nZlfhUEfMxRpxaP8Du2gsx5l0xhQ=", "43VkiaMpsLfNtTKbAfFhRVB7wLgWwc858dEarZn6K1w=", "Paweł", "Piotrowski", "89012345678", "+48770033445", 2 },
					{ 11, "aleksandra.grabowska@pjwstk.edu.pl", "9KSiVzvollBIW96nZlfhUEfMxRpxaP8Du2gsx5l0xhQ=", "43VkiaMpsLfNtTKbAfFhRVB7wLgWwc858dEarZn6K1w=", "Aleksandra", "Grabowska", "67890123456", "+48555444678", 2 },
					{ 12, "michal.lewandowski@pjwstk.edu.pl", "9KSiVzvollBIW96nZlfhUEfMxRpxaP8Du2gsx5l0xhQ=", "43VkiaMpsLfNtTKbAfFhRVB7wLgWwc858dEarZn6K1w=", "Michał", "Lewandowski", "45678907234", "+48990011223", 2 },
					{ 13, "zofia.zielinska@pjwstk.edu.pl", "9KSiVzvollBIW96nZlfhUEfMxRpxaP8Du2gsx5l0xhQ=", "43VkiaMpsLfNtTKbAfFhRVB7wLgWwc858dEarZn6K1w=", "Zofia", "Zielińska", "23456783012", "+48123499900", 2 },
					{ 14, "piotr.szymanski@pjwstk.edu.pl", "9KSiVzvollBIW96nZlfhUEfMxRpxaP8Du2gsx5l0xhQ=", "43VkiaMpsLfNtTKbAfFhRVB7wLgWwc858dEarZn6K1w=", "Piotr", "Szymański", "79901234567", "+48765543322", 2 },
					{ 15, "emilia.jankowska@pjwstk.edu.pl", "9KSiVzvollBIW96nZlfhUEfMxRpxaP8Du2gsx5l0xhQ=", "43VkiaMpsLfNtTKbAfFhRVB7wLgWwc858dEarZn6K1w=", "Emilia", "Jankowska", "56789012340", "+48660077888", 2 },
					{ 16, "krzysztof.wozniak@pjwstk.edu.pl", "9KSiVzvollBIW96nZlfhUEfMxRpxaP8Du2gsx5l0xhQ=", "43VkiaMpsLfNtTKbAfFhRVB7wLgWwc858dEarZn6K1w=", "Krzysztof", "Woźniak", "89012346789", "+48778999001", 2 },
					{ 17, "natalia.kot@pjwstk.edu.pl", "9KSiVzvollBIW96nZlfhUEfMxRpxaP8Du2gsx5l0xhQ=", "43VkiaMpsLfNtTKbAfFhRVB7wLgWwc858dEarZn6K1w=", "Natalia", "Kot", "45612378900", "+48550123456", 2 },
					{ 18, "jakub.stepien@pjwstk.edu.pl", "9KSiVzvollBIW96nZlfhUEfMxRpxaP8Du2gsx5l0xhQ=", "43VkiaMpsLfNtTKbAfFhRVB7wLgWwc858dEarZn6K1w=", "Jakub", "Stępień", "12345678090", "+48660112233", 2 },
					{ 19, "martyna.adamska@pjwstk.edu.pl", "9KSiVzvollBIW96nZlfhUEfMxRpxaP8Du2gsx5l0xhQ=", "43VkiaMpsLfNtTKbAfFhRVB7wLgWwc858dEarZn6K1w=", "Martyna", "Adamska", "78945612345", "+48559988444", 2 },
					{ 20, "tomasz.dudek@pjwstk.edu.pl", "9KSiVzvollBIW96nZlfhUEfMxRpxaP8Du2gsx5l0xhQ=", "43VkiaMpsLfNtTKbAfFhRVB7wLgWwc858dEarZn6K1w=", "Tomasz", "Dudek", "65432198765", "+48990044556", 2 },
					{ 21, "weronika.nowicka@pjwstk.edu.pl", "9KSiVzvollBIW96nZlfhUEfMxRpxaP8Du2gsx5l0xhQ=", "43VkiaMpsLfNtTKbAfFhRVB7wLgWwc858dEarZn6K1w=", "Weronika", "Nowicka", "32165498701", "+48661234567", 2 },
					{ 22, "mateusz.zajac@pjwstk.edu.pl", "9KSiVzvollBIW96nZlfhUEfMxRpxaP8Du2gsx5l0xhQ=", "43VkiaMpsLfNtTKbAfFhRVB7wLgWwc858dEarZn6K1w=", "Mateusz", "Zając", "76543218900", "+48881234123", 2 },
					{ 23, "patrycja.olszewska@pjwstk.edu.pl", "9KSiVzvollBIW96nZlfhUEfMxRpxaP8Du2gsx5l0xhQ=", "43VkiaMpsLfNtTKbAfFhRVB7wLgWwc858dEarZn6K1w=", "Patrycja", "Olszewska", "54321098765", "+48557733123", 2 },
					{ 24, "dawid.kaczynski@pjwstk.edu.pl", "9KSiVzvollBIW96nZlfhUEfMxRpxaP8Du2gsx5l0xhQ=", "43VkiaMpsLfNtTKbAfFhRVB7wLgWwc858dEarZn6K1w=", "Dawid", "Kaczyński", "10928374655", "+48778999002", 2 },
					{ 25, "paulina.mazurek@pjwstk.edu.pl", "9KSiVzvollBIW96nZlfhUEfMxRpxaP8Du2gsx5l0xhQ=", "43VkiaMpsLfNtTKbAfFhRVB7wLgWwc858dEarZn6K1w=", "Paulina", "Mazurek", "10293847566", "+48445566778", 2 },
					{ 26, "karol.urbanski@pjwstk.edu.pl", "9KSiVzvollBIW96nZlfhUEfMxRpxaP8Du2gsx5l0xhQ=", "43VkiaMpsLfNtTKbAfFhRVB7wLgWwc858dEarZn6K1w=", "Karol", "Urbański", "90817263549", "+48700112233", 2 },
					{ 27, "dominika.rogowska@pjwstk.edu.pl", "9KSiVzvollBIW96nZlfhUEfMxRpxaP8Du2gsx5l0xhQ=", "43VkiaMpsLfNtTKbAfFhRVB7wLgWwc858dEarZn6K1w=", "Dominika", "Rogowska", "98712365409", "+48998877665", 2 },
					{ 28, "sebastian.michalski@pjwstk.edu.pl", "9KSiVzvollBIW96nZlfhUEfMxRpxaP8Du2gsx5l0xhQ=", "43VkiaMpsLfNtTKbAfFhRVB7wLgWwc858dEarZn6K1w=", "Sebastian", "Michalski", "87654321098", "+48221133445", 2 },
					{ 29, "julia.kaczmarczyk@pjwstk.edu.pl", "9KSiVzvollBIW96nZlfhUEfMxRpxaP8Du2gsx5l0xhQ=", "43VkiaMpsLfNtTKbAfFhRVB7wLgWwc858dEarZn6K1w=", "Julia", "Kaczmarczyk", "19283746501", "+48660055443", 2 },
					{ 30, "filip.komorowski@pjwstk.edu.pl", "9KSiVzvollBIW96nZlfhUEfMxRpxaP8Du2gsx5l0xhQ=", "43VkiaMpsLfNtTKbAfFhRVB7wLgWwc858dEarZn6K1w=", "Filip", "Komorowski", "83726194020", "+48889922112", 2 },
					{ 31, "klaudia.brzozowska@pjwstk.edu.pl", "9KSiVzvollBIW96nZlfhUEfMxRpxaP8Du2gsx5l0xhQ=", "43VkiaMpsLfNtTKbAfFhRVB7wLgWwc858dEarZn6K1w=", "Klaudia", "Brzozowska", "76123984561", "+48774455992", 2 },
					{ 32, "patryk.kowalczyk@pjwstk.edu.pl", "9KSiVzvollBIW96nZlfhUEfMxRpxaP8Du2gsx5l0xhQ=", "43VkiaMpsLfNtTKbAfFhRVB7wLgWwc858dEarZn6K1w=", "Patryk", "Kowalczyk", "67890123567", "+48660333221", 2 },
					{ 33, "amelia.kubiak@pjwstk.edu.pl", "9KSiVzvollBIW96nZlfhUEfMxRpxaP8Du2gsx5l0xhQ=", "43VkiaMpsLfNtTKbAfFhRVB7wLgWwc858dEarZn6K1w=", "Amelia", "Kubiak", "80917263547", "+48885444321", 2 },
					{ 34, "bartosz.wasilewski@pjwstk.edu.pl", "9KSiVzvollBIW96nZlfhUEfMxRpxaP8Du2gsx5l0xhQ=", "43VkiaMpsLfNtTKbAfFhRVB7wLgWwc858dEarZn6K1w=", "Bartosz", "Wasilewski", "91283746502", "+48770022113", 2 },
					{ 35, "jan.kowalski@pjwstk.edu.pl", "9KSiVzvollBIW96nZlfhUEfMxRpxaP8Du2gsx5l0xhQ=", "43VkiaMpsLfNtTKbAfFhRVB7wLgWwc858dEarZn6K1w=", "Jan", "Kowalski", "56395736192", "+48926384057", 1 },
					{ 36, "adam.kowalski@pjwstk.edu.pl", "9KSiVzvollBIW96nZlfhUEfMxRpxaP8Du2gsx5l0xhQ=", "43VkiaMpsLfNtTKbAfFhRVB7wLgWwc858dEarZn6K1w=", "Adam", "Kowalski", "46152430983", "+48123456780", 1 },
					{ 37, "michal.kowalski@pjwstk.edu.pl", "9KSiVzvollBIW96nZlfhUEfMxRpxaP8Du2gsx5l0xhQ=", "43VkiaMpsLfNtTKbAfFhRVB7wLgWwc858dEarZn6K1w=", "Michał", "Kowalski", "60937462539", "+48698714231", 1 }
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
				keyValues: new object[] { 1, 2, 3 });

			migrationBuilder.DeleteData(
				table: "RolePermissions",
				keyColumns: new[] { "RoleId", "PermissionId" },
				keyValues: new object[,]
				{
					{ 1, 2 },
					{ 1, 3 },
					{ 2, 1 },
					{ 2, 2 }
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
