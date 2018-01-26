using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace WebApp.Migrations
{
    public partial class forcepasswordchange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Login",
                table: "User");

            migrationBuilder.DropColumn(
                name: "IssueeCareOf",
                table: "Invoice");

            migrationBuilder.DropColumn(
                name: "IssueeOrganization",
                table: "Invoice");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "User",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "ForcePasswordChange",
                table: "User",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ClientContactPerson",
                table: "Invoice",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClientName",
                table: "Invoice",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "User");

            migrationBuilder.DropColumn(
                name: "ForcePasswordChange",
                table: "User");

            migrationBuilder.DropColumn(
                name: "ClientContactPerson",
                table: "Invoice");

            migrationBuilder.DropColumn(
                name: "ClientName",
                table: "Invoice");

            migrationBuilder.AddColumn<string>(
                name: "Login",
                table: "User",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "IssueeCareOf",
                table: "Invoice",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IssueeOrganization",
                table: "Invoice",
                nullable: false,
                defaultValue: "");
        }
    }
}
