using System;
using TicTacToe.Agent.MonteCarlo;
using TicTacToe.Game;

namespace TicTacToe.Agent
{
    public class FirstAvailableSlotAgent : ITicTacToePlayer, ITicTacToeAgent
    {
        public BoardTile Tile { get; }

        public FirstAvailableSlotAgent(BoardTile playerTile)
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

        public TicTacToeAction GetAction(TicTacToeEnvironment environment, Board board)
        {
            return GetAction(board);
        }
    }
}
