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
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			CustomScript_002.Down(migrationBuilder);
			CustomScript_001.Down(migrationBuilder);
		}
	}
}
