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
            if (args.Length == 0 || args[0] == "-h" || args[0] == "--help")
            {
                PrintUsage();
                return;
            }

            _register.LoadPolicyFiles();

            switch (args[0])
            {
                case "play":
                {
                    var runner = new InteractiveTicTacToeConsoleRunner(_userOutput, _register);
                    runner.Run(args.Skip(1).ToArray());
                    break;
                }
                case "train":
                {
                    var trainer = new ConsoleAgentTrainer(_userOutput, _register);
                    trainer.Run(args.Skip(1).ToArray());
                    break;
                }
                case "list":
                {
                    foreach (var player in _register.AvailablePlayers())
                    {
                        Print(player);
                    }
                    break;
                }
            }
        }

        private void PrintUsage()
        {
            Print("usage: <list|play|train> [num games]");
            Print("");
            Print("examples:");
            Print("");
            Print("  # train a monte carlo agent against a FirstAvailableSlot agent, save it as 'john'");
            Print("  train mc FirstAvailableSlotAgent john");
        }

        private void Print(string message)
        {
            _userOutput.PrintLine(message);
        }
    }
}