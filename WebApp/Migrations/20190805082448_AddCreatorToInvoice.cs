using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApp.Migrations
{
    public partial class AddCreatorToInvoice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreatorId",
                table: "Invoice",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Invoice_CreatorId",
                table: "Invoice",
                column: "CreatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoice_User_CreatorId",
                table: "Invoice",
                column: "CreatorId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoice_User_CreatorId",
                table: "Invoice");

            migrationBuilder.DropIndex(
                name: "IX_Invoice_CreatorId",
                table: "Invoice");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "Invoice");
        }
    }
}
