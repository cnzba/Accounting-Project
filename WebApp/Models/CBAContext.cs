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
            });
            
            modelBuilder.Entity<InvoiceLine>(entity =>
            {
                entity.Property(e => e.Amount).IsRequired();
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
                entity.Property(e => e.Amount).IsRequired();
                entity.Property(e => e.PaymentId).IsRequired();
                entity.Property(e => e.paymentDate).IsRequired();
            });
        }
    }
}
