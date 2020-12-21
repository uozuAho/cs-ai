using TicTacToe.Agent.Agents.MonteCarlo;
using TicTacToe.Console.Test;
using TicTacToe.Game;

namespace TicTacToe.Console
{
    public class ConsoleAgentTrainer
    {
        private readonly ITextOutput _userOutput;
        private readonly PlayerRegister _register;

        public ConsoleAgentTrainer(ITextOutput userOutput, PlayerRegister register)
        {
            _userOutput = userOutput;
            _register = register;
        }

        public void Run(string[] args)
        {
            var opponentName = args[1];
            var agentName = args[2];
            Run(opponentName, agentName);
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