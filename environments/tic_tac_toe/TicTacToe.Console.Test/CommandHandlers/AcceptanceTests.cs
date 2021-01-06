using System.IO;
using NUnit.Framework;
using TicTacToe.Console.CommandHandlers;
using TicTacToe.Console.Test.Utils;

namespace TicTacToe.Console.Test.CommandHandlers
{
    public class AcceptanceTests
    {
        private TestUserOutput _output;
        private PlayerRegister _playerRegister;
        private LearningAgentRegister _agentRegister;
        private TrainCommandHandler _trainer;
        private ListCommandHandler _lister;
        private PlayCommandHandler _runner;
        private MemoryStream _ms;
        private StreamWriter _sw;

        [SetUp]
        public void Setup()
        {
            _output = new TestUserOutput();
            _playerRegister = new PlayerRegister();
            _agentRegister = new LearningAgentRegister();
            _trainer = new TrainCommandHandler(_output, _playerRegister, _agentRegister);
            _lister = new ListCommandHandler(_playerRegister, _agentRegister, _output);
            _runner = new PlayCommandHandler(_output, new PlayerRegister());

            _ms = new MemoryStream();
            _sw = new StreamWriter(_ms);
            System.Console.SetOut(_sw);
        }

        [Test]
        public void AfterTrain_NewAgentIsAvailableInRegister()
        {
            Program.Main(ToArgs(
                "train --agent mc --opponent FirstAvailableSlotPlayer --limit-training-rounds 1"));

            _sw.Flush();
            _ms.Seek(0, SeekOrigin.Begin);
            using var sr = new StreamReader(_ms);
            while (!sr.EndOfStream)
            {
                _output.PrintLine(sr.ReadLine());
            }

            Assert.True(_output.ContainsLine(line => line.Contains("mc")));
        }

        private static string[] ToArgs(string text)
        {
            return text.Split();
        }

        [TestCase("mc")]
        [TestCase("td0")]
        public void AfterTrain_TrainedAgentIsPlayable(string agentName)
        {
            const int numGames = 1;

            _trainer.Run(agentName, "FirstAvailableSlotPlayer", numGames);
            _runner.Run(agentName, "FirstAvailableSlotPlayer");

            Assert.True(
                _output.ContainsLine(line => line.Contains("The winner is:"))
                || _output.ContainsLine(line => line.Contains("Draw!")));
        }
    }
}