namespace ailib.Algorithms.Search.NonDeterministic
{
    public interface INonDeterministicSearchProblem<TState, TAction>
    {
        bool IsGoal(TState state);
    }
}