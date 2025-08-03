using Microsoft.EntityFrameworkCore;
using GPass.Models;

namespace GPass.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<CredentialGroup> CredentialGroups { get; set; } = null!;
        public DbSet<CredentialSet> CredentialSets { get; set; } = null!;
        public DbSet<CredentialBase> Credentials { get; set; } = null!;

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CredentialBase>()
                .HasDiscriminator<string>("Type")
                .HasValue<CredTitle>("Title")
                .HasValue<CredField>("Field")
                .HasValue<CredSecretField>("SecretField")
                .HasValue<CredLine>("Line");

            modelBuilder.Entity<CredentialGroup>()
                .HasMany(g => g.CredentialSets)
                .WithOne(s => s.Group)
                .HasForeignKey(s => s.GroupId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CredentialSet>()
                .HasMany(s => s.Credentials)
                .WithOne(c => c.Set)
                .HasForeignKey(c => c.SetId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
} 