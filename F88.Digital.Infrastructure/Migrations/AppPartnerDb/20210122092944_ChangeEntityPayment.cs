using Microsoft.EntityFrameworkCore.Migrations;

namespace F88.Digital.Infrastructure.Migrations.AppPartnerDb
{
    public partial class ChangeEntityPayment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<string>(
                name: "TransactionId",
                table: "TransactionHistorys",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "TransSubTotal",
                table: "TransactionHistorys",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<int>(
                name: "TransFinalStatus",
                table: "TransactionHistorys",
                type: "int",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddColumn<int>(
                name: "TransactionId",
                table: "Payments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TransactionHistorys_TransactionDate",
                table: "TransactionHistorys",
                column: "TransactionDate")
                .Annotation("SqlServer:Clustered", false);

            migrationBuilder.CreateIndex(
                name: "IX_TransactionHistorys_TransactionType",
                table: "TransactionHistorys",
                column: "TransactionType")
                .Annotation("SqlServer:Clustered", false);

            migrationBuilder.CreateIndex(
                name: "IX_TransactionHistorys_TransFinalStatus",
                table: "TransactionHistorys",
                column: "TransFinalStatus")
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
                name: "TransactionId",
                table: "Payments");

            migrationBuilder.AlterColumn<int>(
                name: "TransactionId",
                table: "TransactionHistorys",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<decimal>(
                name: "TransSubTotal",
                table: "TransactionHistorys",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "TransFinalStatus",
                table: "TransactionHistorys",
                type: "bit",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

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
    }
}
