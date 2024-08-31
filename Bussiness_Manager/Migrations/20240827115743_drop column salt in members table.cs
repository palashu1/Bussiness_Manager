using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bussiness_Manager.Migrations
{
    /// <inheritdoc />
    public partial class dropcolumnsaltinmemberstable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "salt",
                table: "members");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updatedOn",
                table: "shopDetails",
                type: "DateTime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "DateTime");

            migrationBuilder.AlterColumn<string>(
                name: "logo",
                table: "shopDetails",
                type: "nvarchar(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)");

            migrationBuilder.AlterColumn<string>(
                name: "dstatus",
                table: "shopDetails",
                type: "nvarchar(2)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(2)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "createdOn",
                table: "shopDetails",
                type: "DateTime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "DateTime");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "updatedOn",
                table: "shopDetails",
                type: "DateTime",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "DateTime",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "logo",
                table: "shopDetails",
                type: "nvarchar(100)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "dstatus",
                table: "shopDetails",
                type: "nvarchar(2)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "createdOn",
                table: "shopDetails",
                type: "DateTime",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "DateTime",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "salt",
                table: "members",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
