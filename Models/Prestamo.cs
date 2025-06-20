using System.ComponentModel.DataAnnotations;

namespace MangaApi.Models
{
    public class Prestamo
    {
        [Key]
        public int ID { get; set; }
        public int Manga_ID { get; set; } // Clave foránea a la tabla 'mangas'
        public DateTime Fecha { get; set; }
        public required string Quien_presto { get; set; }

        // Propiedad de navegación (relación con Manga)
        public Manga? Manga { get; set; }
    }
}
