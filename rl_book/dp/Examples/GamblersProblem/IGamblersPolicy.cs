namespace dp.Examples.GamblersProblem
{
    public interface IGamblersPolicy
    {
        double PAction(GamblersWorldState state, GamblersWorldAction action);
    }
}