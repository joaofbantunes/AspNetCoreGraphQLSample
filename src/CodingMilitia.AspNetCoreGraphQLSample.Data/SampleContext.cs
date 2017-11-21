using CodingMilitia.AspNetCoreGraphQLSample.Data.Model;
using Microsoft.EntityFrameworkCore;

namespace CodingMilitia.AspNetCoreGraphQLSample.Data
{
    public class SampleContext : DbContext
    {
        public DbSet<Box> Boxes { get; set; }
        public DbSet<Content> Contents { get; set; }

        public SampleContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("public");

            modelBuilder.Entity<Box>()
                .HasKey(e => e.Id);

            modelBuilder.Entity<Box>()
                .Property(e => e.Id)
                .UseNpgsqlSerialColumn();

            modelBuilder.Entity<Content>()
                .HasKey(e => e.Id);

            modelBuilder.Entity<Content>()
                .Property(e => e.Id)
                .UseNpgsqlSerialColumn();

            modelBuilder.Entity<Content>()
                .HasOne(p => p.Box)
                .WithMany(b => b.Contents);
        }
    }
}