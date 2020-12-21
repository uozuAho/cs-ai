using TicTacToe.Agent.Environment;
using TicTacToe.Agent.Utils;
using TicTacToe.Game;

namespace TicTacToe.Agent
{
    public interface ITicTacToeAgent
    {
        BoardTile Tile { get; }
        TicTacToeAction GetAction(TicTacToeEnvironment environment);
        void Train(ITicTacToePlayer opponent, int? numGamesLimit = null);
        BoardActionMap GetCurrentPolicy();
    }
}