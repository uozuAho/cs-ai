using TicTacToe.Env;

namespace TicTacToe.Agent.Test
{
    public class TicTacToePTableTests : TicTacToePTableTestsBase
    {
        protected override ITicTacToePTable CreatePTable(BoardTile playerTile)
        {
            return new TicTacToePTable(playerTile);
        }
    }
}
