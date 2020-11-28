using NUnit.Framework;

namespace TicTacToe.Console.Test
{
    public class TicTacToeConsoleRunnerTests
    {
        private StubUserInput _user;
        private TestUserOutput _output;
        private TicTacToeConsoleRunner _ticTacToeRunner;

        [SetUp]
        public void Setup()
        {
            _user = new StubUserInput();
            _output = new TestUserOutput();
            _ticTacToeRunner = new TicTacToeConsoleRunner(_user, _output);
        }

        [Test]
        public void TwoFirstAvailableSlotAgents_PlayUntilGameIsOver()
        {
            _user.WillEnterLines("b", "b", "1");

            // act
            _ticTacToeRunner.Run();

            // assert
            _output.ExpectLines(
                "Player choices:",
                "  a: ConsoleInputPlayer",
                "  b: FirstAvailableSlotAgent",
                "  c: PTableAgent",
                "  d: ModifiedPTableAgent",
                "Choose player 1 (x)",
                "Choose player 2 (o)",
                "How many games? (more than 5 runs headless)");

            _output.ReadToEnd();
        }
    }
}