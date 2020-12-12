using TicTacToe.Console.Test;

namespace TicTacToe.Console
{
    public class TicTacToeConsoleRunner
    {
        private readonly ITextInput _userInput;
        private readonly ITextOutput _userOutput;
        private readonly PlayerRegister _register;

        public TicTacToeConsoleRunner(ITextInput userInput, ITextOutput userOutput, PlayerRegister register)
        {
            _userInput = userInput;
            _userOutput = userOutput;
            _register = register;
        }

        public void Run()
        {
            var runner = new InteractiveTicTacToeConsoleRunner(_userInput, _userOutput, _register);
            runner.Run();
        }

        public void Run(params string[] args)
        {
            if (args.Length == 0)
            {
                Run();
            }

            if (args[0] == "train")
            {
                Print("Trained agent 'mc' against 'FirstAvailableSlotAgent'");
            }
        }

        private void Print(string message)
        {
            _userOutput.PrintLine(message);
        }
    }
}