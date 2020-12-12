namespace TicTacToe.Console
{
    public class Program
    {
        public static int Main(string[] args)
        {
            var runner = new TicTacToeConsoleRunner(
                new ConsoleTextInput(),
                new ConsoleTextOutput(),
                new PlayerRegister());

            runner.Run();

            return 0;
        }
    }
}
