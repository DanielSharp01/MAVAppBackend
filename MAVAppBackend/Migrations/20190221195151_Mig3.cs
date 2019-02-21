using Microsoft.EntityFrameworkCore.Migrations;

namespace MAVAppBackend.Migrations
{
    public partial class Mig3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IntDistance",
                table: "TrainStations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Platform",
                table: "TrainStations",
                maxLength: 16,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FromId",
                table: "Trains",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Trains",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ToId",
                table: "Trains",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ViszNumber",
                table: "Trains",
                maxLength: 16,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Trains_FromId",
                table: "Trains",
                column: "FromId");

            migrationBuilder.CreateIndex(
                name: "IX_Trains_ToId",
                table: "Trains",
                column: "ToId");

            migrationBuilder.AddForeignKey(
                name: "FK_Trains_Stations_FromId",
                table: "Trains",
                column: "FromId",
                principalTable: "Stations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Trains_Stations_ToId",
                table: "Trains",
                column: "ToId",
                principalTable: "Stations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trains_Stations_FromId",
                table: "Trains");

            migrationBuilder.DropForeignKey(
                name: "FK_Trains_Stations_ToId",
                table: "Trains");

            migrationBuilder.DropIndex(
                name: "IX_Trains_FromId",
                table: "Trains");

            migrationBuilder.DropIndex(
                name: "IX_Trains_ToId",
                table: "Trains");

            migrationBuilder.DropColumn(
                name: "IntDistance",
                table: "TrainStations");

            migrationBuilder.DropColumn(
                name: "Platform",
                table: "TrainStations");

            migrationBuilder.DropColumn(
                name: "FromId",
                table: "Trains");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Trains");

            migrationBuilder.DropColumn(
                name: "ToId",
                table: "Trains");

            migrationBuilder.DropColumn(
                name: "ViszNumber",
                table: "Trains");
        }
    }
}
