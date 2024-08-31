using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bussiness_Manager.Migrations
{
    /// <inheritdoc />
    public partial class BussinessManger_Migration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "members",
                columns: table => new
                {
                    memberId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    firstName = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    lastName = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    email = table.Column<string>(type: "nvarchar(64)", nullable: false),
                    phone = table.Column<string>(type: "nvarchar(15)", nullable: false),
                    password = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    dstatus = table.Column<string>(type: "nvarchar(2)", nullable: false),
                    createdOn = table.Column<DateTime>(type: "DateTime", nullable: false),
                    updatedOn = table.Column<DateTime>(type: "DateTime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_members", x => x.memberId);
                });

            migrationBuilder.CreateTable(
                name: "shopDetails",
                columns: table => new
                {
                    shopId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    memberId = table.Column<int>(type: "int", nullable: false),
                    shopName = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    shopDescription = table.Column<string>(type: "nvarchar(500)", nullable: false),
                    bussinessType = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    logo = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    shopAddress = table.Column<string>(type: "nvarchar(500)", nullable: false),
                    dstatus = table.Column<string>(type: "nvarchar(2)", nullable: false),
                    createdOn = table.Column<DateTime>(type: "DateTime", nullable: false),
                    updatedOn = table.Column<DateTime>(type: "DateTime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_shopDetails", x => x.shopId);
                    table.ForeignKey(
                        name: "FK_shopDetails_members_memberId",
                        column: x => x.memberId,
                        principalTable: "members",
                        principalColumn: "memberId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "customers",
                columns: table => new
                {
                    customerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    memberId = table.Column<int>(type: "int", nullable: false),
                    shopId = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    mobileNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    dstatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    createdOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    updatedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_customers", x => x.customerId);
                    table.ForeignKey(
                        name: "FK_customers_members_memberId",
                        column: x => x.memberId,
                        principalTable: "members",
                        principalColumn: "memberId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_customers_shopDetails_shopId",
                        column: x => x.shopId,
                        principalTable: "shopDetails",
                        principalColumn: "shopId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "products",
                columns: table => new
                {
                    productId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    memberId = table.Column<int>(type: "int", nullable: false),
                    shopId = table.Column<int>(type: "int", nullable: false),
                    productName = table.Column<string>(type: "nvarchar(500)", nullable: false),
                    productDescription = table.Column<string>(type: "nvarchar(500)", nullable: true),
                    productCategory = table.Column<string>(type: "nvarchar(500)", nullable: true),
                    qty = table.Column<decimal>(type: "Decimal(18,2)", nullable: true),
                    salePrice = table.Column<decimal>(type: "Decimal(18,2)", nullable: true),
                    dstatus = table.Column<string>(type: "nvarchar(2)", nullable: false),
                    createdOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    updatedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_products", x => x.productId);
                    table.ForeignKey(
                        name: "FK_products_members_memberId",
                        column: x => x.memberId,
                        principalTable: "members",
                        principalColumn: "memberId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_products_shopDetails_shopId",
                        column: x => x.shopId,
                        principalTable: "shopDetails",
                        principalColumn: "shopId");
                });

            migrationBuilder.CreateTable(
                name: "saleInvoices",
                columns: table => new
                {
                    saleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    memberId = table.Column<int>(type: "int", nullable: false),
                    shopId = table.Column<int>(type: "int", nullable: false),
                    customerId = table.Column<int>(type: "int", nullable: false),
                    netAmount = table.Column<decimal>(type: "Decimal(18,2)", nullable: true),
                    discount = table.Column<decimal>(type: "Decimal(18,2)", nullable: true),
                    totalAmount = table.Column<decimal>(type: "Decimal(18,2)", nullable: true),
                    balanceAmount = table.Column<decimal>(type: "Decimal(18,2)", nullable: true),
                    dstatus = table.Column<string>(type: "nvarchar(2)", nullable: false),
                    createdOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    updatedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_saleInvoices", x => x.saleId);
                    table.ForeignKey(
                        name: "FK_saleInvoices_customers_customerId",
                        column: x => x.customerId,
                        principalTable: "customers",
                        principalColumn: "customerId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_saleInvoices_members_memberId",
                        column: x => x.memberId,
                        principalTable: "members",
                        principalColumn: "memberId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_saleInvoices_shopDetails_shopId",
                        column: x => x.shopId,
                        principalTable: "shopDetails",
                        principalColumn: "shopId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "saleInvoiceDetails",
                columns: table => new
                {
                    sdId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    saleId = table.Column<int>(type: "int", nullable: false),
                    productId = table.Column<int>(type: "int", nullable: false),
                    netAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    dstatus = table.Column<string>(type: "nvarchar(2)", nullable: false),
                    createdOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    updatedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_saleInvoiceDetails", x => x.sdId);
                    table.ForeignKey(
                        name: "FK_saleInvoiceDetails_products_productId",
                        column: x => x.productId,
                        principalTable: "products",
                        principalColumn: "productId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_saleInvoiceDetails_saleInvoices_saleId",
                        column: x => x.saleId,
                        principalTable: "saleInvoices",
                        principalColumn: "saleId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_customers_memberId",
                table: "customers",
                column: "memberId");

            migrationBuilder.CreateIndex(
                name: "IX_customers_shopId",
                table: "customers",
                column: "shopId");

            migrationBuilder.CreateIndex(
                name: "IX_products_memberId",
                table: "products",
                column: "memberId");

            migrationBuilder.CreateIndex(
                name: "IX_products_shopId",
                table: "products",
                column: "shopId");

            migrationBuilder.CreateIndex(
                name: "IX_saleInvoiceDetails_productId",
                table: "saleInvoiceDetails",
                column: "productId");

            migrationBuilder.CreateIndex(
                name: "IX_saleInvoiceDetails_saleId",
                table: "saleInvoiceDetails",
                column: "saleId");

            migrationBuilder.CreateIndex(
                name: "IX_saleInvoices_customerId",
                table: "saleInvoices",
                column: "customerId");

            migrationBuilder.CreateIndex(
                name: "IX_saleInvoices_memberId",
                table: "saleInvoices",
                column: "memberId");

            migrationBuilder.CreateIndex(
                name: "IX_saleInvoices_shopId",
                table: "saleInvoices",
                column: "shopId");

            migrationBuilder.CreateIndex(
                name: "IX_shopDetails_memberId",
                table: "shopDetails",
                column: "memberId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "saleInvoiceDetails");

            migrationBuilder.DropTable(
                name: "products");

            migrationBuilder.DropTable(
                name: "saleInvoices");

            migrationBuilder.DropTable(
                name: "customers");

            migrationBuilder.DropTable(
                name: "shopDetails");

            migrationBuilder.DropTable(
                name: "members");
        }
    }
}
