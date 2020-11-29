using System.Linq;
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

            mcAgent.Train(new FirstAvailableSlotAgent(BoardTile.O));

            Assert.IsNotEmpty(mcAgent.CurrentPolicy.States);
            mcAgent.CurrentPolicy.Action(mcAgent.CurrentPolicy.States.First());
        }

        [Test]
        public void Train_ExploresAllPossibleStarts()
        {
            var mcAgent = new MonteCarloTicTacToeAgent(BoardTile.X);
            var opponent = new FirstAvailableSlotAgent(BoardTile.O);

            var game = new TicTacToeGame(new Board(), mcAgent, opponent);

            mcAgent.Train(opponent);

            var states = mcAgent.CurrentPolicy.States.ToHashSet();
            Assert.True(states.Contains("x        "));
        }
    }
}
