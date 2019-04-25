using System.Linq;

namespace ailib.Algorithms.Search.NonDeterministic
{
    public class NonDeterministicDfsSearch<TState, TAction> : INonDeterministicSearchAlgorithm<TState, TAction>
    {
        private readonly INonDeterministicSearchProblem<TState, TAction> _problem;
        private readonly TState _initialState;

        public NonDeterministicDfsSearch(INonDeterministicSearchProblem<TState, TAction> problem, TState initialState)
        {
            _problem = problem;
            _initialState = initialState;
        }

        public AndOrTree<TState, TAction> GetSolution()
        {
            var firstAction = _problem.GetActions(_initialState).First();
            
            return new AndOrTree<TState, TAction>(new OrNode<TAction>(firstAction));
        }
    }
}