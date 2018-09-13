using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkillsWebApp.Data;

namespace SkillsWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingsController : ControllerBase
    {
        private readonly SkillContext _context;

        public RatingsController(SkillContext context)
        {
            _context = context;
        }

        // GET: api/RatingDBs
        [HttpGet]
        public IEnumerable<RatingDB> GetRating()
        {
            return _context.Rating;
        }

        // GET: api/RatingDBs/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRatingDB([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ratingDB = await _context.Rating.FindAsync(id);

            if (ratingDB == null)
            {
                return NotFound();
            }

            return Ok(ratingDB);
        }

        // PUT: api/RatingDBs/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRatingDB([FromRoute] int id, [FromBody] RatingDB ratingDB)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != ratingDB.Id)
            {
                return BadRequest();
            }

            _context.Entry(ratingDB).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RatingDBExists(id))
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

        // POST: api/RatingDBs
        [HttpPost]
        public async Task<IActionResult> PostRatingDB([FromBody] RatingDB ratingDB)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Rating.Add(ratingDB);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRatingDB", new { id = ratingDB.Id }, ratingDB);
        }

        // DELETE: api/RatingDBs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRatingDB([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ratingDB = await _context.Rating.FindAsync(id);
            if (ratingDB == null)
            {
                return NotFound();
            }

            _context.Rating.Remove(ratingDB);
            await _context.SaveChangesAsync();

            return Ok(ratingDB);
        }

        private bool RatingDBExists(int id)
        {
            return _context.Rating.Any(e => e.Id == id);
        }
    }
}