using System;
using ailib.Utils;
using TicTacToe.Game;

namespace TicTacToe.Agent.MonteCarlo
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
                ? _rng.Choice(environment.AvailableActions())
                : _innerAgent.GetAction(environment);

            _isFirstAction = false;

            return action;
        }

        public TicTacToeAction GetAction(
            TicTacToeEnvironment environment,
            TicTacToeObservation lastObservation)
        {
            return GetAction(environment);
        }
    }
}