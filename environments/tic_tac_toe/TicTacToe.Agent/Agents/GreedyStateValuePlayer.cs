using System.Linq;
using MoreLinq.Extensions;
using TicTacToe.Agent.Utils;
using TicTacToe.Game;

namespace TicTacToe.Agent.Agents
{
    public class GreedyStateValuePlayer : ITicTacToePlayer
    {
        public BoardTile Tile { get; }

        private readonly StateValueTable _stateValues;

        public GreedyStateValuePlayer(StateValueTable stateValues, BoardTile tile)
        {
            _stateValues = stateValues;
            Tile = tile;
        }

        public TicTacToeAction GetAction(Board board)
        {
            return HighestValueAction(board);
        }

        private TicTacToeAction HighestValueAction(Board board)
        {
            return board
                .AvailableActions()
                .Select(a => new
                {
                    action = a,
                    nextBoard = board.DoAction(a)
                })
                .MaxBy(b => _stateValues.Value(b.nextBoard))
                .First().action;
        }
    }
}