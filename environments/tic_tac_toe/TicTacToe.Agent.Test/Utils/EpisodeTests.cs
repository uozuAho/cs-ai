﻿using System.Collections.Generic;
using NUnit.Framework;
using TicTacToe.Agent.Agents;
using TicTacToe.Agent.Utils;
using TicTacToe.Game;

namespace TicTacToe.Agent.Test.Utils
{
    internal class EpisodeTests
    {
        [Test]
        public void TwoFirstAvailableSlotAgents_PlayPredictableGame()
        {
            var ep = Episode.Generate(
                new FirstAvailableSlotPlayer(BoardTile.X),
                new FirstAvailableSlotPlayer(BoardTile.O));

            Assert.AreEqual(5, ep.Length);
            Assert.AreEqual("   |   |   ", ep.Steps[0].State.ToString());
            Assert.AreEqual("xo |   |   ", ep.Steps[1].State.ToString());
            Assert.AreEqual("xox|o  |   ", ep.Steps[2].State.ToString());
            Assert.AreEqual("xox|oxo|   ", ep.Steps[3].State.ToString());
            Assert.AreEqual("xox|oxo|x  ", ep.Steps[4].State.ToString());

            Assert.AreEqual(1.0, ep.Steps[4].Reward);
            Assert.IsNull(ep.Steps[4].Action);
        }

        [Test]
        public void TwoFirstAvailableSlotAgents_PlayPredictableGame_TilesReversed()
        {
            var ep = Episode.Generate(
                new FirstAvailableSlotPlayer(BoardTile.O),
                new FirstAvailableSlotPlayer(BoardTile.X));

            Assert.AreEqual(5, ep.Length);
            Assert.AreEqual("   |   |   ", ep.Steps[0].State.ToString());
            Assert.AreEqual("ox |   |   ", ep.Steps[1].State.ToString());
            Assert.AreEqual("oxo|x  |   ", ep.Steps[2].State.ToString());
            Assert.AreEqual("oxo|xox|   ", ep.Steps[3].State.ToString());
            Assert.AreEqual("oxo|xox|o  ", ep.Steps[4].State.ToString());

            Assert.AreEqual(-1.0, ep.Steps[4].Reward);
            Assert.IsNull(ep.Steps[4].Action);
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        public void TimeOfFirstVisit_ToStateAndActionN_IsN(int time)
        {
            var ep = Episode.Generate(
                new FirstAvailableSlotPlayer(BoardTile.X),
                new FirstAvailableSlotPlayer(BoardTile.O));

            var stateN = ep.Steps[time].State;
            var actionN = ep.Steps[time].Action;

            Assert.AreEqual(time, ep.TimeOfFirstVisit(stateN, actionN));
        }

        [Test]
        public void TimeOfFirstVisit_ThrowsWhenStateActionPairWasNotVisited()
        {
            var ep = Episode.Generate(
                new FirstAvailableSlotPlayer(BoardTile.X),
                new FirstAvailableSlotPlayer(BoardTile.O));

            Assert.Throws<KeyNotFoundException>(() =>
                ep.TimeOfFirstVisit(Board.CreateFromString("xxx|xxx|xxx"), new TicTacToeAction()));
        }
    }
}
