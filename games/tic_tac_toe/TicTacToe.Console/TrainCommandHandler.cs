using TicTacToe.Agent.Agents.MonteCarlo;
using TicTacToe.Console.Io;
using TicTacToe.Game;

namespace TicTacToe.Console
{
    public class TrainCommandHandler
    {
        private readonly ITextOutput _userOutput;
        private readonly PlayerRegister _register;

        public TrainCommandHandler(ITextOutput userOutput, PlayerRegister register)
        {
            _userOutput = userOutput;
            _register = register;
        }

        public static TrainCommandHandler Default()
        {
            return new(new ConsoleTextOutput(), new PlayerRegister());
        }

        public void Run(string opponentName, string agentName)
        {
            var opponent = _register.GetPlayerByName(opponentName, BoardTile.O);
            var agent = TrainAgent(opponent);
            agent.GetCurrentActionMap().SaveToFile($"{agentName}.agent.json");
            _userOutput.PrintLine($"Trained mc agent '{agentName}' against '{opponentName}'");
        }

        private static MonteCarloTicTacToeAgent TrainAgent(ITicTacToePlayer opponent)
        {
            var agent = new MonteCarloTicTacToeAgent(BoardTile.X);
            var opponentAgent = new PlayerAgent(opponent);
            agent.Train(opponentAgent);
            return agent;
        }
    }
}