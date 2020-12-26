using System.Linq;
using NUnit.Framework;
using TicTacToe.Agent.Agents;
using TicTacToe.Agent.Agents.MonteCarlo;
using TicTacToe.Agent.Environment;
using TicTacToe.Game;

namespace TicTacToe.Agent.Test.Agents.MonteCarlo
{
    internal class ExploringStartPolicyTests
    {
        [Test]
        public void FirstActionIsRandom()
        {
            var environment = new TicTacToeEnvironment(new FirstAvailableSlotPlayer(BoardTile.O));
            var innerPolicy = new MonteCarloTicTacToeAgent(BoardTile.X);

            var distinctFirstActions = Enumerable.Range(0, 10)
                .Select(_ => new ExploringStartPolicy(innerPolicy).GetAction(environment))
                .Distinct();

            Assert.Greater(distinctFirstActions.Count(), 1);
        }
    }
}
