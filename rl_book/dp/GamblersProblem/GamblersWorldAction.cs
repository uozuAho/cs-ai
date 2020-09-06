namespace dp.GamblersProblem
{
    public readonly struct GamblersWorldAction
    {
        public int Stake { get; }

        public GamblersWorldAction(int stake)
        {
            Stake = stake;
        }

        public bool Equals(GamblersWorldAction other)
        {
            return Stake == other.Stake;
        }

        public override bool Equals(object obj)
        {
            return obj is GamblersWorldAction other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Stake;
        }
    }
}