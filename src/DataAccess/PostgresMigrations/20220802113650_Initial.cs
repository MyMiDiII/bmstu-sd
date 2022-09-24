using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DataAccess.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EventGameRelations",
                columns: table => new
                {
                    BoardGameID = table.Column<long>(type: "bigint", nullable: false),
                    BoardGameEventID = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventGameRelations", x => new { x.BoardGameID, x.BoardGameEventID });
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    StartTime = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    Duration = table.Column<long>(type: "bigint", nullable: false),
                    Cost = table.Column<long>(type: "bigint", nullable: false),
                    Purchase = table.Column<bool>(type: "boolean", nullable: false),
                    RegistrationTime = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    State = table.Column<int>(type: "integer", nullable: false),
                    OrganizerID = table.Column<long>(type: "bigint", nullable: false),
                    VenueID = table.Column<long>(type: "bigint", nullable: false),
                    Deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Favorites",
                columns: table => new
                {
                    BoardGameID = table.Column<long>(type: "bigint", nullable: false),
                    PlayerID = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Favorites", x => new { x.BoardGameID, x.PlayerID });
                });

            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Produser = table.Column<string>(type: "text", nullable: true),
                    Year = table.Column<long>(type: "bigint", nullable: false),
                    MaxAge = table.Column<long>(type: "bigint", nullable: false),
                    MinAge = table.Column<long>(type: "bigint", nullable: false),
                    MaxPlayerNum = table.Column<long>(type: "bigint", nullable: false),
                    MinPlayerNum = table.Column<long>(type: "bigint", nullable: false),
                    MaxDuration = table.Column<long>(type: "bigint", nullable: false),
                    MinDuration = table.Column<long>(type: "bigint", nullable: false),
                    Deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Organizers",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Address = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: true),
                    URL = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    Deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organizers", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    League = table.Column<string>(type: "text", nullable: false),
                    Rating = table.Column<long>(type: "bigint", nullable: false),
                    Deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Registrations",
                columns: table => new
                {
                    PlayerID = table.Column<long>(type: "bigint", nullable: false),
                    BoardGameEventID = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Registrations", x => new { x.BoardGameEventID, x.PlayerID });
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleName = table.Column<string>(type: "text", nullable: false),
                    RoleID = table.Column<long>(type: "bigint", nullable: false),
                    UserID = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Venues",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    Address = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: true),
                    URL = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    Deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Venues", x => x.ID);
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "ID", "RoleID", "RoleName", "UserID" },
                values: new object[] { 1L, 0L, "guest", 1L });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "ID", "Name", "Password" },
                values: new object[] { 1L, "guest", "guest" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventGameRelations");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "Favorites");

            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropTable(
                name: "Organizers");

            migrationBuilder.DropTable(
                name: "Players");

            migrationBuilder.DropTable(
                name: "Registrations");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Venues");
        }
    }
}
