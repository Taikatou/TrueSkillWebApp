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

        // GET: api/Players
        [HttpGet]
        public IEnumerable<Player> GetPlayer()
        {
            return _context.Player;
        }

        // GET: api/Players/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPlayer([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var player = await _context.Player.Include(e => e.Rating)
                               .FirstOrDefaultAsync(e => e.Id == id);

            if (player == null)
            {
                return NotFound();
            }

            return Ok(player);
        }

        [HttpGet("skill/{id}")]
        public async Task<IActionResult> GetSkillPlayer([FromRoute] string PlayfabId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var player = await _context.Player.Include(e => e.Rating)
                               .FirstOrDefaultAsync(e => e.PlayfabId == PlayfabId);

            if (player == null)
            {
                return NotFound();
            }

            return Ok(player.Rating.Mean);
        }

        // GET: api/Players/5
        [HttpGet("playfab/{id}")]
        public async Task<IActionResult> GetPlayfabPlayer([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var player = await _context.Player.Include(e => e.Rating)
                               .FirstOrDefaultAsync(e => e.PlayfabId == id);

            if (player == null)
            {
                return NotFound();
            }

            return Ok(player);
        }

        // PUT: api/Players/5
        [HttpPut("playfab")]
        public async Task<IActionResult> PutPlayerPlayfab([FromBody] Player player)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            bool playerExists = PlayerExistsPlayfab(player.PlayfabId);
            if (!playerExists)
            {
                return await PutPlayer(player);
            }
            var playerStored = await _context.Player.Include(e => e.Rating).FirstOrDefaultAsync(e => e.PlayfabId == player.PlayfabId);
            if (player.Id == playerStored.Id)
            {
                _context.Entry(player).State = EntityState.Modified;
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    playerExists = PlayerExistsPlayfab(player.PlayfabId);
                    if (playerExists)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            else
            {
                return Ok(playerStored);
            }

            return NoContent();
        }

        // PUT: api/Players/5
        [HttpPut]
        public async Task<IActionResult> PutPlayer([FromBody] Player player)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!PlayerExistsPlayfab(player.PlayfabId))
            {
                return await PostPlayer(player);
            }

            _context.Entry(player).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlayerExistsPlayfab(player.PlayfabId))
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

        // POST: api/Players
        [HttpPost]
        public async Task<IActionResult> PostPlayer([FromBody] Player player)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            player.Rating = GameInfo.DefaultGameInfo.DefaultRating;
            _context.Player.Add(player);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPlayer", new { id = player.Id }, player);
        }

        // DELETE: api/Players/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlayer([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var player = await _context.Player.FindAsync(id);
            if (player == null)
            {
                return NotFound();
            }

            _context.Player.Remove(player);
            await _context.SaveChangesAsync();

            return Ok(player);
        }

        private bool PlayerExists(int id)
        {
            return _context.Player.Any(e => e.Id == id);
        }

        private bool PlayerExistsPlayfab(string PlayfabId)
        {
            return  _context.Player.Any(e => e.PlayfabId == PlayfabId);
        }
    }
}