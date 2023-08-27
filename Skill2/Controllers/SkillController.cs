using Microsoft.AspNetCore.Mvc;
using Moserware.Skills;
using SkillsWebApp.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SkillsWebApp.Controllers
{
    [Serializable]
    public class SkillData
    {
        public SkillData() { }
        public string Player;
        public double Rating;
    }

    [Route("api/[controller]")]
    [ApiController]
    public class SkillController : ControllerBase
    {
        private static Mutex mutex = new Mutex();

        public SkillController()
        {
        }

        // PUT: api/Skill/5
        [HttpPut("update")]
        public IActionResult PutPlayerInt([FromBody] TeamListModel TeamList)
        {
            mutex.WaitOne();
            var gameInfo = GameInfo.DefaultGameInfo;
            List<Team> teamList = new List<Team>();
            List<int> positions = new List<int>();
            foreach (ResultTeam team in TeamList.TeamList)
            {
                var team1 = new Team();
                var playerCount = 0;
                foreach (string ID in team.PlayerIds)
                {
                    if(!string.IsNullOrEmpty(ID))
                    {
                        if (!team1.TeamPlayers.Any(p => p.Player.PlayfabId == ID))
                        {
                            var player = PlayersController.FindOrAddPlayer(ID);
                            team1.AddPlayer(player, player.Rating);
                            playerCount++;
                        }
                    }
                }
                if (playerCount >= 1)
                {
                    teamList.Add(team1);
                    positions.Add(team.Place);
                }
            }
            if(teamList.Count < 2)
            {
                return Ok();
            }
            var teams = Teams.Concat(teamList.ToArray());
            var newRatings = TrueSkillCalculator.CalculateNewRatings(gameInfo, teams, positions);

            var skillData = new List<SkillData>();
            foreach (KeyValuePair<Player, Rating> item in newRatings)
            {
                skillData.Add(new SkillData { Player = item.Key.PlayfabId, Rating = item.Key.Rating.ConservativeRating });
            }

            var returnData = skillData.ToArray();
            mutex.ReleaseMutex();
            return Ok(new { Players = newRatings.Keys});
        }
    }
}