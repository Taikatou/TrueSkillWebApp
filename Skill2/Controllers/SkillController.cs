using Microsoft.AspNetCore.Mvc;
using Moserware.Skills;
using SkillsWebApp.Data;
using System.Collections.Generic;
using System.Text.RegularExpressions;
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
                var player = PlayersController.FindOrAddPlayer(item.Key.PlayfabId);
                player.Rating = item.Value;
            }


            foreach(var player in PlayersController.Players)
            {
                var file_name = $"{player.PlayfabId}";
                Regex rgx = new Regex("[^a-zA-Z0-9 -]");
                file_name = rgx.Replace(file_name, "") + "_results_2.csv";
                if (!System.IO.File.Exists(file_name))
                {
                    using (FileStream fs = System.IO.File.Create(file_name));
                }

                StreamWriter sw = System.IO.File.AppendText(file_name);
                sw.WriteLine(player.Rating.ConservativeRating);
                sw.Close();
            }
            System.Threading.Thread.Sleep(1000);

            var returnData = skillData.ToArray();
            mutex.ReleaseMutex();
            return Ok(new { Players = newRatings.Keys});
        }
    }
}