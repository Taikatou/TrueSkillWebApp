using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moserware.Skills;
using SkillsWebApp.Data;
using System.Collections.Generic;
using System.Linq;
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
        public async Task<IActionResult> PutUpdateSkill([FromBody] List<ResultTeam> TeamList)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var gameInfo = GameInfo.DefaultGameInfo;
            List<Team> teamList = new List<Team>();
            List<int> positions = new List<int>();
            List<PlayerInt> players = new List<PlayerInt>();
            foreach (ResultTeam team in TeamList)
            {
                var team1 = new Team();
                foreach(int ID in team.PlayerIds)
                {
                    var playerInt = await _context.Players.FindAsync(ID);
                    players.Add(playerInt);
                    team1.AddPlayer(playerInt, gameInfo.DefaultRating);
                }
                teamList.Add(team1);
                positions.Add(team.Place);
            }
            var teams = Teams.Concat(teamList.ToArray());
            var newRatings = TrueSkillCalculator.CalculateNewRatings(gameInfo, teams, positions);
            for(int i = 0; i < players.Count; i++)
            {
                players[i].Rating = newRatings[players[i]];
                _context.Entry(players[i]).State = EntityState.Modified;
            }
            try
            {
                await _context.SaveChangesAsync();
                return Ok(newRatings);
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }
        }

        private bool PlayerIntExists(int id)
        {
            return _context.Players.Any(e => e.Id == id);
        }
    }
}