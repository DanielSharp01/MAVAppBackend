using Microsoft.EntityFrameworkCore.Migrations;

namespace MAVAppBackend.Migrations
{
    public partial class normalizedname : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Stations_Name",
                table: "Stations");

            migrationBuilder.AddColumn<string>(
                name: "NormalizedName",
                table: "Stations",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Stations_NormalizedName",
                table: "Stations",
                column: "NormalizedName",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Stations_NormalizedName",
                table: "Stations");

            migrationBuilder.DropColumn(
                name: "NormalizedName",
                table: "Stations");

            migrationBuilder.CreateIndex(
                name: "IX_Stations_Name",
                table: "Stations",
                column: "Name",
                unique: true);
        }
    }
}
