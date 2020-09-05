namespace dp.GamblersProblem
{
    internal class AlwaysStake1DollarPolicy : IGamblersPolicy
    {
        public double PAction(GamblersWorldState state, GamblersWorldAction action)
        {
            return action.Stake == 1 ? 1.0 : 0;
        }
    }
}