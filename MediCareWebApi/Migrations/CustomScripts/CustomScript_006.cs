using Microsoft.EntityFrameworkCore.Migrations;

namespace MediCare.Migrations.CustomScripts
{
	public class CustomScript_006
	{
		public static void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.Sql(@"
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

				    ALTER INDEX ALL ON Logs REBUILD WITH (ONLINE = OFF);
				END;
		    ");
		}

		public static void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.Sql(@"
				DROP PROCEDURE IF EXISTS DeleteOldLogsAndRebuildIndexes;
			");
		}
	}
}
