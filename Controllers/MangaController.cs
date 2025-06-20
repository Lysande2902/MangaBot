using MangaApi.Data;
using MangaApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MangaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MangaController : ControllerBase
    {
        private readonly MangaContext _context;

        public MangaController(MangaContext context)
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
