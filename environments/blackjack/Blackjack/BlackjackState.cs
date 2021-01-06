namespace Blackjack
{
    public class BlackjackState
    {
        public int[] PlayerCards { get; set; } = System.Array.Empty<int>();
        public bool IsPlayerStaying { get; }
    }
}