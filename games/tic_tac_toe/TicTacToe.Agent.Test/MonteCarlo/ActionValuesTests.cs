using System.Collections.Generic;
using NUnit.Framework;
using TicTacToe.Agent.MonteCarlo;
using TicTacToe.Game;

namespace TicTacToe.Agent.Test.MonteCarlo
{
    public class ActionValuesTests
    {
        [Test]
        public void HighestValueAction_ThrowsKeyError_ForNonExistentState()
        {
            Assert.Throws<KeyNotFoundException>(() =>
                new ActionValues().HighestValueAction(new Board()));
        }

        [Test]
        public void HighestValueAction_ReturnsHighestValueAction()
        {
            var values = new ActionValues();
            var state = new Board();
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
            var state = new Board();
            var action = new TicTacToeAction();

            values.Set(state, action, 1.0);

            Assert.AreEqual(action, values.HighestValueAction(state));
        }
    }
}
