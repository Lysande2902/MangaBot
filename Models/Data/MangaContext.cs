using MangaApi.Models;
using Microsoft.EntityFrameworkCore;

namespace MangaApi.Data
{
    public class MangaContext : DbContext
    {
        public MangaContext(DbContextOptions<MangaContext> options)
            : base(options) { }

        public DbSet<Manga> Mangas { get; set; }

        // Configuración del nombre de la tabla en minúsculas usando Fluent API
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Aquí aseguramos que la tabla se llame "mangas" en minúsculas
            modelBuilder.Entity<Manga>().ToTable("mangas");
        }
    }
}
