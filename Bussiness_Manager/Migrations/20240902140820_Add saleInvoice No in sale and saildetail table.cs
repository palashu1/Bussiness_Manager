using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bussiness_Manager.Migrations
{
    /// <inheritdoc />
    public partial class AddsaleInvoiceNoinsaleandsaildetailtable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "saleInvoiceNo",
                table: "saleInvoices",
                type: "nvarchar(50)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "saleInvoiceNo",
                table: "saleInvoiceDetails",
                type: "nvarchar(50)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "saleInvoiceNo",
                table: "saleInvoices");

            migrationBuilder.DropColumn(
                name: "saleInvoiceNo",
                table: "saleInvoiceDetails");
        }
    }
}
