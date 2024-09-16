using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bussiness_Manager.Migrations
{
    /// <inheritdoc />
    public partial class CreatetableTransactions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "unit",
                table: "products",
                type: "nvarchar(200)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "salePrice",
                table: "products",
                type: "Decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "Decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "qty",
                table: "products",
                type: "Decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "Decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "productName",
                table: "products",
                type: "nvarchar(500)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "hsnCode",
                table: "products",
                type: "nvarchar(50)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    transactionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    memberId = table.Column<int>(type: "int", nullable: false),
                    shopId = table.Column<int>(type: "int", nullable: false),
                    customerId = table.Column<int>(type: "int", nullable: false),
                    saleId = table.Column<int>(type: "int", nullable: false),
                    totalAmount = table.Column<decimal>(type: "decimal(18,3)", nullable: true),
                    discount = table.Column<decimal>(type: "decimal(18,3)", nullable: true),
                    netAmount = table.Column<decimal>(type: "decimal(18,3)", nullable: true),
                    balanceAmount = table.Column<decimal>(type: "decimal(18,3)", nullable: true),
                    dstatus = table.Column<string>(type: "nvarchar(2)", nullable: true),
                    createdOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    updatedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.transactionId);
                    table.ForeignKey(
                        name: "FK_Transactions_customers_customerId",
                        column: x => x.customerId,
                        principalTable: "customers",
                        principalColumn: "customerId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transactions_members_memberId",
                        column: x => x.memberId,
                        principalTable: "members",
                        principalColumn: "memberId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Transactions_saleInvoices_saleId",
                        column: x => x.saleId,
                        principalTable: "saleInvoices",
                        principalColumn: "saleId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transactions_shopDetails_shopId",
                        column: x => x.shopId,
                        principalTable: "shopDetails",
                        principalColumn: "shopId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_customerId",
                table: "Transactions",
                column: "customerId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_memberId",
                table: "Transactions",
                column: "memberId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_saleId",
                table: "Transactions",
                column: "saleId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_shopId",
                table: "Transactions",
                column: "shopId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.AlterColumn<string>(
                name: "unit",
                table: "products",
                type: "nvarchar(200)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)");

            migrationBuilder.AlterColumn<decimal>(
                name: "salePrice",
                table: "products",
                type: "Decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "Decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "qty",
                table: "products",
                type: "Decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "Decimal(18,2)");

            migrationBuilder.AlterColumn<string>(
                name: "productName",
                table: "products",
                type: "nvarchar(500)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)");

            migrationBuilder.AlterColumn<string>(
                name: "hsnCode",
                table: "products",
                type: "nvarchar(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)");
        }
    }
}
