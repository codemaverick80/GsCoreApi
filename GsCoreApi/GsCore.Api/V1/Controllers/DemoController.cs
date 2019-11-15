using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GsCore.Database.Entities;

namespace GsCore.Api.V1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DemoController : ControllerBase
    {
        private readonly GsDbContext _context;

        public DemoController(GsDbContext context)
        {
            _context = context;
        }

        // GET: api/Demo
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Artist>>> GetArtist()
        {
            return await _context.Artist.ToListAsync();
        }

        // GET: api/Demo/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Artist>> GetArtist(int id)
        {
            var artist = await _context.Artist.FindAsync(id);

            if (artist == null)
            {
                return NotFound();
            }

            return artist;
        }

        // PUT: api/Demo/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutArtist(int id, Artist artist)
        {
            if (id != artist.Id)
            {
                return BadRequest();
            }

            _context.Entry(artist).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArtistExists(id))
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

        // POST: api/Demo
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Artist>> PostArtist(Artist artist)
        {
            _context.Artist.Add(artist);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ArtistExists(artist.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetArtist", new { id = artist.Id }, artist);
        }

        // DELETE: api/Demo/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Artist>> DeleteArtist(int id)
        {
            var artist = await _context.Artist.FindAsync(id);
            if (artist == null)
            {
                return NotFound();
            }

            _context.Artist.Remove(artist);
            await _context.SaveChangesAsync();

            return artist;
        }

        private bool ArtistExists(int id)
        {
            return _context.Artist.Any(e => e.Id == id);
        }
    }
}
