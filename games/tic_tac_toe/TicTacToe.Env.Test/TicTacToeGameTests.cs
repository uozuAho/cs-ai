using System;
using NSubstitute;
using NUnit.Framework;
using TicTacToe.Game.Test.Utils;

namespace TicTacToe.Game.Test
{
    public class TicTacToeGameTests
    {
        private Board _board;
        private IPlayer _player1;
        private IPlayer _player2;
        private TicTacToeGame _game;

        [SetUp]
        public void Setup()
        {
            _board = new Board();
            _player1 = Substitute.For<IPlayer>();
            _player2 = Substitute.For<IPlayer>();
            _player1.Tile.Returns(BoardTile.X);
            _player2.Tile.Returns(BoardTile.O);
            _game = new TicTacToeGame(_board, _player1, _player2);
        }

        [TestCase(BoardTile.Empty, BoardTile.X)]
        [TestCase(BoardTile.X, BoardTile.X)]
        [TestCase(BoardTile.O, BoardTile.O)]
        public void CannotMakeGameWithInvalidPlayerTiles(BoardTile player1Tile, BoardTile player2Tile)
        {
            _player1.Tile.Returns(player1Tile);
            _player2.Tile.Returns(player2Tile);

            Assert.Throws<InvalidOperationException>(() =>
                new TicTacToeGame(_board, _player1, _player2));
        }

        [Test]
        public void Player1IsFirst()
        {
            _player1.GetAction(Arg.Any<Board>()).Returns(new TicTacToeAction {Position = 0, Tile = BoardTile.X});

            _game.DoNextTurn();

            _player1.Received(1).GetAction(Arg.Any<Board>());
            _player2.DidNotReceive().GetAction(Arg.Any<Board>());
        }

        [Test]
        public void Player1X_WinsInThreeMoves_Diagonal()
        {
            var player1 = new TestPlayer(BoardTile.X);
            player1.SetMoves(new[] { 0, 4, 8 });
            var player2 = new TestPlayer(BoardTile.O);
            player2.SetMoves(new[] { 1, 2 });

            var game = new TicTacToeGame(new Board(), player1, player2);

            // act
            game.Run();

            // assert
            Assert.That(game.IsFinished());
            var expectedBoardState = Board.CreateFromString("xoo|" +
                                                            " x |" +
                                                            "  x");
            Assert.That(game.Board.IsSameStateAs(expectedBoardState));
            Assert.AreEqual(BoardTile.X, game.Winner());
        }
    }
}
