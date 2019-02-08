using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MAVAppBackend.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Stations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 255, nullable: false),
                    Latitude = table.Column<double>(nullable: true),
                    Longitude = table.Column<double>(nullable: true),
                    ExpiryDate = table.Column<DateTime>(nullable: false),
                    FullKnowledge = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TrainInstances",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ElviraId = table.Column<string>(maxLength: 16, nullable: false),
                    Latitude = table.Column<double>(nullable: false),
                    Longitude = table.Column<double>(nullable: false),
                    Delay = table.Column<int>(nullable: false),
                    State = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainInstances", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Trains",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TrainNumber = table.Column<int>(nullable: false),
                    Type = table.Column<int>(nullable: true),
                    EncodedPolyline = table.Column<string>(nullable: true),
                    ExpiryDate = table.Column<DateTime>(nullable: false),
                    FullKnowledge = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trains", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TrainStationLinks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TrainId = table.Column<int>(nullable: false),
                    FromId = table.Column<int>(nullable: true),
                    ToId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainStationLinks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrainStationLinks_Stations_FromId",
                        column: x => x.FromId,
                        principalTable: "Stations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrainStationLinks_Stations_ToId",
                        column: x => x.ToId,
                        principalTable: "Stations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrainStationLinks_Trains_TrainId",
                        column: x => x.TrainId,
                        principalTable: "Trains",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrainStations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TrainId = table.Column<int>(nullable: false),
                    StationId = table.Column<int>(nullable: false),
                    Distance = table.Column<double>(nullable: true),
                    Arrival = table.Column<TimeSpan>(nullable: true),
                    Departure = table.Column<TimeSpan>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainStations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrainStations_Stations_StationId",
                        column: x => x.StationId,
                        principalTable: "Stations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TrainStations_Trains_TrainId",
                        column: x => x.TrainId,
                        principalTable: "Trains",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TrainStationLinks_FromId",
                table: "TrainStationLinks",
                column: "FromId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainStationLinks_ToId",
                table: "TrainStationLinks",
                column: "ToId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainStationLinks_TrainId",
                table: "TrainStationLinks",
                column: "TrainId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainStations_StationId",
                table: "TrainStations",
                column: "StationId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainStations_TrainId",
                table: "TrainStations",
                column: "TrainId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrainInstances");

            migrationBuilder.DropTable(
                name: "TrainStationLinks");

            migrationBuilder.DropTable(
                name: "TrainStations");

            migrationBuilder.DropTable(
                name: "Stations");

            migrationBuilder.DropTable(
                name: "Trains");
        }
    }
}
