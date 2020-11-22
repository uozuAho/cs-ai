using NUnit.Framework;

namespace TicTacToe.Console.Test
{
    public class TicTacToeConsoleRunnerTests
    {
        private TicTacToeConsoleRunner _runner;

        [SetUp]
        public void Setup()
        {
            var userInput = new StubUserInput();
            _runner = new TicTacToeConsoleRunner(userInput);
        }

        [Test]
        public void Test1()
        {
            // doesn't end, waiting for input
            // _runner.Run();
        }
    }
}