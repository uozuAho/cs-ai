namespace dp.GamblersProblem
{
    public interface IGamblersWorldRewarder
    {
        double Reward(
            GamblersWorldState oldState,
            GamblersWorldState newState,
            GamblersWorldAction action);
    }
}