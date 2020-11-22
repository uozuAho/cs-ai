using System;
using TicTacToe.Game;

namespace TicTacToe.Agent
{
    public class FirstAvailableSlotAgent : IPlayer
    {
        public BoardTile Tile { get; }

        public FirstAvailableSlotAgent(BoardTile playerTile)
        {
            Tile = playerTile;
        }

        public TicTacToeAction GetAction(ITicTacToeGame game)
        {
            for (var i = 0; i < 9; i++)
            {
                if (game.Board.GetTileAt(i) == BoardTile.Empty)
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
