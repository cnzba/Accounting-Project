using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApp.Migrations
{
    public partial class AddOrganisationToUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrganisationId",
                table: "User",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_OrganisationId",
                table: "User",
                column: "OrganisationId");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Organisation_OrganisationId",
                table: "User",
                column: "OrganisationId",
                principalTable: "Organisation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Organisation_OrganisationId",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_OrganisationId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "OrganisationId",
                table: "User");
        }
    }
}
