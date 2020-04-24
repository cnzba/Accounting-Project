using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApp.Migrations
{
    public partial class ForgetPasswordToken : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ForgetPasswordToken",
                table: "User",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ForgetPasswordTokenGenerateDateTme",
                table: "User",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ForgetPasswordToken",
                table: "User");

            migrationBuilder.DropColumn(
                name: "ForgetPasswordTokenGenerateDateTme",
                table: "User");
        }
    }
}
