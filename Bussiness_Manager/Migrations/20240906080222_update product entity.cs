using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bussiness_Manager.Migrations
{
    /// <inheritdoc />
    public partial class updateproductentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "productCategory",
                table: "products");

            migrationBuilder.RenameColumn(
                name: "productDescription",
                table: "products",
                newName: "productSummary");

            migrationBuilder.AddColumn<string>(
                name: "brand",
                table: "products",
                type: "nvarchar(200)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "color",
                table: "products",
                type: "nvarchar(100)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "hsnCode",
                table: "products",
                type: "nvarchar(50)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "productCode",
                table: "products",
                type: "nvarchar(200)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "size",
                table: "products",
                type: "Decimal(18,3)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "unit",
                table: "products",
                type: "nvarchar(200)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "dstatus",
                table: "customers",
                type: "nvarchar(2)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(2)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "brand",
                table: "products");

            migrationBuilder.DropColumn(
                name: "color",
                table: "products");

            migrationBuilder.DropColumn(
                name: "hsnCode",
                table: "products");

            migrationBuilder.DropColumn(
                name: "productCode",
                table: "products");

            migrationBuilder.DropColumn(
                name: "size",
                table: "products");

            migrationBuilder.DropColumn(
                name: "unit",
                table: "products");

            migrationBuilder.RenameColumn(
                name: "productSummary",
                table: "products",
                newName: "productDescription");

            migrationBuilder.AddColumn<string>(
                name: "productCategory",
                table: "products",
                type: "nvarchar(500)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "dstatus",
                table: "customers",
                type: "nvarchar(2)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(2)",
                oldNullable: true);
        }
    }
}
