﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;
using WebApp.Models;

namespace WebApp.Migrations
{
    [DbContext(typeof(CBAContext))]
    partial class CBAContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.0-rtm-26452")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("WebApp.Models.Invoice", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CharitiesNumber")
                        .IsRequired();

                    b.Property<string>("ClientContact");

                    b.Property<string>("ClientContactPerson");

                    b.Property<string>("ClientName")
                        .IsRequired();

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("date");

                    b.Property<DateTime>("DateDue")
                        .HasColumnType("date");

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("GstNumber")
                        .IsRequired();

                    b.Property<decimal>("GstRate");

                    b.Property<string>("InvoiceNumber")
                        .IsRequired();

                    b.Property<string>("PaymentId");

                    b.Property<int>("Status");

                    b.HasKey("Id");

                    b.HasAlternateKey("InvoiceNumber");

                    b.ToTable("Invoice");
                });

            modelBuilder.Entity("WebApp.Models.InvoiceLine", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<decimal>("Amount");

                    b.Property<string>("Description");

                    b.Property<int?>("InvoiceId")
                        .IsRequired();

                    b.Property<int>("ItemOrder");

                    b.HasKey("Id");

                    b.HasIndex("InvoiceId");

                    b.ToTable("InvoiceLine");
                });

            modelBuilder.Entity("WebApp.Models.PaymentModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<decimal>("Amount");

                    b.Property<string>("Charge");

                    b.Property<string>("ChargeId");

                    b.Property<string>("Gateway");

                    b.Property<string>("InvoiceNo")
                        .IsRequired();

                    b.Property<string>("PaymentId")
                        .IsRequired();

                    b.Property<string>("Status")
                        .IsRequired();

                    b.Property<string>("Token");

                    b.Property<string>("TokenId");

                    b.Property<string>("Type")
                        .IsRequired();

                    b.Property<DateTime>("paymentDate");

                    b.HasKey("Id");

                    b.ToTable("Payment");
                });

            modelBuilder.Entity("WebApp.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<bool>("ForcePasswordChange");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("Password")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("User");
                });

            modelBuilder.Entity("WebApp.Models.InvoiceLine", b =>
                {
                    b.HasOne("WebApp.Models.Invoice", "Invoice")
                        .WithMany("InvoiceLine")
                        .HasForeignKey("InvoiceId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
