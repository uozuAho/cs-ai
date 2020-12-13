using System.Collections.Generic;
using NUnit.Framework;
using TicTacToe.Agent.Utils;
using TicTacToe.Game;

namespace TicTacToe.Agent.Test.MonteCarlo
{
    public class ActionValuesTests
    {
        [Test]
        public void HighestValueAction_ThrowsKeyError_ForNonExistentState()
        {
            Assert.Throws<KeyNotFoundException>(() =>
                new ActionValues().HighestValueAction(Board.CreateEmptyBoard()));
        }

        [Test]
        public void HighestValueAction_ReturnsHighestValueAction()
        {
            var values = new ActionValues();
            var state = Board.CreateEmptyBoard();
            var action0 = new TicTacToeAction();
            var action1 = new TicTacToeAction();
            var action2 = new TicTacToeAction();

            values.Set(state, action0, 0);
            values.Set(state, action1, 1);
            values.Set(state, action2, 2);

            Assert.AreEqual(action2, values.HighestValueAction(state));
        }

        [Test]
        public void GivenASingleStateAndAction_HighestValueAction_ReturnsOnlyAction()
        {
            var values = new ActionValues();
            var state = Board.CreateEmptyBoard();
            var action = new TicTacToeAction();

            values.Set(state, action, 1.0);

            Assert.AreEqual(action, values.HighestValueAction(state));
        }

        [Test]
        public void DuplicateBoards_AreTreatedAsEqual()
        {
            var values = new ActionValues();
            var emptyBoard1 = Board.CreateEmptyBoard();
            var emptyBoard2 = Board.CreateEmptyBoard();
            var action1 = new TicTacToeAction();
            var action2 = new TicTacToeAction();

            values.Set(emptyBoard1, action1, 1.0);
            values.Set(emptyBoard2, action2, 2.0);

            Assert.AreEqual(action2, values.HighestValueAction(emptyBoard1));
            Assert.AreEqual(action2, values.HighestValueAction(emptyBoard2));
        }

        [Test]
        public void DuplicateActions_AreTreatedAsEqual()
        {
            var values = new ActionValues();
            var board = Board.CreateEmptyBoard();
            var action1 = new TicTacToeAction {Position = 2, Tile = BoardTile.X};
            var action2 = new TicTacToeAction {Position = 2, Tile = BoardTile.X};

            values.Set(board, action1, 1.0);
            values.Set(board, action2, 2.0);

            var expectedHighestValueAction = new TicTacToeAction {Position = 2, Tile = BoardTile.X};

            Assert.AreEqual(expectedHighestValueAction, values.HighestValueAction(board));
        }
    }
}
