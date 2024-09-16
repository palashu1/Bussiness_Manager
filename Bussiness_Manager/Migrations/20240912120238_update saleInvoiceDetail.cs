using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bussiness_Manager.Migrations
{
    /// <inheritdoc />
    public partial class updatesaleInvoiceDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_customers_customerId",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_members_memberId",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_saleInvoices_saleId",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_shopDetails_shopId",
                table: "Transactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Transactions",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "saleInvoiceNo",
                table: "saleInvoiceDetails");

            migrationBuilder.RenameTable(
                name: "Transactions",
                newName: "transactions");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_shopId",
                table: "transactions",
                newName: "IX_transactions_shopId");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_saleId",
                table: "transactions",
                newName: "IX_transactions_saleId");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_memberId",
                table: "transactions",
                newName: "IX_transactions_memberId");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_customerId",
                table: "transactions",
                newName: "IX_transactions_customerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_transactions",
                table: "transactions",
                column: "transactionId");

            migrationBuilder.AddForeignKey(
                name: "FK_transactions_customers_customerId",
                table: "transactions",
                column: "customerId",
                principalTable: "customers",
                principalColumn: "customerId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_transactions_members_memberId",
                table: "transactions",
                column: "memberId",
                principalTable: "members",
                principalColumn: "memberId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_transactions_saleInvoices_saleId",
                table: "transactions",
                column: "saleId",
                principalTable: "saleInvoices",
                principalColumn: "saleId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_transactions_shopDetails_shopId",
                table: "transactions",
                column: "shopId",
                principalTable: "shopDetails",
                principalColumn: "shopId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_transactions_customers_customerId",
                table: "transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_transactions_members_memberId",
                table: "transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_transactions_saleInvoices_saleId",
                table: "transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_transactions_shopDetails_shopId",
                table: "transactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_transactions",
                table: "transactions");

            migrationBuilder.RenameTable(
                name: "transactions",
                newName: "Transactions");

            migrationBuilder.RenameIndex(
                name: "IX_transactions_shopId",
                table: "Transactions",
                newName: "IX_Transactions_shopId");

            migrationBuilder.RenameIndex(
                name: "IX_transactions_saleId",
                table: "Transactions",
                newName: "IX_Transactions_saleId");

            migrationBuilder.RenameIndex(
                name: "IX_transactions_memberId",
                table: "Transactions",
                newName: "IX_Transactions_memberId");

            migrationBuilder.RenameIndex(
                name: "IX_transactions_customerId",
                table: "Transactions",
                newName: "IX_Transactions_customerId");

            migrationBuilder.AddColumn<string>(
                name: "saleInvoiceNo",
                table: "saleInvoiceDetails",
                type: "nvarchar(50)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Transactions",
                table: "Transactions",
                column: "transactionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_customers_customerId",
                table: "Transactions",
                column: "customerId",
                principalTable: "customers",
                principalColumn: "customerId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_members_memberId",
                table: "Transactions",
                column: "memberId",
                principalTable: "members",
                principalColumn: "memberId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_saleInvoices_saleId",
                table: "Transactions",
                column: "saleId",
                principalTable: "saleInvoices",
                principalColumn: "saleId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_shopDetails_shopId",
                table: "Transactions",
                column: "shopId",
                principalTable: "shopDetails",
                principalColumn: "shopId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
