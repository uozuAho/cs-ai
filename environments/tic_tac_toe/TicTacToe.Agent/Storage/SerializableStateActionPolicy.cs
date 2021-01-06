using System.Collections.Generic;
using TicTacToe.Agent.Agents;
using TicTacToe.Agent.Utils;
using TicTacToe.Game;

namespace TicTacToe.Agent.Storage
{
    // todo: make this private
    // don't use this type in client code
    public record StateAction(string Board, double Value, int Action);

    public record SerializableStateActionPolicy(
        string Name,
        string Description,
        BoardTile Tile) : ITicTacToePolicy
    {
        // ReSharper disable once UnusedMember.Global : used for (de)serialisation
        public PolicyFileType Type => PolicyFileType.StateAction;

        // only here for (de)serialization, don't use externally
        public List<StateAction> Actions { get; init; } = new();

        public ITicTacToePlayer ToPlayer()
        {
            return new TicTacToeFixedPolicyPlayer(ToPolicy(), Tile);
        }

        public void AddStateAction(Board board, int position, double value)
        {
            Actions.Add(new StateAction(board.ToString(), value, position));
        }

        private FixedPolicy ToPolicy()
        {
            var policy = new FixedPolicy();

            foreach (var (boardString, _, position) in Actions)
            {
                var board = Board.CreateFromString(boardString, Tile);
                var action = new TicTacToeAction { Tile = Tile, Position = position };
                policy.SetAction(board, action);
            }

            return policy;
        }
    }
}
