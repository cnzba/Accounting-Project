using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace WebApp.Migrations
{
    public partial class PaymentNew : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RefId",
                table: "Payment");

            migrationBuilder.AddColumn<string>(
                name: "PaymentId",
                table: "Payment",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "paymentDate",
                table: "Payment",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Invoice",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PaymentId",
                table: "Invoice",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentId",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "paymentDate",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Invoice");

            migrationBuilder.DropColumn(
                name: "PaymentId",
                table: "Invoice");

            migrationBuilder.AddColumn<string>(
                name: "RefId",
                table: "Payment",
                nullable: false,
                defaultValue: "");
        }
    }
}
