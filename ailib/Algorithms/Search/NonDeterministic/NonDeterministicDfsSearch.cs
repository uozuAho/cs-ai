using System;
using System.Collections.Generic;
using System.Linq;

namespace ailib.Algorithms.Search.NonDeterministic
{
    public class NonDeterministicDfsSearch<TState, TAction> : INonDeterministicSearchAlgorithm<TState, TAction>
    {
        private readonly INonDeterministicSearchProblem<TState, TAction> _problem;
        private readonly TState _initialState;
        private NonDeterministicSearchSolution<TState, TAction> _solution;

        public NonDeterministicDfsSearch(INonDeterministicSearchProblem<TState, TAction> problem, TState initialState)
        {
            _problem = problem;
            _initialState = initialState;
            Solve();
        }

        public NonDeterministicSearchSolution<TState, TAction> GetSolution()
        {
            if (_solution == null) throw new InvalidOperationException("No solution");

            return _solution;
        }

        private void Solve()
        {
            var plan = AiBookAndOrSearch<TState, TAction>.AndOrGraphSearch(_problem, _initialState);

            if (plan == null) return;
            
            var planList = plan.ToList();
            _solution = new NonDeterministicSearchSolution<TState, TAction>(planList);
        }
    }
}