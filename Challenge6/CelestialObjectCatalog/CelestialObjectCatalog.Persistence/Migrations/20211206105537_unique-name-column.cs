using Microsoft.EntityFrameworkCore.Migrations;

namespace CelestialObjectCatalog.Persistence.Migrations
{
    public partial class uniquenamecolumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "DiscoverySources",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DiscoverySources_Name",
                table: "DiscoverySources",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_DiscoverySources_Name",
                table: "DiscoverySources");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "DiscoverySources",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}
