using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    public partial class EventGameRel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_EventGameRelations_BoardGameEventID",
                table: "EventGameRelations",
                column: "BoardGameEventID");

            migrationBuilder.AddForeignKey(
                name: "FK_EventGameRelations_Events_BoardGameEventID",
                table: "EventGameRelations",
                column: "BoardGameEventID",
                principalTable: "Events",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EventGameRelations_Games_BoardGameID",
                table: "EventGameRelations",
                column: "BoardGameID",
                principalTable: "Games",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventGameRelations_Events_BoardGameEventID",
                table: "EventGameRelations");

            migrationBuilder.DropForeignKey(
                name: "FK_EventGameRelations_Games_BoardGameID",
                table: "EventGameRelations");

            migrationBuilder.DropIndex(
                name: "IX_EventGameRelations_BoardGameEventID",
                table: "EventGameRelations");
        }
    }
}
