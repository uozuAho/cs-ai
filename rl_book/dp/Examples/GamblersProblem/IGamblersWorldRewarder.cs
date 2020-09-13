namespace dp.Examples.GamblersProblem
{
    public interface IGamblersWorldRewarder
    {
        double Reward(
            GamblersWorldState oldState,
            GamblersWorldState newState,
            GamblersWorldAction action);
    }
}