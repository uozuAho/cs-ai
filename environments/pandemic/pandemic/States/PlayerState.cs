using System.Collections.Generic;

namespace pandemic.States
{
    public record PlayerState
    {
        public List<PlayerCard> Hand { get; set; } = new();
    }

    public record PlayerCard
    {
    }
}