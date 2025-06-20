using MangaApi.Models;
using Microsoft.EntityFrameworkCore;

namespace MangaApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Manga> Mangas { get; set; } = default!;
        public DbSet<Prestamo> Prestamos { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Manga>(entity =>
            {
                entity.ToTable("mangas");
            });

            modelBuilder.Entity<Prestamo>(entity =>
            {
                entity.ToTable("prestamo");

                entity.HasOne(d => d.Manga)
                    .WithMany()
                    .HasForeignKey(d => d.Manga_ID)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });
        }
    }
} 