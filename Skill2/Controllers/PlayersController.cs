using Microsoft.AspNetCore.Mvc;
using Moserware.Skills;

namespace SkillsWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayersController : ControllerBase
    {
        public static Dictionary<string, List<Player>> CharacterData = new Dictionary<string, List<Player>>();
        public static List<Player> Players = new List<Player>();
        public PlayersController()
        {
        }

        // GET: api/Players
        [HttpGet]
        public IEnumerable<Player> GetPlayer()
        {
            return Players;
        }

        // GET: api/Players/5
        [HttpGet("{id}")]
        public IActionResult GetPlayer([FromRoute] int id)
        {
            var player = Players.Find(e => e.Id == id);

            if (player == null)
            {
                return NotFound();
            }

            return Ok(player);
        }

        [HttpGet("skill/{id}")]
        public IActionResult GetSkillPlayer([FromRoute] string PlayfabId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var player = Players.Find(e => e.PlayfabId == PlayfabId);

            if (player == null)
            {
                return NotFound();
            }

            return Ok(player.Rating.Mean);
        }

        // GET: api/Players/5
        [HttpGet("playfab/{id}")]
        public IActionResult GetPlayfabPlayer([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var player = Players.Find(e => e.PlayfabId == id);

            if (player == null)
            {
                return NotFound();
            }

            return Ok(player);
        }

        // PUT: api/Players/5
        [HttpPut("playfab")]
        public IActionResult PutPlayerPlayfab([FromBody] Player player)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            bool playerExists = PlayerExistsPlayfab(player.PlayfabId);
            if (!playerExists)
            {
                return PutPlayer(player);
            }
            var playerStored = Players.Find(e => e.PlayfabId == player.PlayfabId);
            return Ok(playerStored);
        }

        // PUT: api/Players/5
        [HttpPut]
        public IActionResult PutPlayer([FromBody] Player player)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!PlayerExistsPlayfab(player.PlayfabId))
            {
                return PostPlayer(player);
            }

            return NoContent();
        }

        // POST: api/Players
        [HttpPost]
        public IActionResult PostPlayer([FromBody] Player player)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            player.Rating = GameInfo.DefaultGameInfo.DefaultRating;
            Players.Add(player);

            return CreatedAtAction("GetPlayer", new { id = player.Id }, player);
        }

        // DELETE: api/Players/5
        [HttpDelete("{id}")]
        public IActionResult DeletePlayer([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var player = Players.Find(p => p.Id == id); ;
            if (player == null)
            {
                return NotFound();
            }

            Players.Remove(player);

            return Ok(player);
        }

        private bool PlayerExists(int id)
        {
            return Players.Any(e => e.Id == id);
        }

        private static bool PlayerExistsPlayfab(string PlayfabId)
        {
            return Players.Any(e => e.PlayfabId == PlayfabId);
        }

        public static Player FindOrAddPlayer(string PlayfabId)
        {
            if(!PlayerExistsPlayfab(PlayfabId))
            {
                Player previousPlayer = null;
                var characterName = PlayfabId.Split("?")[0];
                for (var i = Players.Count-1; i >= 0 && previousPlayer == null; i--)
                {
                    var playerNameSplit = Players[i].PlayfabId.Split("?");
                    var name = playerNameSplit[0];
                    var team = playerNameSplit[1].Split('=')[1];
                    if(name == characterName && team != "0")
                    {
                        previousPlayer = Players[i];
                    }
                }

                var rating = GameInfo.DefaultGameInfo.DefaultRating;
                if (previousPlayer != null)
                {
                    rating = new Rating(previousPlayer.Rating.Mean, previousPlayer.Rating.StandardDeviation);
                }
                var player = new Player
                {
                    PlayfabId = PlayfabId,
                    Rating = rating
                };
                if(!CharacterData.ContainsKey(characterName))
                {
                    CharacterData.Add(characterName, new List<Player>());
                }
                CharacterData[characterName].Add(player);
                
                Players.Add(player);
                return player;
            }
            return Players.Find(p => p.PlayfabId == PlayfabId);
        }
    }
}