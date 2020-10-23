namespace Blackjack
{
    public class BlackjackState
    {
        public int[] PlayerCards { get; set; }
        public int[] DealerCards { get; }
        public int DealerShowing => DealerCards[0];
        public bool IsPlayerStaying { get; }
    }
}