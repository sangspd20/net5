using Microsoft.EntityFrameworkCore.Migrations;

namespace F88.Digital.Infrastructure.Migrations.AppPartnerDb
{
    public partial class Delete_ManyPayment_UserProfile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_UserLoanReferrals_UserLoanReferralId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_UserLoanReferralId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "UserLoanReferralId",
                table: "Payments");

            migrationBuilder.AddColumn<decimal>(
                name: "OtherAmount",
                table: "Payments",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TaxValue",
                table: "Payments",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TransactionHistorys_TransactionDate",
                table: "TransactionHistorys",
                column: "TransactionDate",
                unique: true)
                .Annotation("SqlServer:Clustered", false);

            migrationBuilder.CreateIndex(
                name: "IX_TransactionHistorys_TransactionType",
                table: "TransactionHistorys",
                column: "TransactionType",
                unique: true)
                .Annotation("SqlServer:Clustered", false);

            migrationBuilder.CreateIndex(
                name: "IX_TransactionHistorys_TransFinalStatus",
                table: "TransactionHistorys",
                column: "TransFinalStatus",
                unique: true)
                .Annotation("SqlServer:Clustered", false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TransactionHistorys_TransactionDate",
                table: "TransactionHistorys");

            migrationBuilder.DropIndex(
                name: "IX_TransactionHistorys_TransactionType",
                table: "TransactionHistorys");

            migrationBuilder.DropIndex(
                name: "IX_TransactionHistorys_TransFinalStatus",
                table: "TransactionHistorys");

            migrationBuilder.DropColumn(
                name: "OtherAmount",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "TaxValue",
                table: "Payments");

            migrationBuilder.AddColumn<int>(
                name: "UserLoanReferralId",
                table: "Payments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Payments_UserLoanReferralId",
                table: "Payments",
                column: "UserLoanReferralId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_UserLoanReferrals_UserLoanReferralId",
                table: "Payments",
                column: "UserLoanReferralId",
                principalTable: "UserLoanReferrals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
