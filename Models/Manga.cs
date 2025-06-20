namespace MangaApi.Models
{
    public class Manga
    {
        public int ID { get; set; }
        public required string Titulo { get; set; }
        public required string Genero { get; set; }
        public required string Autor { get; set; }
        public int Anio_publicacion { get; set; }
        public int Volumenes { get; set; }
        public bool Sigue_en_emision { get; set; }
    }
}