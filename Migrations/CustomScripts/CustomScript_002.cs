using Microsoft.EntityFrameworkCore.Migrations;

namespace MediCare.Migrations.CustomScripts
{
	public class CustomScript_002
	{
		public static void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.Sql(@"
				CREATE TRIGGER GeneratePatientCard
				ON Patients
				AFTER INSERT
				AS
				BEGIN
				    SET NOCOUNT ON;

				    UPDATE p
				    SET p.PatientCard = CONCAT(
				        SUBSTRING(u.Pesel, 1, 6),
				        LEFT(CAST(CAST(GETDATE() AS BIGINT) AS VARCHAR), 14)
				    )
				    FROM Patients p
				    INNER JOIN inserted i ON p.Id = i.Id
				    INNER JOIN Users u ON u.Id = i.UserId;
				END;
		    ");
		}

		public static void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.Sql(@"DROP TRIGGER IF EXISTS GeneratePatientCard;");
		}
	}
}
