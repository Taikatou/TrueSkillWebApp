using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moserware.Skills;
using SkillsWebApp.Data;

namespace SkillsWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayersController : ControllerBase
    {
        private readonly SkillContext _context;

        public PlayersController(SkillContext context)
        {
            _context = context;
        }

        // GET: api/Player
        [HttpGet]
        public IEnumerable<PlayerInt> GetPlayers()
        {
            return _context.Players;
        }

        // GET: api/Player/5
        [HttpGet("rating/{id}")]
        public async Task<IActionResult> GetPlayerRating([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var playerInt = await _context.Players.FindAsync(id);

            if (playerInt == null)
            {
                return NotFound();
            }

            return Ok(playerInt.Rating);
        }

        // GET: api/Player/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPlayerInt([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var playerInt = await _context.Players.FindAsync(id);

            if (playerInt == null)
            {
                return NotFound();
            }

            return Ok(playerInt);
        }

        // PUT: api/Player/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPlayerInt([FromRoute] int id, [FromBody] PlayerInt playerInt)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != playerInt.Id)
            {
                return BadRequest();
            }

            _context.Entry(playerInt).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlayerIntExists(id))
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

        // POST: api/Player
        [HttpPost]
        public async Task<IActionResult> PostPlayerInt([FromBody] PlayerInt playerInt)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            playerInt.Rating = GameInfo.DefaultGameInfo.DefaultRating;
            _context.Players.Add(playerInt);
            //_context.Rating.Add(playerInt.Rating);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPlayerInt", new { id = playerInt.Id }, playerInt);
        }

        // DELETE: api/Player/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlayerInt([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var playerInt = await _context.Players.FindAsync(id);
            if (playerInt == null)
            {
                return NotFound();
            }

            _context.Players.Remove(playerInt);
            await _context.SaveChangesAsync();

            return Ok(playerInt);
        }

        private bool PlayerIntExists(int id)
        {
            return _context.Players.Any(e => e.Id == id);
        }
    }
}