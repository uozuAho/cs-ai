namespace ailib.Algorithms.Search.NonDeterministic
{
    public class NonDeterministicDfsSearch<TState, TAction> : INonDeterministicSearchAlgorithm<TState, TAction>
    {
        public NonDeterministicDfsSearch(INonDeterministicSearchProblem<TState, TAction> problem)
        {
        }

        public AndOrTree<TState, TAction> GetSolution()
        {
            return new AndOrTree<TState, TAction>(new OrNode<TAction>(default(TAction)));
        }
    }
}