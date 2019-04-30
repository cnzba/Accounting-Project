using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace WebApp.Models
{
    public partial class CBAContext : DbContext
    {
        // Constuctor do not remove (DI)
        public CBAContext() { }
        public CBAContext(DbContextOptions<CBAContext> options) : base(options) { }

        public virtual DbSet<Invoice> Invoice { get; set; }
        public virtual DbSet<InvoiceLine> InvoiceLine { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<PaymentModel> Payment { get; set; }
        public virtual DbSet<Department> Department { get; set; }
        public virtual DbSet<TaxInfo> TaxInfo { get; set; }
        public virtual DbSet<Products> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // In order to keep this method simple, many items are configured by
            // convention and therefore not listed. Please review the EF Core docs
            // at https://docs.microsoft.com/en-us/ef/core/modeling/
            // e.g. properties named "Id" are treated as primary keys; therefore
            // primary keys have not been explicitly configured

            modelBuilder.Entity<Invoice>(entity =>
            {
                entity.HasAlternateKey(e => e.InvoiceNumber);

                entity.Property(e => e.DateCreated).IsRequired();
                entity.Property(e => e.DateDue).HasColumnType("date");

                entity.Property(e => e.InvoiceNumber).IsRequired();
                entity.Property(e => e.ClientName).IsRequired();

                entity.Property(e => e.GstNumber).IsRequired();
                entity.Property(e => e.CharitiesNumber).IsRequired();
                entity.Property(e => e.GstRate).HasColumnType("decimal(18,2)");
            });
            
            modelBuilder.Entity<InvoiceLine>(entity =>
            {
                entity.Property(e => e.Amount).IsRequired().HasColumnType("decimal(18,2)");
                // an invoice line cannot exist without an invoice: IsRequired gives cascade delete behaviour
                entity.Property("InvoiceId").IsRequired();
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Email).IsRequired();
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.Password).IsRequired();
            });

            modelBuilder.Entity<PaymentModel>(entity =>
            {
                entity.Property(e => e.Type).IsRequired();
                entity.Property(e => e.InvoiceNo).IsRequired();
                entity.Property(e => e.Status).IsRequired();
                entity.Property(e => e.Amount).IsRequired().HasColumnType("decimal(18,2)");
                entity.Property(e => e.PaymentId).IsRequired();
                entity.Property(e => e.paymentDate).IsRequired();
            });

            modelBuilder.Entity<Department>(entity =>
            {
                entity.Property(e => e.DepartmentId).IsRequired();
                entity.Property(e => e.DepartmentAddress).IsRequired();
                entity.Property(e => e.Deleted).IsRequired();
            });

            
            modelBuilder.Entity<TaxInfo>(entity =>
            {
                entity.Property(e => e.TaxId).IsRequired();
                entity.Property(e => e.TaxName).IsRequired();
                entity.Property(e => e.TaxValue).IsRequired();
            });

            modelBuilder.Entity<Products>(entity =>
            {
                    entity.Property(e => e.ProductId).IsRequired();
                    entity.Property(e => e.ProductName).IsRequired();
                    entity.Property(e => e.ProductDescription).IsRequired();
                    entity.Property(e => e.ProductPrice).HasColumnType("decimal(18,2)");
                    entity.Property(e => e.TaxId).IsRequired();
            });
           
        }
    }
}
