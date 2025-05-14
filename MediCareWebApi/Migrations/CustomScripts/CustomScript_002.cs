using Microsoft.EntityFrameworkCore.Migrations;

namespace MediCare.Migrations.CustomScripts
{
	public class CustomScript_002
	{
		public static void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.Sql(@"
				USE MediCare;
				CREATE USER [IIS APPPOOL\MediCare AppPool] FOR LOGIN [IIS APPPOOL\MediCare AppPool];
				ALTER ROLE db_datareader ADD MEMBER [IIS APPPOOL\MediCare AppPool];
				ALTER ROLE db_datawriter ADD MEMBER [IIS APPPOOL\MediCare AppPool];

				GO

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

				GO

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

				GO

				CREATE PROCEDURE UpdateAppointmentStatus
				AS
				BEGIN
				    SET NOCOUNT ON;

				    UPDATE Appointments
				    SET status = 50
				    WHERE Time < DATEADD(MINUTE, -15, GETDATE());
				END;

				GO

				CREATE PROCEDURE DeleteOldLogsAndRebuildIndexes
				AS
				BEGIN
				    SET NOCOUNT ON;

				    DECLARE @RowsAffected INT = 1;

				    WHILE @RowsAffected > 0
				    BEGIN
				        DELETE TOP (1000)
					        FROM Logs
					        WHERE CreatedAt < DATEADD(DAY, -30, GETDATE());

				        SET @RowsAffected = @@ROWCOUNT;
				    END;

				    ALTER INDEX ALL ON Logs REBUILD WITH (ONLINE = ON);
				END;
		    ");
		}

		public static void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.Sql(@"
				DROP PROCEDURE IF EXISTS DeleteOldLogsAndRebuildIndexes;

				GO

				DROP PROCEDURE IF EXISTS UpdateAppointmentStatus;

				GO

				DROP TRIGGER IF EXISTS CheckOnlyDoctorOnlyPatientBits;

				GO

				DROP TRIGGER IF EXISTS GeneratePatientCard;

				GO

				USE MediCare;
				ALTER ROLE db_datareader DROP MEMBER [IIS APPPOOL\MediCare AppPool];
				ALTER ROLE db_datawriter DROP MEMBER [IIS APPPOOL\MediCare AppPool];
				DROP USER [IIS APPPOOL\MediCare AppPool];
			");
		}
	}
}
