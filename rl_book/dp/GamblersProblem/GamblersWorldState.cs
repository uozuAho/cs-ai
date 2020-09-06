namespace dp.GamblersProblem
{
    public readonly struct GamblersWorldState
    {
        public int DollarsInHand { get; }

        public GamblersWorldState(int dollarsInHand)
        {
            DollarsInHand = dollarsInHand;
        }
    }
}