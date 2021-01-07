using TicTacToe.Agent.Environment;
using TicTacToe.Game;

namespace TicTacToe.Agent
{
    public interface ITicTacToeAgent
    {
        BoardTile Tile { get; }
        TicTacToeAction GetAction(TicTacToeEnvironment environment);
        void Train(ITicTacToePlayer opponent, int? numGamesLimit = null);
        void SaveTrainedValues(string agentName, string path);
    }
}