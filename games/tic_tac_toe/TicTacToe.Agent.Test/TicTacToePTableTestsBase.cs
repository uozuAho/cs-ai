using System;
using NUnit.Framework;
using TicTacToe.Game;

namespace TicTacToe.Agent.Test
{
    public abstract class TicTacToePTableTestsBase
    {
        private const BoardTile PlayerTile = BoardTile.X;
        private ITicTacToePTable _table;

        protected abstract ITicTacToePTable CreatePTable(BoardTile playerTile);

        [OneTimeSetUp]
        public void Setup()
        {
            _table = CreatePTable(PlayerTile);
        }

        [TestCase("xxx      ")]
        [TestCase("ooooooooo")]
        [TestCase("xxxooo   ")]
        public void GivenInvalidBoard_GetWinProbability_ShouldThrow(string boardState)
        {
            var board = Board.CreateFromString(boardState);
            Assert.Throws<ArgumentException>(() => _table.GetWinProbability(board));
        }

        [TestCase("xxx      ")]
        [TestCase("ooooooooo")]
        [TestCase("xxxooo   ")]
        public void GivenInvalidBoard_UpdateWinProbability_ShouldThrow(string boardState)
        {
            var board = Board.CreateFromString(boardState);
            Assert.Throws<ArgumentException>(() => _table.UpdateWinProbability(board, 0.3));
        }

        [Test]
        public void GivenPlayerTileWon_PShouldBe1()
        {
            Assert.AreEqual(1, _table.GetWinProbability(Board.CreateFromString("xxxoo    ")));
        }

        [Test]
        public void GivenPlayerTileLost_PShouldBe0()
        {
            Assert.AreEqual(0, _table.GetWinProbability(Board.CreateFromString("xx ooo   ")));
        }

        [Test]
        public void GivenNoWinner_PShouldBeHalf()
        {
            Assert.AreEqual(0.5, _table.GetWinProbability(Board.CreateFromString("         ")));
        }
    }
}