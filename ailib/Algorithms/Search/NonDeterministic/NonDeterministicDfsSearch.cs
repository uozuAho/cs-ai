using System;
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
            var solution = new NonDeterministicSearchSolution<TState, TAction>();
            var currentState = _initialState;

            while (true)
            {
                if (_problem.IsGoal(currentState))
                {
                    _solution = solution;
                    break;
                }
                
                var availableActions = _problem.GetActions(currentState).ToList();
                if (!availableActions.Any())
                    break;
                
                var action = availableActions.First();
                var nextState = _problem.DoAction(currentState, action).First();
                if (solution.Contains(nextState))
                {
                    break;
                }
                solution.AddAction(currentState, action);
                currentState = nextState;
            }
            // todo: implement dfs from book here
        }
    }
}