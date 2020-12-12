using System.Linq;
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

        public void Run(params string[] args)
        {
            if (args.Length == 0)
            {
                PrintUsage();
            }

            else switch (args[0])
            {
                case "play":
                {
                    var runner = new InteractiveTicTacToeConsoleRunner(_userInput, _userOutput, _register);
                    runner.Run(args.Skip(1).ToArray());
                    break;
                }
                case "train":
                    Print("Trained agent 'mc' against 'FirstAvailableSlotAgent'");
                    break;
            }
        }

        private void PrintUsage()
        {
            Print("usage: run <play|train> [num games]");
        }

        private void Print(string message)
        {
            _userOutput.PrintLine(message);
        }
    }
}