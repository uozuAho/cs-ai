namespace dp.GamblersProblem
{
    public class UniformRandomGamblersPolicy : IGamblersPolicy, IPolicy<GamblersWorldState, GamblersWorldAction>
    {
        public double PAction(GamblersWorldState state, GamblersWorldAction action)
        {
            if (action.Stake > state.DollarsInHand) return 0.0;

            return 1.0 / (state.DollarsInHand + 1);
        }
    }
}