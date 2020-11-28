using System;
using TicTacToe.Game;

namespace TicTacToe.Agent.MonteCarlo
{
    internal class ExploringStartPolicy : IPlayer
    {
        public BoardTile Tile { get; }

        public ExploringStartPolicy(IPlayer policy)
        {
        }

        public TicTacToeAction GetAction(ITicTacToeGame game)
        {
            throw new NotImplementedException();
        }
    }
}