using NUnit.Framework;
using TicTacToe.Console.CommandHandlers;
using TicTacToe.Console.Test.Utils;

namespace TicTacToe.Console.Test.CommandHandlers
{
    public class CombinedCommandHandlerTests
    {
        private TestUserOutput _output;
        private PlayerRegister _playerRegister;
        private LearningAgentRegister _agentRegister;
        private TrainCommandHandler _trainer;
        private ListCommandHandler _lister;
        private PlayCommandHandler _runner;

        [SetUp]
        public void Setup()
        {
            _output = new TestUserOutput();
            _playerRegister = new PlayerRegister();
            _agentRegister = new LearningAgentRegister();
            _trainer = new TrainCommandHandler(_output, _playerRegister, _agentRegister);
            _lister = new ListCommandHandler(_playerRegister, _agentRegister, _output);
            _runner = new PlayCommandHandler(_output, new PlayerRegister());
        }

        [Test]
        public void AfterTrain_NewAgentIsAvailableInRegister()
        {
            const string trainedAgentName = "mc_vs_firstSlot";
            const int numGames = 1;

            _trainer.Run("FirstAvailableSlotAgent", trainedAgentName, numGames);
            _lister.Run();

            Assert.True(_output.ContainsLine(line => line.Contains(trainedAgentName)));
        }

        [Test]
        public void AfterTrain_TrainedAgentIsPlayable()
        {
            const string agentName = "mc_agent";
            const int numGames = 1;

            _trainer.Run("FirstAvailableSlotAgent", agentName, numGames);
            _runner.Run(agentName, "FirstAvailableSlotAgent");

            Assert.True(_output.ContainsLine(line => line.Contains("The winner is:")));
        }
    }
}