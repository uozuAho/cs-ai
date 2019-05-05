namespace ailib.Algorithms.Search.NonDeterministic
{
    public interface INonDeterministicSearchSolution<in TState, out TAction>
    {
        TAction NextAction(TState state);
    }
}