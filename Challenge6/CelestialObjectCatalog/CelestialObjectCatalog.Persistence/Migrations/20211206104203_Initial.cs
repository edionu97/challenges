using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CelestialObjectCatalog.Persistence.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CelestialObjects",
                columns: table => new
                {
                    CelestialObjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Mass = table.Column<double>(type: "float", nullable: false),
                    EquatorialDiameter = table.Column<double>(type: "float", nullable: false),
                    SurfaceTemperature = table.Column<double>(type: "float", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CelestialObjects", x => x.CelestialObjectId);
                });

            migrationBuilder.CreateTable(
                name: "DiscoverySources",
                columns: table => new
                {
                    DiscoverySourceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EstablishmentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    StateOwner = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscoverySources", x => x.DiscoverySourceId);
                });

            migrationBuilder.CreateTable(
                name: "CelestialObjectDiscoveries",
                columns: table => new
                {
                    CelestialObjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DiscoverySourceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DiscoveryDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CelestialObjectDiscoveries", x => new { x.CelestialObjectId, x.DiscoverySourceId });
                    table.ForeignKey(
                        name: "FK_CelestialObjectDiscoveries_CelestialObjects_CelestialObjectId",
                        column: x => x.CelestialObjectId,
                        principalTable: "CelestialObjects",
                        principalColumn: "CelestialObjectId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CelestialObjectDiscoveries_DiscoverySources_DiscoverySourceId",
                        column: x => x.DiscoverySourceId,
                        principalTable: "DiscoverySources",
                        principalColumn: "DiscoverySourceId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CelestialObjectDiscoveries_DiscoverySourceId",
                table: "CelestialObjectDiscoveries",
                column: "DiscoverySourceId");

            migrationBuilder.CreateIndex(
                name: "IX_CelestialObjects_Name",
                table: "CelestialObjects",
                column: "Name")
                .Annotation("SqlServer:Clustered", false);

            migrationBuilder.CreateIndex(
                name: "IX_CelestialObjects_Type",
                table: "CelestialObjects",
                column: "Type")
                .Annotation("SqlServer:Clustered", false);

            migrationBuilder.CreateIndex(
                name: "IX_DiscoverySources_StateOwner",
                table: "DiscoverySources",
                column: "StateOwner")
                .Annotation("SqlServer:Clustered", false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CelestialObjectDiscoveries");

            migrationBuilder.DropTable(
                name: "CelestialObjects");

            migrationBuilder.DropTable(
                name: "DiscoverySources");
        }
    }
}
