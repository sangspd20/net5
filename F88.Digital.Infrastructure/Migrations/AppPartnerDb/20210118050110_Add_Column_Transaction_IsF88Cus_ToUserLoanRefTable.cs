using Microsoft.EntityFrameworkCore.Migrations;

namespace F88.Digital.Infrastructure.Migrations.AppPartnerDb
{
    public partial class Add_Column_Transaction_IsF88Cus_ToUserLoanRefTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsF88Cus",
                table: "UserLoanReferrals",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "TransactionId",
                table: "UserLoanReferrals",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsF88Cus",
                table: "UserLoanReferrals");

            migrationBuilder.DropColumn(
                name: "TransactionId",
                table: "UserLoanReferrals");
        }
    }
}
