namespace dp.GamblersProblem
{
    public interface IGamblersPolicy
    {
        double PAction(GamblersWorldState state, GamblersWorldAction action);
    }
}