using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    public partial class FavGamesRel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Favorites_PlayerID",
                table: "Favorites",
                column: "PlayerID");

            migrationBuilder.AddForeignKey(
                name: "FK_Favorites_Games_BoardGameID",
                table: "Favorites",
                column: "BoardGameID",
                principalTable: "Games",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Favorites_Players_PlayerID",
                table: "Favorites",
                column: "PlayerID",
                principalTable: "Players",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Favorites_Games_BoardGameID",
                table: "Favorites");

            migrationBuilder.DropForeignKey(
                name: "FK_Favorites_Players_PlayerID",
                table: "Favorites");

            migrationBuilder.DropIndex(
                name: "IX_Favorites_PlayerID",
                table: "Favorites");
        }
    }
}
