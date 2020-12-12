namespace Blackjack
{
    public class BlackjackState
    {
        public int[] PlayerCards { get; set; } = new int[0];
        public int[] DealerCards { get; } = new int[0];
        public int DealerShowing => DealerCards[0];
        public bool IsPlayerStaying { get; }
    }
}