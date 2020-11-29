using System.Collections.Generic;
using NUnit.Framework;
using TicTacToe.Agent.MonteCarlo;
using TicTacToe.Game;

namespace TicTacToe.Agent.Test.MonteCarlo
{
    internal class EpisodeTests
    {
        [Test]
        public void TwoFirstAvailableSlotAgents_PlayPredictableGame()
        {
            var ep = Episode.Generate(
                new FirstAvailableSlotAgent(BoardTile.X),
                new FirstAvailableSlotAgent(BoardTile.O));

            Assert.AreEqual(5, ep.Length);
            Assert.AreEqual("         ", ep.Steps[0].State.AsString());
            Assert.AreEqual("xo       ", ep.Steps[1].State.AsString());
            Assert.AreEqual("xoxo     ", ep.Steps[2].State.AsString());
            Assert.AreEqual("xoxoxo   ", ep.Steps[3].State.AsString());
            Assert.AreEqual("xoxoxox  ", ep.Steps[4].State.AsString());

            Assert.AreEqual(1.0, ep.Steps[4].Reward);
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        public void TimeOfFirstVisit_ToStateAndActionN_IsN(int time)
        {
            var ep = Episode.Generate(
                new FirstAvailableSlotAgent(BoardTile.X),
                new FirstAvailableSlotAgent(BoardTile.O));

            var stateN = ep.Steps[time].State;
            var actionN = ep.Steps[time].Action;

            Assert.AreEqual(time, ep.TimeOfFirstVisit(stateN, actionN));
        }

        [Test]
        public void TimeOfFirstVisit_ThrowsWhenStateActionPairWasNotVisited()
        {
            var ep = Episode.Generate(
                new FirstAvailableSlotAgent(BoardTile.X),
                new FirstAvailableSlotAgent(BoardTile.O));

            Assert.Throws<KeyNotFoundException>(() =>
                ep.TimeOfFirstVisit(Board.CreateFromString("xxxxxxxxx"), new TicTacToeAction()));
        }
    }
}
