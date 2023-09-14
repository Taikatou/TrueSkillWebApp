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

        public static int counter = 0;

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
                        var splitName = ID.Split("_");
                        if (splitName.Length == 2 && splitName[1] == "-1.0")
                        {
                            var learningPlayerId = ID.Split("?")[0] + "?team=0";
                            var learningPlayer = PlayersController.FindOrAddPlayer(learningPlayerId);
                            var rating = new Rating(learningPlayer.Rating.Mean, learningPlayer.Rating.StandardDeviation);
                            var player = new Player
                            {
                                PlayfabId = ID,
                                Rating = rating
                            };
                            team1.AddPlayer(player, player.Rating);
                        }
                        else if (!team1.TeamPlayers.Any(p => p.Player.PlayfabId == ID))
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

            if(counter >= 50)
            {
                foreach (var character in PlayersController.CharacterData)
                {
                    var file_name = character.Key + "_results_2.csv";
                    var sw = new StreamWriter(file_name);
                    foreach(var player in character.Value)
                    {
                        sw.Write(player.PlayfabId + "," + player.Rating.ConservativeRating + "," + player.Rating.StandardDeviation + "," + player.Rating.Mean + "\n");
                    }
                    sw.Close();
                }
                counter = 0;
            }
            else
            {
                counter++;
            }
            
            var returnData = skillData.ToArray();
            mutex.ReleaseMutex();
            return Ok(new { Players = newRatings.Keys});
        }
    }
}