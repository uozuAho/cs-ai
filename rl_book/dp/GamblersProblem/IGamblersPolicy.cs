namespace dp.GamblersProblem
{
    internal interface IGamblersPolicy
    {
        double PAction(GamblersWorldState state, GamblersWorldAction action);
    }
}