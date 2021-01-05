using NUnit.Framework;
using TicTacToe.Agent.Agents;
using TicTacToe.Agent.Utils;
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
            Assert.DoesNotThrow(() => agent.GetCurrentPolicy("name", "description"));
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
    }
}
