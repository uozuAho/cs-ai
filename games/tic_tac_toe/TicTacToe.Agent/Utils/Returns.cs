using System.Collections.Generic;
using System.Linq;
using TicTacToe.Game;

namespace TicTacToe.Agent.Utils
{
    public class Returns
    {
        private readonly Dictionary<(Board, TicTacToeAction), List<double>> _returns = new();

        public void Add(Board state, TicTacToeAction action, in double reward)
        {
            var stateActionPair = (state, action);
            if (_returns.ContainsKey(stateActionPair))
                _returns[stateActionPair].Add(reward);
            else
                _returns[stateActionPair] = new List<double> {reward};
        }

        public double AverageReturnFrom(Board state, TicTacToeAction action)
        {
            var returns = _returns[(state, action)];

            return returns.Sum() / returns.Count;
        }
    }
}