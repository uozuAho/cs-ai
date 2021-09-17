using System.Collections.Generic;

namespace pandemic
{
    public record PlayerState
    {
        public List<PlayerCard> Hand { get; set; } = new List<PlayerCard>
        {
            new PlayerCard(),
            new PlayerCard(),
            new PlayerCard(),
            new PlayerCard(),
        };
    }

    public record PlayerCard
    {
    }
}