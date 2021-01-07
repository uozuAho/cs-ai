using System;
using System.Linq;
using NUnit.Framework;
using TicTacToe.Agent.Agents;
using TicTacToe.Agent.Storage;
using TicTacToe.Game;

namespace TicTacToe.Agent.Test.Agents
{
    internal class NStepAgentTests
    {
        private NStepAgent _agent;

        [SetUp]
        public void Setup()
        {
            _agent = new NStepAgent(BoardTile.X);
        }

        [Test]
        public void Trains()
        {
            _agent.Train(new RandomTicTacToePlayer(BoardTile.O), 50);
            Assert.DoesNotThrow(() => _agent.GetCurrentValues("name", "description"));
        }

        [Test]
        public void AfterTraining_PlaysAGameToCompletion()
        {
            var opponent = new FirstAvailableSlotPlayer(BoardTile.O);
            _agent.Train(opponent, 50);
            var agentPlayer = new GreedyStateValuePlayer(_agent.GetCurrentStateValues(), BoardTile.X);
            var game = new TicTacToeGame(Board.CreateEmptyBoard(), agentPlayer, opponent);

            game.Run();

            Assert.IsTrue(game.IsFinished());
        }

        [Test]
        public void Saves_And_Loads()
        {
            var opponent = new FirstAvailableSlotPlayer(BoardTile.O);
            _agent.Train(opponent, 1);

            var path = $"{nameof(NStepAgentTests)}.{nameof(Saves_And_Loads)}.agent.json";
            _agent.SaveTrainedValues("asdf", path);

            var stateValueTable = PolicyFileIo.LoadStateValueTable(path);
            Assert.NotNull(stateValueTable);
        }

        [Test]
        public void AllStateValues_AreBetween_0_and_1()
        {
            var opponent = new FirstAvailableSlotPlayer(BoardTile.O);
            _agent.Train(opponent, 50);

            var stateValues = _agent.GetCurrentStateValues().All().ToList();

            Assert.IsTrue(stateValues.All(sv => Math.Abs(sv.Item2) >= 0.0 && Math.Abs(sv.Item2) <= 1.0));
        }
    }
}
