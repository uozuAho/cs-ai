using System;

namespace ailib.Algorithms.Search.NonDeterministic
{
    public class NonDeterministicDfsSearch<TState, TAction> : INonDeterministicSearchAlgorithm<TState, TAction>
    {
        private readonly INonDeterministicSearchProblem<TState, TAction> _problem;
        private readonly TState _initialState;
        private INonDeterministicSearchSolution<TState, TAction> _solution;

        public NonDeterministicDfsSearch(INonDeterministicSearchProblem<TState, TAction> problem, TState initialState)
        {
            _problem = problem;
            _initialState = initialState;
            Solve();
        }

        public INonDeterministicSearchSolution<TState, TAction> GetSolution()
        {
            if (_solution == null) throw new InvalidOperationException("No solution");

            return _solution;
        }

        private void Solve()
        {
            _solution = MyNonDetermDfsSearch<TState, TAction>.Search(_problem, _initialState);
        }
    }
}