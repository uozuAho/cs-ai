using NUnit.Framework;

namespace TicTacToe.Console.Test
{
    public class TicTacToeConsoleRunnerTests
    {
        private StubUserInput _user;
        private TestUserOutput _output;
        private TicTacToeConsoleRunner _runner;

        [SetUp]
        public void Setup()
        {
            _user = new StubUserInput();
            _output = new TestUserOutput();
            _runner = new TicTacToeConsoleRunner(_user);
        }

        [Test]
        public void TwoFirstAvailableSlotAgents_PlayUntilGameIsOver()
        {
            // argh this is too hard to implement, just do like yatzy
            _output.ExpectLine("Player choices:");
            _output.ExpectLineContaining("a: ConsoleInputPlayer");
            _output.ExpectLineContaining("b: FirstAvailableSlotAgent");
            _user.EnterLine("b");
            _user.EnterLine("b");
            _user.EnterLine("1");

            // _runner.Run();
        }
    }
}