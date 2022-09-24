using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    public partial class BGEOrgVenFKs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Events_OrganizerID",
                table: "Events",
                column: "OrganizerID");

            migrationBuilder.CreateIndex(
                name: "IX_Events_VenueID",
                table: "Events",
                column: "VenueID");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Organizers_OrganizerID",
                table: "Events",
                column: "OrganizerID",
                principalTable: "Organizers",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Venues_VenueID",
                table: "Events",
                column: "VenueID",
                principalTable: "Venues",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Organizers_OrganizerID",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_Events_Venues_VenueID",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Events_OrganizerID",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Events_VenueID",
                table: "Events");
        }
    }
}
