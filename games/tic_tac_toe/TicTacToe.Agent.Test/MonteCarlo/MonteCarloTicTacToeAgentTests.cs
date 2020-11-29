using System.Linq;
using NUnit.Framework;
using TicTacToe.Agent.MonteCarlo;
using TicTacToe.Game;

namespace TicTacToe.Agent.Test.MonteCarlo
{
    public class MonteCarloTicTacToeAgentTests
    {
        [Test]
        public void Trains()
        {
            var mcAgent = new MonteCarloTicTacToeAgent(BoardTile.X);

            mcAgent.Train(new FirstAvailableSlotAgent(BoardTile.O));

            Assert.IsNotEmpty(mcAgent.CurrentMutablePolicy.States);
            mcAgent.CurrentMutablePolicy.Action(mcAgent.CurrentMutablePolicy.States.First());
        }

        [Test]
        public void AfterTraining_AlwaysBeatsFirstAvailableSlotAgent()
        {
            var mcAgent = new MonteCarloTicTacToeAgent(BoardTile.X);
            var opponent = new FirstAvailableSlotAgent(BoardTile.O);

            mcAgent.Train(opponent);
            var trainedPlayer = mcAgent.ToFixedPolicyPlayer();

            for (var i = 0; i < 100; i++)
            {
                var game = new TicTacToeGame(new Board(), trainedPlayer, opponent);
                game.Run();
                Assert.AreEqual(trainedPlayer.Tile, game.Winner(), $"Lost/drew game {i}");
            }
        }

        [Test]
        [Ignore("not sure if this is a good test")]
        public void Train_ExploresAllPossibleStarts()
        {
            var mcAgent = new MonteCarloTicTacToeAgent(BoardTile.X);
            var opponent = new FirstAvailableSlotAgent(BoardTile.O);

            mcAgent.Train(opponent);

            var states = mcAgent.CurrentMutablePolicy.States.ToHashSet();
            Assert.AreEqual(9, states.Count);

            // each state has an x in a different spot
            Assert.AreEqual(9, Enumerable.Range(0, 9)
                .Select(i => states.Where(s => s[i] == 'x'))
                .Count());
        }
    }
}
