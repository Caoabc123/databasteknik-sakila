using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sakila.Data;
using Sakila.Models;

namespace Sakila.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilmActorsController : ControllerBase
    {
        private readonly SakilaDbContext _context;

        public FilmActorsController(SakilaDbContext context)
        {
            _context = context;
        }

        // GET: api/FilmActors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FilmActor>>> GetFilmActors()
        {
            return await _context.FilmActors.ToListAsync();
        }

        // GET: api/FilmActors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FilmActor>> GetFilmActor(int id)
        {
            var filmActor = await _context.FilmActors.FindAsync(id);

            if (filmActor == null)
            {
                return NotFound();
            }

            return filmActor;
        }

        // PUT: api/FilmActors/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFilmActor(int id, FilmActor filmActor)
        {
            if (id != filmActor.FilmId)
            {
                return BadRequest();
            }

            _context.Entry(filmActor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FilmActorExists(id))
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

        // POST: api/FilmActors
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<FilmActor>> PostFilmActor(FilmActor filmActor)
        {
            _context.FilmActors.Add(filmActor);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (FilmActorExists(filmActor.FilmId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetFilmActor", new { id = filmActor.FilmId }, filmActor);
        }

        // DELETE: api/FilmActors/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<FilmActor>> DeleteFilmActor(int id)
        {
            var filmActor = await _context.FilmActors.FindAsync(id);
            if (filmActor == null)
            {
                return NotFound();
            }

            _context.FilmActors.Remove(filmActor);
            await _context.SaveChangesAsync();

            return filmActor;
        }

        private bool FilmActorExists(int id)
        {
            return _context.FilmActors.Any(e => e.FilmId == id);
        }
    }
}
