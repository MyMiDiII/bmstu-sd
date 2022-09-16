using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DataAccess.Migrations
{
    public partial class AddProducer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Producer",
                table: "Games");

            migrationBuilder.AddColumn<long>(
                name: "ProducerID",
                table: "Games",
                type: "bigint",
                nullable: false,
                defaultValue: 1L);

            migrationBuilder.CreateTable(
                name: "Producers",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    URL = table.Column<string>(type: "text", nullable: false),
                    Rating = table.Column<long>(type: "bigint", nullable: false),
                    Deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Producers", x => x.ID);
                });

            migrationBuilder.InsertData(
                table: "Producers",
                columns: new[] { "ID", "Deleted", "Name", "Rating", "URL" },
                values: new object[] { 1L, false, "base", 0L, "" });

            migrationBuilder.CreateIndex(
                name: "IX_Games_ProducerID",
                table: "Games",
                column: "ProducerID");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Producers_ProducerID",
                table: "Games",
                column: "ProducerID",
                principalTable: "Producers",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_Producers_ProducerID",
                table: "Games");

            migrationBuilder.DropTable(
                name: "Producers");

            migrationBuilder.DropIndex(
                name: "IX_Games_ProducerID",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "ProducerID",
                table: "Games");

            migrationBuilder.AddColumn<string>(
                name: "Producer",
                table: "Games",
                type: "text",
                nullable: true);
        }
    }
}
