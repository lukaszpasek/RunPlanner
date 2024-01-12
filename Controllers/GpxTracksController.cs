using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RunPlanner.Models;

namespace RunPlanner.Controllers
{
    [ApiController]
    [Route("api/gpxtracks")]
    public class GpxTracksController : ControllerBase
    {
        private readonly GpxDbContext _context;

        public GpxTracksController(GpxDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetGpxTracks()
        {
            var tracks = await _context.GpxTracks.ToListAsync();
            return Ok(tracks);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetGpxTrack(int id)
        {
            var track = await _context.GpxTracks.FindAsync(id);

            if (track == null)
                return NotFound();

            return Ok(track);
        }

        [HttpPost]
        public async Task<IActionResult> CreateGpxTrack([FromBody] GpxTrack track)
        {
            _context.GpxTracks.Add(track);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetGpxTrack), new { id = track.Id }, track);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGpxTrack(int id, [FromBody] GpxTrack track)
        {
            if (id != track.Id)
                return BadRequest();

            _context.Entry(track).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GpxTrackExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGpxTrack(int id)
        {
            var track = await _context.GpxTracks.FindAsync(id);

            if (track == null)
                return NotFound();

            _context.GpxTracks.Remove(track);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GpxTrackExists(int id)
        {
            return _context.GpxTracks.Any(e => e.Id == id);
        }
    }

}
