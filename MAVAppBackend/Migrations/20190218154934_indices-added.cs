using Microsoft.EntityFrameworkCore.Migrations;

namespace MAVAppBackend.Migrations
{
    public partial class indicesadded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<int>(
                name: "TrainId",
                table: "TrainInstances",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TrainStations_TrainId_StationId",
                table: "TrainStations",
                columns: new[] { "TrainId", "StationId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Trains_TrainNumber",
                table: "Trains",
                column: "TrainNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TrainInstances_ElviraId",
                table: "TrainInstances",
                column: "ElviraId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TrainInstances_TrainId",
                table: "TrainInstances",
                column: "TrainId");

            migrationBuilder.CreateIndex(
                name: "IX_Stations_Name",
                table: "Stations",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TrainInstances_Trains_TrainId",
                table: "TrainInstances",
                column: "TrainId",
                principalTable: "Trains",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrainInstances_Trains_TrainId",
                table: "TrainInstances");

            migrationBuilder.DropIndex(
                name: "IX_TrainStations_TrainId_StationId",
                table: "TrainStations");

            migrationBuilder.DropIndex(
                name: "IX_Trains_TrainNumber",
                table: "Trains");

            migrationBuilder.DropIndex(
                name: "IX_TrainInstances_ElviraId",
                table: "TrainInstances");

            migrationBuilder.DropIndex(
                name: "IX_TrainInstances_TrainId",
                table: "TrainInstances");

            migrationBuilder.DropIndex(
                name: "IX_Stations_Name",
                table: "Stations");

            migrationBuilder.DropColumn(
                name: "TrainId",
                table: "TrainInstances");
        }
    }
}
