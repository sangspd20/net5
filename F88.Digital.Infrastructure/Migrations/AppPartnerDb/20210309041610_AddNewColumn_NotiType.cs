using Microsoft.EntityFrameworkCore.Migrations;

namespace F88.Digital.Infrastructure.Migrations.AppPartnerDb
{
    public partial class AddNewColumn_NotiType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NotiType",
                table: "Notifications",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NotiType",
                table: "Notifications");
        }
    }
}
