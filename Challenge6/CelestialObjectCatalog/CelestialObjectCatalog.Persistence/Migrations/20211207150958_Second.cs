using Microsoft.EntityFrameworkCore.Migrations;

namespace CelestialObjectCatalog.Persistence.Migrations
{
    public partial class Second : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CelestialObjects_Name",
                table: "CelestialObjects");

            migrationBuilder.CreateIndex(
                name: "IX_CelestialObjects_Name",
                table: "CelestialObjects",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL")
                .Annotation("SqlServer:Clustered", false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CelestialObjects_Name",
                table: "CelestialObjects");

            migrationBuilder.CreateIndex(
                name: "IX_CelestialObjects_Name",
                table: "CelestialObjects",
                column: "Name")
                .Annotation("SqlServer:Clustered", false);
        }
    }
}
