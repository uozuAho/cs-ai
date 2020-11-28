using NUnit.Framework;
using TicTacToe.Game;

namespace TicTacToe.Agent.Test
{
    class MonteCarloAgentTests
    {
        [Test]
        public void PlaysGame()
        {
            var mcAgent = new MonteCarloTicTacToeAgent(BoardTile.X);
            var game = new TicTacToeGame(new Board(), mcAgent, new FirstAvailableSlotAgent(BoardTile.O));

            game.Run();

            Assert.True(game.IsFinished());
        }
    }
}
