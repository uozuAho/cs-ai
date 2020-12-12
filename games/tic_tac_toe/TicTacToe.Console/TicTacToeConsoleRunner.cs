using System.IO;
using System.Linq;
using TicTacToe.Agent.MonteCarlo;
using TicTacToe.Console.Test;
using TicTacToe.Game;

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
                return;
            }

            _register.LoadPolicyFiles();

            switch (args[0])
            {
                case "play":
                {
                    var runner = new InteractiveTicTacToeConsoleRunner(_userInput, _userOutput, _register);
                    runner.Run(args.Skip(1).ToArray());
                    break;
                }
                case "train":
                {
                    var agent = new MonteCarloTicTacToeAgent(BoardTile.X);
                    var opponent = _register.GetPlayerByName(args[2], BoardTile.O);
                    var opponentAgent = new PlayerAgent(opponent);
                    agent.Train(opponentAgent);
                    var policyStorage = new PolicyStorage();
                    policyStorage.SaveJsonFile("trained MonteCarloTicTacToeAgent.json", agent.ToFixedPolicy());
                    Print("Trained agent 'mc' against 'FirstAvailableSlotAgent'");
                    break;
                }
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

    internal class PlayerAgent : ITicTacToeAgent
    {
        public BoardTile Tile => _player.Tile;

        public TicTacToeAction GetAction(TicTacToeEnvironment environment, Board board) => _player.GetAction(board);

        private readonly IPlayer _player;

        public PlayerAgent(IPlayer player)
        {
            _player = player;
        }
    }

    public class PolicyStorage
    {
        public void SaveJsonFile(string filename, TicTacToeMutablePolicy player)
        {
            File.WriteAllText(filename, "asdf");
        }
    }
}