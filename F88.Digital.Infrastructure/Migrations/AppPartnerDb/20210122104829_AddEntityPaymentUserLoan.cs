using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace F88.Digital.Infrastructure.Migrations.AppPartnerDb
{
    public partial class AddEntityPaymentUserLoan : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TransactionId",
                table: "Payments");

            migrationBuilder.RenameColumn(
                name: "TransferStatus",
                table: "Payments",
                newName: "Status");

            migrationBuilder.CreateTable(
                name: "PaymentUserLoanReferral",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PaymentId = table.Column<int>(type: "int", nullable: false),
                    UserLoanReferralId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentUserLoanReferral", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentUserLoanReferral_Payments_PaymentId",
                        column: x => x.PaymentId,
                        principalTable: "Payments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PaymentUserLoanReferral_UserLoanReferrals_UserLoanReferralId",
                        column: x => x.UserLoanReferralId,
                        principalTable: "UserLoanReferrals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PaymentUserLoanReferral_PaymentId",
                table: "PaymentUserLoanReferral",
                column: "PaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentUserLoanReferral_UserLoanReferralId",
                table: "PaymentUserLoanReferral",
                column: "UserLoanReferralId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PaymentUserLoanReferral");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Payments",
                newName: "TransferStatus");

            migrationBuilder.AddColumn<int>(
                name: "TransactionId",
                table: "Payments",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
