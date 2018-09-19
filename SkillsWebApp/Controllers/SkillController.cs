using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moserware.Skills;
using SkillsWebApp.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SkillsWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SkillController : ControllerBase
    {
        private readonly SkillContext _context;

        public SkillController(SkillContext context)
        {
            _context = context;
        }

        // PUT: api/Skill/5
        [HttpPut("update")]
        public async Task<IActionResult> PutPlayerInt([FromBody] TeamListModel TeamList)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var gameInfo = GameInfo.DefaultGameInfo;
            List<Team> teamList = new List<Team>();
            List<int> positions = new List<int>();
            foreach (ResultTeam team in TeamList.TeamList)
            {
                var team1 = new Team();

                foreach (string ID in team.PlayerIds)
                {
                    var player = await _context.Player.Include(e => e.Rating)
                                        .FirstOrDefaultAsync(e => e.PlayfabId == ID);
                    team1.AddPlayer(player, player.Rating);
                }
                teamList.Add(team1);
                positions.Add(team.Place);
            }
            var teams = Teams.Concat(teamList.ToArray());
            var newRatings = TrueSkillCalculator.CalculateNewRatings(gameInfo, teams, positions);
            foreach (KeyValuePair<Player, Rating> item in newRatings)
            {
                item.Key.Rating = item.Value;
                _context.Entry(item.Key).State = EntityState.Modified;
            }
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }
            return Ok(newRatings);
        }
    }
}