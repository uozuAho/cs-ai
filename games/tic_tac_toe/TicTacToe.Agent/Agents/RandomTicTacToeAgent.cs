using System;
using ailib.Utils;
using TicTacToe.Game;

namespace TicTacToe.Agent.Agents
{
    public class RandomTicTacToeAgent : ITicTacToePlayer
    {
        public BoardTile Tile { get; }

        private readonly Random _random = new();

        public RandomTicTacToeAgent(BoardTile tile)
        {
            Tile = tile;
        }

        public TicTacToeAction GetAction(Board board)
        {
            return _random.Choice(board.AvailableActions());
        }
    }
}
