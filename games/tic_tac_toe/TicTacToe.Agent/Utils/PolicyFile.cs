using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using TicTacToe.Game;

namespace TicTacToe.Agent.Utils
{
    public record PolicyFile(
        string Name,
        string Description,
        BoardTile Tile,
        PolicyFileAction[] Actions)
    {
        public static PolicyFile FromPolicy(
            string name,
            string description,
            BoardTile boardTile,
            FixedPolicy policy)
        {
            var policyActions = policy.AllActions().Select(a => new PolicyFileAction(
                a.Item1.ToString(), 0, a.Item2.Position)).ToArray();

            return new PolicyFile(name, description, boardTile, policyActions);
        }

        public static PolicyFile Load(string path)
        {
            var fileContents = File.ReadAllText(path);
            return FromJsonString(fileContents);
        }

        public void Save(string path)
        {
            File.WriteAllText(path, JsonSerializer.Serialize(this, BuildJsonOptions()));
        }

        public static PolicyFile FromJsonString(string text)
        {
            var file = JsonSerializer.Deserialize<PolicyFile>(text, BuildJsonOptions());

            if (file == null) throw new InvalidOperationException("Policy file deserialised to null :(");

            return file;
        }

        public FixedPolicy ToPolicy()
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