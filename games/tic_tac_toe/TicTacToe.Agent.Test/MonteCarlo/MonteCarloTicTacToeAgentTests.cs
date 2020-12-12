using NUnit.Framework;
using TicTacToe.Agent.MonteCarlo;
using TicTacToe.Game;

namespace TicTacToe.Agent.Test.MonteCarlo
{
    public class MonteCarloTicTacToeAgentTests
    {
        // todo: passes 50% of the time. Is this a good test?
        [Test]
        public void AfterTraining_AlwaysBeatsFirstAvailableSlotAgent()
        {
            var mcAgent = new MonteCarloTicTacToeAgent(BoardTile.X);
            var opponent = new FirstAvailableSlotAgent(BoardTile.O);

            mcAgent.Train(opponent);
            var trainedPlayer = mcAgent.ToFixedPolicyPlayer();

            for (var i = 0; i < 100; i++)
            {
                var game = new TicTacToeGame(Board.CreateEmptyBoard(), trainedPlayer, opponent);
                game.Run();
                Assert.AreEqual(trainedPlayer.Tile, game.Winner(), $"Lost/drew game {i}");
            }
        }
    }
}
