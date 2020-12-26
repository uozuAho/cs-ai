using System;
using ailib.Utils;
using TicTacToe.Game;

namespace TicTacToe.Agent.Agents
{
    public class RandomTicTacToePlayer : ITicTacToePlayer
    {
        public BoardTile Tile { get; }

        private readonly Random _random = new();

        public RandomTicTacToePlayer(BoardTile tile)
        {
            Tile = tile;
        }

        public TicTacToeAction GetAction(Board board)
        {
            return _random.Choice(board.AvailableActions());
        }
    }
}
