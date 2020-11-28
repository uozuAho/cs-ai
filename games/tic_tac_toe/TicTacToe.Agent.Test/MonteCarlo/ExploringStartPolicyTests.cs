using System.Collections.Generic;
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
            var game = Substitute.For<ITicTacToeGame>();
            game.GetAvailableActions().Returns(new List<TicTacToeAction>
            {
                new TicTacToeAction {Position = 1},
                new TicTacToeAction {Position = 2}
            });
            var innerPolicy = Substitute.For<IPlayer>();

            var firstActions = Enumerable.Range(0, 10)
                .Select(_ => new ExploringStartPolicy(innerPolicy).GetAction(game));

            Assert.Greater(1, firstActions.Distinct().Count());
        }
    }
}
