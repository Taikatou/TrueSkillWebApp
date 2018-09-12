using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> PutPlayerInt([FromBody] List<ResultTeam> TeamList)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var gameInfo = GameInfo.DefaultGameInfo;
            List<Team> teamList = new List<Team>();
            List<int> positions = new List<int>();
            foreach (ResultTeam team in TeamList)
            {
                var team1 = new Team();
                foreach(int ID in team.PlayerIds)
                {
                    var playerInt = await _context.Players.FindAsync(ID);
                    team1.AddPlayer(playerInt, gameInfo.DefaultRating);
                }
                teamList.Add(team1);
                positions.Add(team.Place);
            }
            var teams = Teams.Concat(teamList.ToArray());
            var newRatings = TrueSkillCalculator.CalculateNewRatings(gameInfo, teams, positions);

            return Ok(newRatings);
        }
    }
}