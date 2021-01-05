using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using TicTacToe.Game;

namespace TicTacToe.Agent.Utils
{
    public record PolicyFile(
        string Name,
        string Description,
        BoardTile Tile,
        PolicyFileAction[] Actions) : IPolicyFile
    {
        public void Save(string path)
        {
            File.WriteAllText(path, JsonSerializer.Serialize(this, BuildJsonOptions()));
        }

        public ITicTacToePlayer ToPlayer()
        {
            return new TicTacToeFixedPolicyPlayer(ToPolicy(), Tile);
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

        private static JsonSerializerOptions BuildJsonOptions()
        {
            return new()
            {
                PropertyNameCaseInsensitive = true,
                Converters =
                {
                    new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
                },
                WriteIndented = true
            };
        }
    }
}