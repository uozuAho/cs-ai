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
            // var agent = GetAgentFromSomewhere(args[0]);
            var opponent = _register.GetPlayerByName(args[1], BoardTile.O);
            TrainAgent(opponent);
            _userOutput.PrintLine("Trained agent 'mc' against 'FirstAvailableSlotAgent'");
        }

        private static void TrainAgent(ITicTacToePlayer opponent)
        {
            var agent = new MonteCarloTicTacToeAgent(BoardTile.X);
            var opponentAgent = new PlayerAgent(opponent);
            agent.Train(opponentAgent);
            var policyStorage = new PolicyStorage();
            policyStorage.SaveJsonFile("trained MonteCarloTicTacToeAgent.json", agent.GetCurrentActionMap());
        }
    }
}