using System;
using System.Collections.Generic;
using System.Text;
using cgspamd.core.Models;
using Microsoft.EntityFrameworkCore;

namespace cgspamd.core.Contexts
{
    public class StoreDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<FilterRule> FilterRules { get; set; }
        private string fileName = "Store.sqlite";
        public StoreDbContext(DbContextOptions<StoreDbContext> options) : base(options)
        {
        }
        protected override void ConfigureConventions(
            ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<string>().UseCollation("NOCASE");
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite($"Data Source={fileName}");
            }
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.UserName).IsUnique();
                entity.Property(e => e.Enabled).IsRequired().HasDefaultValue(true);
                entity.Property(e => e.Hash).IsRequired().UseCollation("BINARY");
                entity.Property(e => e.FullName).IsRequired();
                entity.Property(e => e.TokenVersion).IsRequired().HasDefaultValue(Int32.MinValue);
                entity.Property(e => e.IsAdmin).IsRequired().HasDefaultValue(false);
                entity.Property(e => e.Deleted).IsRequired().HasDefaultValue(false);
            });
            modelBuilder.Entity<FilterRule>(entity =>
            {
                entity.ToTable("FilterRules");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Value).IsRequired();
                entity.Property(e => e.Comment).IsRequired().HasDefaultValue(String.Empty);
                entity.Property(e => e.Type).IsRequired();
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.Property(e => e.UpdatedAt);
                entity.HasOne(e => e.CreatedByUser).WithMany().HasForeignKey(e => e.CreatedByUserId).IsRequired();
                entity.HasOne(e => e.UpdatedByUser).WithMany().HasForeignKey(e => e.UpdatedByUserId);
            });
        }
    }
}
