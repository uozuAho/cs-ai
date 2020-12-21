using System;
using TicTacToe.Game;

namespace TicTacToe.Agent.Agents
{
    public class FirstAvailableSlotPlayer : ITicTacToePlayer
    {
        public BoardTile Tile { get; }

        public FirstAvailableSlotPlayer(BoardTile playerTile)
        {
            Tile = playerTile;
        }

        public TicTacToeAction GetAction(Board board)
        {
            for (var i = 0; i < 9; i++)
            {
                if (board.GetTileAt(i) == BoardTile.Empty)
                {
                    return new TicTacToeAction
                    {
                        Position = i,
                        Tile = Tile
                    };
                }
            }

            throw new InvalidOperationException("No available actions");
        }
    }
}
