using System;
using ailib.Utils;
using TicTacToe.Agent.Environment;
using TicTacToe.Game;

namespace TicTacToe.Agent.Agents.MonteCarlo
{
    public class ExploringStartPolicy : ITicTacToeAgent
    {
        public BoardTile Tile => _innerAgent.Tile;

        private bool _isFirstAction;
        private readonly Random _rng;
        private readonly MonteCarloTicTacToeAgent _innerAgent;

        public ExploringStartPolicy(MonteCarloTicTacToeAgent innerAgent)
        {
            _innerAgent = innerAgent;
            _isFirstAction = true;
            _rng = new Random();
        }

        public TicTacToeAction GetAction(TicTacToeEnvironment environment)
        {
            var action = _isFirstAction
                ? _rng.Choice(environment.ActionSpace())
                : _innerAgent.GetAction(environment);

            _isFirstAction = false;

            return action;
        }
    }
}