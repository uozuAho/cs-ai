using NUnit.Framework;
using TicTacToe.Agent.MonteCarlo;
using TicTacToe.Game;

namespace TicTacToe.Agent.Test.MonteCarlo
{
    public class MonteCarloTicTacToeAgentTests
    {
        [Test]
        public void PlaysGame()
        {
            var mcAgent = new MonteCarloTicTacToeAgent(BoardTile.X);
            var game = new TicTacToeGame(new Board(), mcAgent, new FirstAvailableSlotAgent(BoardTile.O));

            game.Run();

            Assert.True(game.IsFinished());
        }

        [Test]
        public void Trains()
        {
            var mcAgent = new MonteCarloTicTacToeAgent(BoardTile.X);
            var game = new TicTacToeGame(new Board(), mcAgent, new FirstAvailableSlotAgent(BoardTile.O));

            mcAgent.Train(game);

            Assert.IsNotEmpty(mcAgent.CurrentPolicy.States);
        }
    }
}
