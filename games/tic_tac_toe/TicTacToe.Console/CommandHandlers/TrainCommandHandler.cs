using TicTacToe.Agent.Agents.MonteCarlo;
using TicTacToe.Console.Io;
using TicTacToe.Game;

namespace TicTacToe.Console.CommandHandlers
{
    public class TrainCommandHandler
    {
        private readonly ITextOutput _userOutput;
        private readonly PlayerRegister _playerRegister;
        private readonly LearningAgentRegister _agentRegister;

        public TrainCommandHandler(
            ITextOutput userOutput,
            PlayerRegister playerRegister,
            LearningAgentRegister agentRegister)
        {
            _userOutput = userOutput;
            _playerRegister = playerRegister;
            _agentRegister = agentRegister;
        }

        public static TrainCommandHandler Default()
        {
            return new(
                new ConsoleTextOutput(),
                new PlayerRegister(),
                new LearningAgentRegister()
            );
        }

        public void Run(string opponentName, string agentName, int? numGamesLimit = null)
        {
            var opponent = _playerRegister.GetPlayerByName(opponentName, BoardTile.O);
            var agent = TrainAgent(opponent, numGamesLimit);
            agent.GetCurrentActionMap().SaveToFile($"{agentName}.agent.json");
            _userOutput.PrintLine($"Trained mc agent '{agentName}' against '{opponentName}'");
        }

        private static MonteCarloTicTacToeAgent TrainAgent(ITicTacToePlayer opponent, int? numGamesLimit)
        {
            var agent = new MonteCarloTicTacToeAgent(BoardTile.X);
            var opponentAgent = new PlayerAgent(opponent);
            agent.Train(opponentAgent, numGamesLimit);
            return agent;
        }
    }
}