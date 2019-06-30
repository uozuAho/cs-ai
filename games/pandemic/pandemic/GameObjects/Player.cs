namespace pandemic.GameObjects
{
    public class Player
    {
        public Character Character { get; }
        public City Location { get; set; }

        public Player(Character character)
        {
            Character = character;
        }
    }
}