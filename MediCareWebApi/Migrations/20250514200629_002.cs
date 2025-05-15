using MediCare.Migrations.CustomScripts;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediCare.Migrations
{
    /// <inheritdoc />
    public partial class _002 : Migration
    {
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			CustomScript_001.Up(migrationBuilder);
			CustomScript_002.Up(migrationBuilder);
			CustomScript_003.Up(migrationBuilder);
			CustomScript_004.Up(migrationBuilder);
			CustomScript_005.Up(migrationBuilder);
			CustomScript_006.Up(migrationBuilder);
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			CustomScript_006.Down(migrationBuilder);
			CustomScript_005.Down(migrationBuilder);
			CustomScript_004.Down(migrationBuilder);
			CustomScript_003.Down(migrationBuilder);
			CustomScript_002.Down(migrationBuilder);
			CustomScript_001.Down(migrationBuilder);
		}
	}
}
