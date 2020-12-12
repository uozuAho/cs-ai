using System.Linq;
using NSubstitute;
using NUnit.Framework;
using TicTacToe.Agent.MonteCarlo;
using TicTacToe.Game;

namespace TicTacToe.Agent.Test.MonteCarlo
{
    internal class ExploringStartPolicyTests
    {
        [Test]
        public void FirstActionIsRandom()
        {
            var environment = new TicTacToeEnvironment(new FirstAvailableSlotAgent(BoardTile.O));
            var innerPolicy = new MonteCarloTicTacToeAgent(BoardTile.X);

            var distinctFirstActions = Enumerable.Range(0, 10)
                .Select(_ => new ExploringStartPolicy(innerPolicy).GetAction(environment))
                .Distinct();

            Assert.Greater(distinctFirstActions.Count(), 1);
        }
    }
}
