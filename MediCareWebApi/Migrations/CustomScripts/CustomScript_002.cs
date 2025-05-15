using Microsoft.EntityFrameworkCore.Migrations;

namespace MediCare.Migrations.CustomScripts
{
	public class CustomScript_002
	{
		public static void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.Sql(@"
				IF NOT EXISTS (SELECT * FROM sys.server_principals WHERE name = N'IIS APPPOOL\MediCareAppPool')
				BEGIN
				    CREATE LOGIN [IIS APPPOOL\MediCareAppPool] FROM WINDOWS;
				END

				USE MediCare;
				CREATE USER [IIS APPPOOL\MediCareAppPool] FOR LOGIN [IIS APPPOOL\MediCareAppPool];
				ALTER ROLE db_datareader ADD MEMBER [IIS APPPOOL\MediCareAppPool];
				ALTER ROLE db_datawriter ADD MEMBER [IIS APPPOOL\MediCareAppPool];
		    ");
		}

		public static void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.Sql(@"
				IF EXISTS (SELECT * FROM sys.server_principals WHERE name = N'IIS APPPOOL\MediCareAppPool')
				BEGIN
				    DROP LOGIN [IIS APPPOOL\MediCareAppPool];
				END

				USE MediCare;
				ALTER ROLE db_datareader DROP MEMBER [IIS APPPOOL\MediCareAppPool];
				ALTER ROLE db_datawriter DROP MEMBER [IIS APPPOOL\MediCareAppPool];
				DROP USER [IIS APPPOOL\MediCareAppPool];
			");
		}
	}
}
