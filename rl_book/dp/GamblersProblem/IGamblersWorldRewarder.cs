namespace dp.GamblersProblem
{
    internal interface IGamblersWorldRewarder
    {
        double Reward(
            GamblersWorldState oldState,
            GamblersWorldState newState,
            GamblersWorldAction action);
    }
}