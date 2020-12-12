using System.Collections.Immutable;
using System.Linq;
using NUnit.Framework;

namespace TicTacToe.Game.Test
{
    public class BoardTests
    {
        [TestCase("xxx|   |   ", BoardTile.X)]
        [TestCase("   |xxx|   ", BoardTile.X)]
        [TestCase("   |   |xxx", BoardTile.X)]
        [TestCase("x  |x  |x  ", BoardTile.X)]
        [TestCase(" x | x | x ", BoardTile.X)]
        [TestCase("  x|  x|  x", BoardTile.X)]
        [TestCase("x  | x |  x", BoardTile.X)]
        [TestCase("  x| x |x  ", BoardTile.X)]
        [TestCase("ooo|   |   ", BoardTile.O)]
        [TestCase("   |ooo|   ", BoardTile.O)]
        [TestCase("   |   |ooo", BoardTile.O)]
        [TestCase("o  |o  |o  ", BoardTile.O)]
        [TestCase(" o | o | o ", BoardTile.O)]
        [TestCase("  o|  o|  o", BoardTile.O)]
        [TestCase("o  | o |  o", BoardTile.O)]
        [TestCase("  o| o |o  ", BoardTile.O)]
        // negatives
        [TestCase("   |   |   ", null)]
        [TestCase("   |x  |   ", null)]
        [TestCase("   |x  |  o", null)]
        public void TestWinner(string boardState, BoardTile? expectedWinner)
        {
            var board = Board.CreateFromString(boardState);
            Assert.AreEqual(expectedWinner, board.Winner());
        }

        [TestCase("xxx|   | oo")]
        [TestCase("x  |   |  o")]
        [TestCase("xox|" +
                  "oxx|" +
                  "oox")]
        [TestCase("xxx|" +
                  "oox|" +
                  "oox")]
        public void TestValidBoards(string boardState)
        {
            var board = Board.CreateFromString(boardState);
            Assert.IsTrue(board.IsValid());
        }

        [TestCase("xxx|   |   ")]
        [TestCase("xxx|xxx|xxx")]
        [TestCase("ooo|ooo|ooo")]
        [TestCase("xxx|ooo|   ")]
        public void TestInvalidBoards(string boardState)
        {
            var board = Board.CreateFromString(boardState);
            Assert.IsFalse(board.IsValid());
        }

        [TestCase("xxx|   | oo")]
        [TestCase("x  |   |  o")]
        [TestCase("xxx|   |   ")]
        [TestCase("xxx|xxx|xxx")]
        [TestCase("ooo|ooo|ooo")]
        [TestCase("xxx|ooo|   ")]
        public void CreateFromString_ShouldGenerateBoardWithSameString(string boardState)
        {
            var board = Board.CreateFromString(boardState);

            Assert.AreEqual(boardState, board.ToString());
        }

        [Test]
        public void Clone_ShouldBeEqual()
        {
            var board = Board.CreateEmptyBoard();
            var board2 = board.Clone();

            Assert.AreEqual(board, board2);
        }

        [Test]
        public void Clone_ShouldHaveSameTiles()
        {
            var board = Board.CreateEmptyBoard();
            var board2 = board.Clone();

            for (var i = 0; i < 9; i++)
            {
                Assert.AreEqual(board.GetTileAt(i), board2.GetTileAt(i));
            }
        }

        [Test]
        public void CreateEmpty_CurrentPlayerIsX()
        {
            var board = Board.CreateEmptyBoard();

            Assert.AreEqual(BoardTile.X, board.CurrentPlayer);
        }

        [Test]
        public void Clone_HasSameCurrentPlayer()
        {
            var board = Board.CreateEmptyBoard(BoardTile.O);

            Assert.AreEqual(BoardTile.O, board.CurrentPlayer);
            Assert.AreEqual(BoardTile.O, board.Clone().CurrentPlayer);
        }

        [Test]
        public void GivenClone_ModifyOriginal_DoesNotModifyClone()
        {
            var board = Board.CreateEmptyBoard();
            var board2 = board.Clone();
            const int position = 0;
            var board2TileBefore = board2.GetTileAt(position);

            board.DoAction(new TicTacToeAction {Tile = BoardTile.X, Position = position});

            Assert.AreEqual(board2TileBefore, board2.GetTileAt(position));
        }

        [Test]
        public void GivenEmptyBoard_AvailableActions_ShouldCount9()
        {
            Assert.AreEqual(9, Board.CreateEmptyBoard().AvailableActions().Count());
        }

        [Test]
        public void GivenEmptyBoard_AvailableActions_ShouldAllBeForCurrentPlayer()
        {
            var board = Board.CreateEmptyBoard();
            var actions = board.AvailableActions().ToList();

            Assert.IsTrue(actions.All(a => a.Tile == board.CurrentPlayer));
        }

        [Test]
        public void GivenBoardWithOnePlacedTile_AvailableActions_ShouldCount8()
        {
            var board = Board.CreateFromString("x  |   |   ");

            Assert.AreEqual(8, board.AvailableActions().Count());
        }

        [Test]
        public void ImmutableArrayCopies_AreNotEqual()
        {
            var a = ImmutableArray.Create(1, 2, 3);
            var b = ImmutableArray.Create(1, 2, 3);

            Assert.AreNotEqual(a, b);
            Assert.True(a.SequenceEqual(b));
        }

        [Test]
        public void ImmutableArrayCopies_AreSequenceEqual()
        {
            var a = ImmutableArray.Create(1, 2, 3);
            var b = ImmutableArray.Create(1, 2, 3);

            Assert.True(a.SequenceEqual(b));
        }

        [Test]
        public void ImmutableArrayCopies_HaveDifferentHashCodes()
        {
            var a = ImmutableArray.Create(1, 2, 3);
            var b = ImmutableArray.Create(1, 2, 3);

            Assert.AreNotEqual(a.GetHashCode(), b.GetHashCode());
        }
    }
}