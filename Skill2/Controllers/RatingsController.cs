using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moserware.Skills;
using SkillsWebApp.Data;

namespace SkillsWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingsController : ControllerBase
    {
       /* // GET: api/Ratings
        [HttpGet]
        public IEnumerable<Rating> GetRating()
        {
            return Ratings;
        }

        // GET: api/Ratings/5
        [HttpGet("{id}")]
        public IActionResult GetRating([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var rating = Ratings.Find(r => r.Id == id);

            if (rating == null)
            {
                return NotFound();
            }

            return Ok(rating);
        }

        // PUT: api/Ratings/5
        [HttpPut("{id}")]
        public IActionResult PutRating([FromRoute] int id, [FromBody] Rating rating)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != rating.Id)
            {
                return BadRequest();
            }

            return NoContent();
        }

        // POST: api/Ratings
        [HttpPost]
        public IActionResult PostRating([FromBody] Rating rating)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return CreatedAtAction("GetRating", new { id = rating.Id }, rating);
        }

        // DELETE: api/Ratings/5
        [HttpDelete("{id}")]
        public IActionResult DeleteRating([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var rating = Ratings.Find(r => r.Id == id);
            if (rating == null)
            {
                return NotFound();
            }

            return Ok(rating);
        }

        private bool RatingExists(int id)
        {
            return Ratings.Any(e => e.Id == id);
        }*/
    }
}