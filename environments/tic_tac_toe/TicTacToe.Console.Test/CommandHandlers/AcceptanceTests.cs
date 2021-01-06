using NUnit.Framework;
using TicTacToe.Console.Test.Utils;

namespace TicTacToe.Console.Test.CommandHandlers
{
    public class AcceptanceTests
    {
        private ConsoleCapture _consoleCapture;

        [SetUp]
        public void Setup()
        {
            _consoleCapture = new ConsoleCapture();
        }

        [Test]
        public void AfterTrain_NewAgentIsAvailableInRegister()
        {
            RunCli("train --agent mc --opponent FirstAvailableSlotPlayer --limit-training-rounds 1");
            RunCli("list");

            Assert.True(_consoleCapture.ContainsLine(line => line.Contains("mc")));
        }

        [TestCase("mc")]
        [TestCase("td0")]
        public void AfterTrain_TrainedAgentIsPlayable(string agentName)
        {
            RunCli($"train --agent {agentName} --opponent FirstAvailableSlotPlayer --limit-training-rounds 1");
            RunCli($"play --player1 {agentName} --player2 FirstAvailableSlotPlayer");

            Assert.True(
                _consoleCapture.ContainsLine(line => line.Contains("The winner is:"))
                || _consoleCapture.ContainsLine(line => line.Contains("Draw!")));
        }

        [Test]
        public void Play_InvalidPlayer_ShowsHelpfulMessage()
        {
            RunCli("play --player1 InvalidPlayerName --player2 FirstAvailableSlotPlayer");

            Assert.True(_consoleCapture.ContainsLine(line => line.Contains("Invalid player 'InvalidPlayerName'")));
        }

        private static void RunCli(string input)
        {
            Program.Main(ToArgs(input));
        }

        private static string[] ToArgs(string text)
        {
            return text.Split();
        }
    }
}