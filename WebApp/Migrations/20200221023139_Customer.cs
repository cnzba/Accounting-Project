using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApp.Migrations
{
    public partial class Customer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false),
                    StreetAddressOne = table.Column<string>(nullable: false),
                    StreetAddressTwo = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: false),
                    Country = table.Column<string>(nullable: false),
                    Phone = table.Column<string>(nullable: true),
                    ContactFirstName = table.Column<string>(nullable: false),
                    ContactLastName = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    InvoiceDeliveryContactFirstName = table.Column<string>(nullable: true),
                    InvoiceDeliveryContactLastName = table.Column<string>(nullable: true),
                    InvoiceDeliveryEmail = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    AccountID = table.Column<string>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    OrganisationId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Customer_Organisation_OrganisationId",
                        column: x => x.OrganisationId,
                        principalTable: "Organisation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Customer_OrganisationId",
                table: "Customer",
                column: "OrganisationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Customer");
        }
    }
}
