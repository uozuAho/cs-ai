using System;
using System.Linq;
using NSubstitute;
using NUnit.Framework;

namespace TicTacToe.Game.Test
{
    public class TicTacToeGameTests
    {
        private IBoard _board;
        private IPlayer _player1;
        private IPlayer _player2;
        private TicTacToeGame _game;

        [SetUp]
        public void Setup()
        {
            _board = Substitute.For<IBoard>();
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
            _game.DoNextTurn();

            _player1.Received(1).GetAction(Arg.Any<ITicTacToeGame>());
            _player2.DidNotReceive().GetAction(Arg.Any<ITicTacToeGame>());
        }

        [Test]
        public void GivenEmptyBoard_AvailableActions_ShouldCount9()
        {
            var actions = _game.GetAvailableActions().ToList();

            Assert.AreEqual(9, actions.Count);
        }

        [Test]
        public void GivenEmptyBoard_AvailableActions_ShouldAllBeForPlayer1()
        {
            var actions = _game.GetAvailableActions().ToList();

            Assert.IsTrue(actions.All(a => a.Tile == _player1.Tile));
        }

        [Test]
        public void GivenBoardWithOnePlacedTile_AvailableActions_ShouldCount8()
        {
            _board.GetTileAt(0).Returns(BoardTile.O);
            var actions = _game.GetAvailableActions().ToList();

            Assert.AreEqual(8, actions.Count);
        }
    }
}
