using Moserware.Skills;

namespace SkillsWebApp.Data
{
    public class PlayerInt : Player
    {
        public Rating Rating { get; set; } = GameInfo.DefaultGameInfo.DefaultRating;
    }
}
