using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace WebApp.Models
{
    public partial class CBAWEBACCOUNTContext : DbContext
    {

        //Constuctor do not remove (DI)
        public CBAWEBACCOUNTContext (DbContextOptions<CBAWEBACCOUNTContext> options)
            : base(options)     { }


        public virtual DbSet<Customers> Customers { get; set; }
        public virtual DbSet<Products> Products { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        // Conexion String is inside appsettings.json
        
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    ////            if (!optionsBuilder.IsConfigured)
        //    ////            {
        //    ////#warning To protect potentially sensitive information in your connection string, you should move it out of source code. 
        //    ////                See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
        //    ////                        optionsBuilder.UseSqlServer(@"server=cbasqlserver.database.windows.net;initial catalog=CBAWEBACCOUNT;persist security info=True;user id=Developer;password=1231!#ASDF!a");
        //    ////            }
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customers>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.BirthDate).HasColumnType("datetime");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Products>(entity =>
            {
                entity.HasKey(e => e.ProductId);

                entity.Property(e => e.ProductId).HasColumnName("Product ID");

                entity.Property(e => e.ProductName).HasMaxLength(50);
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasKey(e => e.IdUser);

                entity.Property(e => e.Login)
                    .IsRequired()
                    .HasColumnName("login")
                    .HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(50);
            });
        }
    }
}
