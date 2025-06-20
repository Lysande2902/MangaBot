using MangaApi.Models;
using Microsoft.EntityFrameworkCore;

namespace MangaApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Manga> Mangas { get; set; }
        public DbSet<Prestamo> Prestamos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Manga>().ToTable("mangas");
            modelBuilder.Entity<Prestamo>().ToTable("prestamo");
        }
    }
} 