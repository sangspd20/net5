using Microsoft.EntityFrameworkCore.Migrations;

namespace F88.Digital.Infrastructure.Migrations.AppPartnerDb
{
    public partial class AddColumn_Icon_Bank : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Notifications_NotiType",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "NotiType",
                table: "Notifications");

            migrationBuilder.AddColumn<bool>(
                name: "IsRead",
                table: "UserNotification",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "NotiTypeCode",
                table: "Notifications",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_NotiTypeCode",
                table: "Notifications",
                column: "NotiTypeCode")
                .Annotation("SqlServer:Clustered", false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Notifications_NotiTypeCode",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "IsRead",
                table: "UserNotification");

            migrationBuilder.DropColumn(
                name: "NotiTypeCode",
                table: "Notifications");

            migrationBuilder.AddColumn<int>(
                name: "NotiType",
                table: "Notifications",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_NotiType",
                table: "Notifications",
                column: "NotiType")
                .Annotation("SqlServer:Clustered", false);
        }
    }
}
