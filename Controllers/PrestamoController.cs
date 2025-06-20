using MangaApi.Data;
using MangaApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MangaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrestamoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PrestamoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/prestamo (con paginado)
        [HttpGet]
        public async Task<ActionResult> GetPrestamos(
            [FromQuery] int pagina = 1,
            [FromQuery] int registrosPorPagina = 10
        )
        {
            var totalRegistros = await _context.Prestamos.CountAsync();
            var totalPaginas = (int)Math.Ceiling((double)totalRegistros / registrosPorPagina);

            var prestamos = await _context
                .Prestamos.Include(p => p.Manga) // Incluye los detalles del manga relacionado
                .Skip((pagina - 1) * registrosPorPagina)
                .Take(registrosPorPagina)
                .ToListAsync();

            return Ok(
                new
                {
                    pagina_actual = pagina,
                    cantidad_registros = registrosPorPagina,
                    pagina_siguiente = pagina < totalPaginas ? pagina + 1 : (int?)null,
                    pagina_anterior = pagina > 1 ? pagina - 1 : (int?)null,
                    total_paginas = totalPaginas,
                    total_registros = totalRegistros,
                    prestamos,
                }
            );
        }

        // GET: api/prestamo/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Prestamo>> GetPrestamo(int id)
        {
            var prestamo = await _context
                .Prestamos.Include(p => p.Manga)
                .FirstOrDefaultAsync(p => p.ID == id);

            if (prestamo == null)
            {
                return NotFound();
            }

            return prestamo;
        }

        // POST: api/prestamo
        [HttpPost]
        public async Task<ActionResult<Prestamo>> PostPrestamo(Prestamo prestamo)
        {
            // Verificar si el manga que se quiere prestar existe
            var mangaExiste = await _context.Mangas.AnyAsync(m => m.ID == prestamo.Manga_ID);
            if (!mangaExiste)
            {
                return BadRequest(new { message = "El manga que intenta prestar no existe." });
            }

            _context.Prestamos.Add(prestamo);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetPrestamo),
                new { id = prestamo.ID },
                prestamo
            );
        }

        // PUT: api/prestamo/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPrestamo(int id, Prestamo prestamo)
        {
            if (id != prestamo.ID)
            {
                return BadRequest();
            }

            // Verificar si el manga al que se asocia el préstamo existe
            var mangaExiste = await _context.Mangas.AnyAsync(m => m.ID == prestamo.Manga_ID);
            if (!mangaExiste)
            {
                return BadRequest(new { message = "El manga que intenta asociar al préstamo no existe." });
            }

            _context.Entry(prestamo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PrestamoExists(id))
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

        // DELETE: api/prestamo/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePrestamo(int id)
        {
            var prestamo = await _context.Prestamos.FindAsync(id);
            if (prestamo == null)
            {
                return NotFound();
            }

            _context.Prestamos.Remove(prestamo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PrestamoExists(int id)
        {
            return _context.Prestamos.Any(e => e.ID == id);
        }
    }
}
