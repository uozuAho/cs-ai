using System.Collections.Generic;

namespace pandemic
{
    public record PlayerState
    {
        public List<PlayerCard> Hand { get; set; } = new();
        public string Position { get; set; }

        public PlayerState(string postition)
        {
            Position = postition;
        }
    }

    public abstract record PlayerCard;

    public record PlayerCityCard : PlayerCard
    {
        public PlayerCityCard(string city)
        {
            City = city;
        }

        public string City { get; init; }
    }

    public record EpidemicCard : PlayerCard;
}