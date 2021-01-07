using NUnit.Framework;
using TicTacToe.Agent.Agents;
using TicTacToe.Agent.Agents.MonteCarlo;
using TicTacToe.Agent.Storage;
using TicTacToe.Game;

namespace TicTacToe.Agent.Test.Agents.MonteCarlo
{
    internal class MonteCarloTicTacToeAgentTests
    {
        [Test]
        public void Saves_And_Loads()
        {
            var agent = new MonteCarloTicTacToeAgent(BoardTile.X);
            var opponent = new FirstAvailableSlotPlayer(BoardTile.O);
            agent.Train(opponent, 1);

            var path = $"{nameof(MonteCarloTicTacToeAgentTests)}.{nameof(Saves_And_Loads)}.agent.json";
            agent.SaveTrainedValues("asdf", path);

            var stateActionTable = PolicyFileIo.LoadStateActionTable(path);
            Assert.NotNull(stateActionTable);
        }
    }
}
