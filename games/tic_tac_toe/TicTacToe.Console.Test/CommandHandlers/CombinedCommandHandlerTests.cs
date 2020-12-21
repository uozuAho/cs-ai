using NUnit.Framework;
using TicTacToe.Console.CommandHandlers;
using TicTacToe.Console.Test.Utils;

namespace TicTacToe.Console.Test.CommandHandlers
{
    public class CombinedCommandHandlerTests
    {
        private TestUserOutput _output;

        [SetUp]
        public void Setup()
        {
            _output = new TestUserOutput();
        }

        [Test]
        public void AfterTrain_NewAgentIsAvailableInRegister()
        {
            const string trainedAgentName = "mc_vs_firstSlot";
            const int numGames = 1;

            var trainer = new TrainCommandHandler(_output, new PlayerRegister());
            trainer.Run("FirstAvailableSlotAgent", trainedAgentName, numGames);

            var lister = new ListCommandHandler(new PlayerRegister(), _output);
            lister.Run();

            Assert.True(_output.ContainsLine(line => line.Contains(trainedAgentName)));
        }

        [Test]
        public void AfterTrain_TrainedAgentIsPlayable()
        {
            const string agentName = "mc_agent";
            const int numGames = 1;

            var trainer = new TrainCommandHandler(_output, new PlayerRegister());
            trainer.Run("FirstAvailableSlotAgent", agentName, numGames);

            var runner = new PlayCommandHandler(_output, new PlayerRegister());
            runner.Run(agentName, "FirstAvailableSlotAgent");

            Assert.True(_output.ContainsLine(line => line.Contains("The winner is:")));
        }
    }
}