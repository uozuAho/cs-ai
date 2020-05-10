using NUnit.Framework;
using TicTacToe.Env;

namespace TicTacToe.E2eTests
{
    public class Tests
    {
        [Test]
        public void Player1X_WinsInThreeMoves_Diagonal()
        {
            var player1 = new TestPlayer(BoardTile.X);
            player1.SetMoves(new [] {0, 4, 8});
            var player2 = new TestPlayer(BoardTile.O);
            player2.SetMoves(new [] {1, 2});

            var game = new TicTacToeGame(new Board(), player1, player2);

            // act
            game.Run();

            // assert
            Assert.That(game.IsFinished());
            var expectedBoardState = Board.CreateFromString("xoo" +
                                                            " x " +
                                                            "  x");
            Assert.That(game.Board.IsSameStateAs(expectedBoardState));
            Assert.AreEqual(BoardTile.X, game.Winner());
        }
    }
}