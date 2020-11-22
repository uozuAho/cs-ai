using TicTacToe.Game;

namespace TicTacToe.Agent.Test
{
    public class TicTacToePTableLazyTests : TicTacToePTableTestsBase
    {
        protected override ITicTacToePTable CreatePTable(BoardTile playerTile)
        {
            return new TicTacToePTableLazy(playerTile);
        }
    }
}
