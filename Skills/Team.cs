using System.Collections.Generic;
using System.Linq;

namespace Moserware.Skills
{
    /// <summary>
    /// Helper class for working with a single team.
    /// </summary>
    public class Team<TPlayer>
    {
        public int Id { get; set; }

        ICollection<TeamPlayer<TPlayer>> TeamPlayers { get; set; }

        /// <summary>
        /// Constructs a new team.
        /// </summary>
        public Team()
        {
            if (TeamPlayers == null)
            {
                TeamPlayers = new List<TeamPlayer<TPlayer>>();
            }
        }

        /// <summary>
        /// Constructs a <see cref="Team"/> and populates it with the specified <paramref name="player"/>.
        /// </summary>
        /// <param name="player">The player to add.</param>
        /// <param name="rating">The rating of the <paramref name="player"/>.</param>
        public Team(TPlayer player, Rating rating) : this()
        {
            AddPlayer(player, rating);
        }

        /// <summary>
        /// Adds the <paramref name="player"/> to the team.
        /// </summary>
        /// <param name="player">The player to add.</param>
        /// <param name="rating">The rating of the <paramref name="player"/>.</param>
        /// <returns>The instance of the team (for chaining convenience).</returns>
        public Team<TPlayer> AddPlayer(TPlayer player, Rating rating)
        {
            TeamPlayers.Add(new TeamPlayer<TPlayer> { Player = player, Rating = rating });
            return this;
        }

        public bool AddPlayers(ICollection<TeamPlayer<TPlayer>> players)
        {
            foreach(TeamPlayer<TPlayer> p in players)
            {
                TeamPlayers.Add(p);
            }
            return true;
        }

        /// <summary>
        /// Returns the <see cref="Team"/> as a simple dictionary.
        /// </summary>
        /// <returns>The <see cref="Team"/> as a simple dictionary.</returns>
        public IDictionary<TPlayer, Rating> AsDictionary()
        {
            Dictionary<TPlayer, Rating> PlayerRatings = new Dictionary<TPlayer, Rating>();
            foreach (TeamPlayer<TPlayer> p in TeamPlayers)
            {
                PlayerRatings.Add(p.Player, p.Rating);
            }
            return PlayerRatings;
        }
    }

    /// <summary>
    /// Helper class for working with a single team.
    /// </summary>
    public class Team : Team<Player>
    {
        /// <summary>
        /// Constructs a new team.
        /// </summary>
        public Team()
        {
        }

        /// <summary>
        /// Constructs a <see cref="Team"/> and populates it with the specified <paramref name="player"/>.
        /// </summary>
        /// <param name="player">The player to add.</param>
        /// <param name="rating">The rating of the <paramref name="player"/>.</param>
        public Team(Player player, Rating rating)
            : base(player, rating)
        {
        }
    }

    /// <summary>
    /// Helper class for working with multiple teams.
    /// </summary>
    public static class Teams
    {
        /// <summary>
        /// Concatenates multiple teams into a list of teams.
        /// </summary>
        /// <param name="teams">The teams to concatenate together.</param>
        /// <returns>A sequence of teams.</returns>
        public static IEnumerable<IDictionary<TPlayer, Rating>> Concat<TPlayer>(params Team<TPlayer>[] teams)
        {
            return teams.Select(t => t.AsDictionary());
        }
    }
}