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

					DECLARE @RandomString VARCHAR(14) = '';
					WHILE LEN(@RandomString) < 14
						BEGIN
							SET @RandomString = @RandomString + CAST(FLOOR(RAND() * 10) AS VARCHAR(1));
						END;

				    UPDATE p
				    SET p.PatientCard = LEFT(CONCAT(SUBSTRING(u.Pesel, 1, 6), @RandomString), 20)
				    FROM Patients p
				    INNER JOIN inserted i ON p.Id = i.Id
				    INNER JOIN Users u ON u.Id = i.UserId;
				END;

				GO

				CREATE PROCEDURE UpdateAppointmentStatus
				AS
				BEGIN
				    SET NOCOUNT ON;

				    UPDATE Appointments
				    SET status = 50
				    WHERE Time < DATEADD(MINUTE, -15, GETDATE());
				END;
		    ");
		}

		public static void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.Sql(@"DROP TRIGGER IF EXISTS GeneratePatientCard;");
		}
	}
}
