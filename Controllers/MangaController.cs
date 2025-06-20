using MangaApi.Data;
using MangaApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MangaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MangaController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MangaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/manga
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Manga>>> GetMangas()
        {
            return await _context.Mangas.ToListAsync();
        }

        // GET: api/manga/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Manga>> GetManga(int id)
        {
            var manga = await _context.Mangas.FindAsync(id);

            if (manga == null)
            {
                return NotFound();
            }

            return manga;
        }

        // POST: api/manga
        [HttpPost]
        public async Task<ActionResult<Manga>> PostManga(Manga manga)
        {
            // Verificar si ya existe un manga con el mismo título
            var mangaExistente = await _context.Mangas.FirstOrDefaultAsync(m => m.Titulo.ToLower() == manga.Titulo.ToLower());
            if (mangaExistente != null)
            {
                return Conflict(new { message = "Ya existe un manga con ese título." });
            }

            _context.Mangas.Add(manga);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetManga), new { id = manga.ID }, manga);
        }

        // PUT: api/manga/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutManga(int id, Manga manga)
        {
            if (id != manga.ID)
            {
                return BadRequest();
            }

            _context.Entry(manga).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MangaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/manga/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteManga(int id)
        {
            // Verificar si el manga tiene préstamos asociados
            var tienePrestamos = await _context.Prestamos.AnyAsync(p => p.Manga_ID == id);
            if (tienePrestamos)
            {
                return BadRequest(new { message = "No se puede eliminar el manga porque tiene préstamos asociados." });
            }

            var manga = await _context.Mangas.FindAsync(id);
            if (manga == null)
            {
                return NotFound();
            }

            _context.Mangas.Remove(manga);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MangaExists(int id)
        {
            return _context.Mangas.Any(e => e.ID == id);
        }
    }
}
