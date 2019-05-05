namespace ailib.Algorithms.Search.NonDeterministic
{
    public interface INonDeterministicSearchAlgorithm<in TState, out TAction>
    {
        INonDeterministicSearchSolution<TState, TAction> GetSolution();
    }
}