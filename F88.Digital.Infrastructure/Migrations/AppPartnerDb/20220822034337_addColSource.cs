﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace F88.Digital.Infrastructure.Migrations.AppPartnerDb
{
    public partial class addColSource : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Source",
                table: "UserProfiles",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Source",
                table: "UserProfiles");
        }
    }
}
