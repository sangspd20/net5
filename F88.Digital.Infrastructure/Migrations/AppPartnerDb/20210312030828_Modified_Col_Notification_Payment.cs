using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace F88.Digital.Infrastructure.Migrations.AppPartnerDb
{
    public partial class Modified_Col_Notification_Payment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserNotification_NotiEndDate",
                table: "UserNotification");

            migrationBuilder.DropIndex(
                name: "IX_UserNotification_NotiStartDate",
                table: "UserNotification");

            migrationBuilder.DropColumn(
                name: "NotiEndDate",
                table: "UserNotification");

            migrationBuilder.DropColumn(
                name: "NotiStartDate",
                table: "UserNotification");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Payments",
                type: "int",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddColumn<int>(
                name: "Frequency",
                table: "Notifications",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsCampaign",
                table: "Notifications",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Frequency",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "IsCampaign",
                table: "Notifications");

            migrationBuilder.AddColumn<DateTime>(
                name: "NotiEndDate",
                table: "UserNotification",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "NotiStartDate",
                table: "UserNotification",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "Status",
                table: "Payments",
                type: "bit",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_UserNotification_NotiEndDate",
                table: "UserNotification",
                column: "NotiEndDate")
                .Annotation("SqlServer:Clustered", false);

            migrationBuilder.CreateIndex(
                name: "IX_UserNotification_NotiStartDate",
                table: "UserNotification",
                column: "NotiStartDate")
                .Annotation("SqlServer:Clustered", false);
        }
    }
}
