using Microsoft.EntityFrameworkCore;


namespace WebApp.Models
{
    public partial class CBAOptionsContext : DbContext
    {
        // Constuctor do not remove (DI)
        public CBAOptionsContext(DbContextOptions<CBAOptionsContext> options)
            : base(options) { }

        public virtual DbSet<ConfigurationValue> ConfigurationValue { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ConfigurationValue>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(50)
                    .ValueGeneratedNever();

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasMaxLength(50);
            });
        }
        }
}
