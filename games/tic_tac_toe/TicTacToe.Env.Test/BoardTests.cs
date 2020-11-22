using NUnit.Framework;

namespace TicTacToe.Game.Test
{
    public class BoardTests
    {
        [TestCase("xxx      ", BoardTile.X)]
        [TestCase("   xxx   ", BoardTile.X)]
        [TestCase("      xxx", BoardTile.X)]
        [TestCase("x  x  x  ", BoardTile.X)]
        [TestCase(" x  x  x ", BoardTile.X)]
        [TestCase("  x  x  x", BoardTile.X)]
        [TestCase("x   x   x", BoardTile.X)]
        [TestCase("  x x x  ", BoardTile.X)]
        [TestCase("ooo      ", BoardTile.O)]
        [TestCase("   ooo   ", BoardTile.O)]
        [TestCase("      ooo", BoardTile.O)]
        [TestCase("o  o  o  ", BoardTile.O)]
        [TestCase(" o  o  o ", BoardTile.O)]
        [TestCase("  o  o  o", BoardTile.O)]
        [TestCase("o   o   o", BoardTile.O)]
        [TestCase("  o o o  ", BoardTile.O)]
        // negatives
        [TestCase("         ", null)]
        [TestCase("   x     ", null)]
        [TestCase("   x    o", null)]
        public void TestWinner(string boardState, BoardTile? expectedWinner)
        {
            var board = Board.CreateFromString(boardState);
            Assert.AreEqual(expectedWinner, board.Winner());
        }

        [TestCase("xxx    oo")]
        [TestCase("x       o")]
        [TestCase("xox" +
                  "oxx" +
                  "oox")]
        [TestCase("xxx" +
                  "oox" +
                  "oox")]
        public void TestValidBoards(string boardState)
        {
            var board = Board.CreateFromString(boardState);
            Assert.IsTrue(board.IsValid());
        }

        [TestCase("xxx      ")]
        [TestCase("xxxxxxxxx")]
        [TestCase("ooooooooo")]
        [TestCase("xxxooo   ")]
        public void TestInvalidBoards(string boardState)
        {
            var board = Board.CreateFromString(boardState);
            Assert.IsFalse(board.IsValid());
        }

        [TestCase("xxx    oo")]
        [TestCase("x       o")]
        [TestCase("xxx      ")]
        [TestCase("xxxxxxxxx")]
        [TestCase("ooooooooo")]
        [TestCase("xxxooo   ")]
        public void CreateFromString_ShouldGenerateBoardWithSameString(string boardState)
        {
            var board = Board.CreateFromString(boardState);

            Assert.AreEqual(boardState, board.AsString());
        }

        [Test]
        public void Clone_ShouldNotBeEqual()
        {
            var board = new Board();
            var board2 = board.Clone();

            Assert.AreNotEqual(board, board2);
        }

        [Test]
        public void Clone_ShouldHaveSameTiles()
        {
            var board = new Board();
            var board2 = board.Clone();

            for (var i = 0; i < 9; i++)
            {
                Assert.AreEqual(board.GetTileAt(i), board2.GetTileAt(i));
            }
        }

        [Test]
        public void GivenClone_ModifyOriginal_ShouldNotModifyClone()
        {
            var board = new Board();
            var board2 = board.Clone();
            const int position = 0;
            var board2TileBefore = board2.GetTileAt(position);

            board.Update(new TicTacToeAction {Tile = BoardTile.X, Position = position});

            Assert.AreEqual(board2TileBefore, board2.GetTileAt(position));
        }
    }
}