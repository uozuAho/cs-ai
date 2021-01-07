using System.Collections.Generic;
using TicTacToe.Agent.Agents;
using TicTacToe.Agent.Utils;
using TicTacToe.Game;

namespace TicTacToe.Agent.Storage
{
    internal record StateValue(string Board, double Value);

    internal record SerializableStateValueTable : ITicTacToePolicy
    {
        // ReSharper disable once UnusedMember.Global : used for (de)serialisation
        public PolicyFileType Type => PolicyFileType.StateValue;

        public string Name { get; init; }
        public string Description { get; init; }
        public BoardTile Tile { get; init; }

        // only here for (de)serialization, don't use externally
        public List<StateValue> Values { get; set; } = new();

        public SerializableStateValueTable(
            string name,
            string description,
            BoardTile tile)
        {
            Name = name;
            Description = description;
            Tile = tile;
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

        public void SetStateValues(StateValueTable stateValues)
        {
            Values.Clear();
            foreach (var (board, value) in stateValues.All())
            {
                AddStateValue(board, value);
            }
        }

        public void AddStateValue(Board board, double value)
        {
            Values.Add(new StateValue(board.ToString(), value));
        }
    }
}