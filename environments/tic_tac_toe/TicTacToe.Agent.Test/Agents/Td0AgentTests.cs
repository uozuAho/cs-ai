﻿using System;
using System.Linq;
using NUnit.Framework;
using TicTacToe.Agent.Agents;
using TicTacToe.Agent.Storage;
using TicTacToe.Game;

namespace TicTacToe.Agent.Test.Agents
{
    internal class Td0AgentTests
    {
        [Test]
        public void Trains()
        {
            var agent = new Td0Agent(BoardTile.X);
            agent.Train(new RandomTicTacToePlayer(BoardTile.O), 50);
            Assert.DoesNotThrow(() => agent.GetCurrentValues("name", "description"));
        }

        [Test]
        public void AfterTraining_PlaysAGameToCompletion()
        {
            var agent = new Td0Agent(BoardTile.X);
            var opponent = new FirstAvailableSlotPlayer(BoardTile.O);
            agent.Train(opponent, 50);
            var agentPlayer = new GreedyStateValuePlayer(agent.GetCurrentStateValues(), BoardTile.X);
            var game = new TicTacToeGame(Board.CreateEmptyBoard(), agentPlayer, opponent);

            game.Run();

            Assert.IsTrue(game.IsFinished());
        }

        [Test]
        public void Saves_And_Loads()
        {
            var agent = new Td0Agent(BoardTile.X);
            var opponent = new FirstAvailableSlotPlayer(BoardTile.O);
            agent.Train(opponent, 1);

            var path = $"{nameof(Td0AgentTests)}.{nameof(Saves_And_Loads)}.agent.json";
            agent.SaveTrainedValues("asdf", path);

            var stateValueTable = PolicyFileIo.LoadStateValueTable(path);
            Assert.NotNull(stateValueTable);
        }

        [Test]
        public void AllStateValues_AreBetween_0_and_1()
        {
            var agent = new Td0Agent(BoardTile.X);
            var opponent = new FirstAvailableSlotPlayer(BoardTile.O);
            agent.Train(opponent, 50);

            var stateValues = agent.GetCurrentStateValues().All().ToList();

            Assert.IsTrue(stateValues.All(sv => Math.Abs(sv.Item2) >= 0.0 && Math.Abs(sv.Item2) <= 1.0));
        }
    }
}
