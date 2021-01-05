using System.Collections.Generic;
using TicTacToe.Game;

namespace TicTacToe.Agent.Utils
{
    // todo: make this private
    // don't use this type in client code
    public record StateValue(string Board, double Value);

    public record StateValuePolicy : ITicTacToePolicy
    {
        public PolicyFileType Type => PolicyFileType.StateValue;

        public string Name { get; init; }
        public string Description { get; init; }
        public BoardTile Tile { get; init; }

        // only here for (de)serialization, don't use externally
        public List<StateValue> Values { get; init; } = new();

        private StateValuePolicy(
            string name,
            string description,
            BoardTile tile)
        {
            Name = name;
            Description = description;
            Tile = tile;
        }

        public StateValuePolicy(
            string name,
            string description,
            BoardTile tile,
            StateValueTable values)
        {
            Name = name;
            Description = description;
            Tile = tile;
            foreach (var (board, value) in values.All())
            {
                AddStateValue(board, value);
            }
        }

        public ITicTacToePlayer ToPlayer()
        {
            var stateValueTable = new StateValueTable(Tile);

            foreach (var (board, value) in Values)
            {
                stateValueTable.SetValue(Board.CreateFromString(board), value);
            }

            return new GreedyStateValuePlayer(stateValueTable, Tile);
        }

        public void AddStateValue(Board board, double value)
        {
            Values.Add(new StateValue(board.ToString(), value));
        }
    }
}