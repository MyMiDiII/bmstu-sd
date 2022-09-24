using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    public partial class PlayerRegs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Registrations_PlayerID",
                table: "Registrations",
                column: "PlayerID");

            migrationBuilder.AddForeignKey(
                name: "FK_Registrations_Events_BoardGameEventID",
                table: "Registrations",
                column: "BoardGameEventID",
                principalTable: "Events",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Registrations_Players_PlayerID",
                table: "Registrations",
                column: "PlayerID",
                principalTable: "Players",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Registrations_Events_BoardGameEventID",
                table: "Registrations");

            migrationBuilder.DropForeignKey(
                name: "FK_Registrations_Players_PlayerID",
                table: "Registrations");

            migrationBuilder.DropIndex(
                name: "IX_Registrations_PlayerID",
                table: "Registrations");
        }
    }
}
