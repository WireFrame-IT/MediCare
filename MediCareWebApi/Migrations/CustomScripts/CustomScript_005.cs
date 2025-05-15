using Microsoft.EntityFrameworkCore.Migrations;

namespace MediCare.Migrations.CustomScripts
{
	public class CustomScript_005
	{
		public static void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.Sql(@"
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
			migrationBuilder.Sql(@"
				DROP PROCEDURE IF EXISTS UpdateAppointmentStatus;
			");
		}
	}
}
