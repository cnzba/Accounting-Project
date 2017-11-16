using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace WebApp.Migrations
{
    public partial class gstrateitemorder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Gst",
                table: "Invoice");

            migrationBuilder.AddColumn<int>(
                name: "ItemOrder",
                table: "InvoiceLine",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "GstRate",
                table: "Invoice",
                type: "decimal(18, 2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ItemOrder",
                table: "InvoiceLine");

            migrationBuilder.DropColumn(
                name: "GstRate",
                table: "Invoice");

            migrationBuilder.AddColumn<decimal>(
                name: "Gst",
                table: "Invoice",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
