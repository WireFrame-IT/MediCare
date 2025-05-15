using Microsoft.EntityFrameworkCore.Migrations;

namespace MediCare.Migrations.CustomScripts
{
	public class CustomScript_004
	{
		public static void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.Sql(@"
				CREATE TRIGGER CheckDoctorOnlyPatientOnlyBits
				ON Permissions
				AFTER INSERT
				AS
				BEGIN
				    IF EXISTS (SELECT 1 FROM inserted WHERE DoctorOnly = 1 AND PatientOnly = 1)
				    BEGIN
				        RAISERROR('Kolumny ""DoctorOnly"" i ""PatientOnly"" nie mogą być jednocześnie ustawione na 1.', 16, 1);
				        ROLLBACK TRANSACTION;
				    END
				END;

		    ");
		}

		public static void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.Sql(@"
				DROP TRIGGER IF EXISTS CheckOnlyDoctorOnlyPatientBits;
			");
		}
	}
}
