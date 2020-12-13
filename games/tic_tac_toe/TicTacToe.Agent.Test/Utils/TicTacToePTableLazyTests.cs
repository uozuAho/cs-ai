using TicTacToe.Agent.Utils;
using TicTacToe.Game;

namespace TicTacToe.Agent.Test.Utils
{
    public class TicTacToePTableLazyTests : TicTacToePTableTestsBase
    {
        protected override ITicTacToePTable CreatePTable(BoardTile playerTile)
        {
            return new TicTacToePTableLazy(playerTile);
        }
    }
}
