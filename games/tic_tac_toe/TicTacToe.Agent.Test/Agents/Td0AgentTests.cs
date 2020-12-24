using NUnit.Framework;
using TicTacToe.Agent.Agents;
using TicTacToe.Game;

namespace TicTacToe.Agent.Test.Agents
{
    internal class Td0AgentTests
    {
        [Test]
        public void Td0Agent_Trains()
        {
            var agent = new Td0Agent(BoardTile.X);
            agent.Train(new RandomTicTacToePlayer(BoardTile.O), 50);
            Assert.DoesNotThrow(() => agent.GetCurrentPolicy());
        }
    }
}
