namespace Moserware.Skills
{
    /// <summary>
    /// Represents a player who has a <see cref="Rating"/>.
    /// </summary>
    public class Player<T> : ISupportPartialPlay, ISupportPartialUpdate
    {
        public Player()
        {
        }

        /// <summary>
        /// Constructs a player.
        /// </summary>
        /// <param name="id">The identifier for the player, such as a name.</param>
        public Player(T id)
            : this(id, 1.0, 1.0)
        {
        }

        /// <summary>
        /// Constructs a player.
        /// </summary>
        /// <param name="id">The identifier for the player, such as a name.</param>
        /// <param name="partialPlayPercentage">The weight percentage to give this player when calculating a new rank.</param>        
        public Player(T id, double partialPlayPercentage)
            : this(id, partialPlayPercentage, 1.0)
        {
        }

        /// <summary>
        /// Constructs a player.
        /// </summary>
        /// <param name="id">The identifier for the player, such as a name.</param>
        /// <param name="partialPlayPercentage">The weight percentage to give this player when calculating a new rank.</param>
        /// <param name="partialUpdatePercentage">/// Indicated how much of a skill update a player should receive where 0 represents no update and 1.0 represents 100% of the update.</param>
        public Player(T id, double partialPlayPercentage, double partialUpdatePercentage)
        {
            // If they don't want to give a player an id, that's ok...
            Guard.ArgumentInRangeInclusive(partialPlayPercentage, 0, 1.0, "partialPlayPercentage");
            Guard.ArgumentInRangeInclusive(partialUpdatePercentage, 0, 1.0, "partialUpdatePercentage");
            Id = id;
            PartialPlayPercentage = partialPlayPercentage;
            PartialUpdatePercentage = partialUpdatePercentage;
        }

        /// <summary>
        /// The identifier for the player, such as a name.
        /// </summary>
        public T Id { get; set; }

        public int? RatingId { get; set; }

        public Rating Rating { get; set; }

        public string DisplayName { get; set; }

        #region ISupportPartialPlay Members

        /// <summary>
        /// Indicates the percent of the time the player should be weighted where 0.0 indicates the player didn't play and 1.0 indicates the player played 100% of the time.
        /// </summary>

        private const double DEFAULT_VALUE = 1.0;

        public double PartialPlayPercentage { get; set; } = DEFAULT_VALUE;

        #endregion

        #region ISupportPartialUpdate Members

        /// <summary>
        /// Indicated how much of a skill update a player should receive where 0.0 represents no update and 1.0 represents 100% of the update.
        /// </summary>
        public double PartialUpdatePercentage { get; set; } = DEFAULT_VALUE;

        #endregion

        public override string ToString()
        {
            if (Id != null)
            {
                return Id.ToString();
            }

            return base.ToString();
        }
    }

    /// <summary>
    /// Represents a player who has a <see cref="Rating"/>.
    /// </summary>
    public class Player : Player<int>
    {
        public string PlayfabId { get; set; }
        /// <summary>
        /// Constructs a player.
        /// </summary>
        /// <param name="id">The identifier for the player, such as a name.</param>
        /// 
        public Player()
        {

        }
        public Player(int id)
            : base(id)
        {
        }

        /// <summary>
        /// Constructs a player.
        /// </summary>
        /// <param name="id">The identifier for the player, such as a name.</param>
        /// <param name="partialPlayPercentage">The weight percentage to give this player when calculating a new rank.</param>
        /// <param name="partialUpdatePercentage">Indicated how much of a skill update a player should receive where 0 represents no update and 1.0 represents 100% of the update.</param>
        public Player(int id, double partialPlayPercentage)
            : base(id, partialPlayPercentage)
        {
        }

        /// <summary>
        /// Constructs a player.
        /// </summary>
        /// <param name="id">The identifier for the player, such as a name.</param>
        /// <param name="partialPlayPercentage">The weight percentage to give this player when calculating a new rank.</param>
        /// <param name="partialUpdatePercentage">Indicated how much of a skill update a player should receive where 0 represents no update and 1.0 represents 100% of the update.</param>
        public Player(int id, double partialPlayPercentage, double partialUpdatePercentage)
            : base(id, partialPlayPercentage, partialUpdatePercentage)
        {
        }
    }
}