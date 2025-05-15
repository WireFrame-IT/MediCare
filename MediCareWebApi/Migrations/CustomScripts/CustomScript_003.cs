using Microsoft.EntityFrameworkCore.Migrations;

namespace MediCare.Migrations.CustomScripts
{
	public class CustomScript_003
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

					DECLARE @RandomString VARCHAR(14) = '';
					WHILE LEN(@RandomString) < 14
						BEGIN
							SET @RandomString = @RandomString + CAST(FLOOR(RAND() * 10) AS VARCHAR(1));
						END;

				    UPDATE p
				    SET p.PatientCard = LEFT(CONCAT(SUBSTRING(u.Pesel, 1, 6), @RandomString), 20)
				    FROM Patients p
				    INNER JOIN inserted i ON p.UserId = i.UserId
				    INNER JOIN Users u ON u.Id = i.UserId;
				END;
		    ");
		}

		public static void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.Sql(@"
				DROP TRIGGER IF EXISTS GeneratePatientCard;
			");
		}
	}
}
