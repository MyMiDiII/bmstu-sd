using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    public partial class BGEregtime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RegistrationTime",
                table: "Events");

            migrationBuilder.AddColumn<DateTime>(
                name: "BeginRegistration",
                table: "Events",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "EndRegistration",
                table: "Events",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BeginRegistration",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "EndRegistration",
                table: "Events");

            migrationBuilder.AddColumn<TimeOnly>(
                name: "RegistrationTime",
                table: "Events",
                type: "time without time zone",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));
        }
    }
}
