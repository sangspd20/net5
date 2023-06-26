using Microsoft.EntityFrameworkCore.Migrations;

namespace F88.Digital.Infrastructure.Migrations.AppPartnerDb
{
    public partial class EditPaymentPaymentUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "PaymentUserLoanReferral",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "TransactionId",
                table: "PaymentUserLoanReferral",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserPhone",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "PaymentUserLoanReferral");

            migrationBuilder.DropColumn(
                name: "TransactionId",
                table: "PaymentUserLoanReferral");

            migrationBuilder.DropColumn(
                name: "UserPhone",
                table: "Payments");
        }
    }
}
