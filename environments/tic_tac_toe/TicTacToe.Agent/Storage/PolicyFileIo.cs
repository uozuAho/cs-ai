using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using TicTacToe.Agent.Utils;
using TicTacToe.Game;

namespace TicTacToe.Agent.Storage
{
    public class PolicyFileIo
    {
        public static void Save(
            StateValueTable stateValues,
            string name,
            string description,
            string path)
        {
            var table = new SerializableStateValueTable(name, description, stateValues.Tile);
            table.SetStateValues(stateValues);
            File.WriteAllText(path, JsonSerializer.Serialize(table, BuildJsonOptions()));
        }

        public static void Save(
            ActionValueTable actionValues,
            string agentName,
            string description,
            string path,
            BoardTile tile)
        {
            var s = new SerializableStateActionPolicy(agentName, description, tile);
            foreach (var (board, action) in actionValues.HighestValueActions())
            {
                s.AddStateAction(
                    board,
                    action.Position,
                    actionValues.HighestValue(board));
            }
            File.WriteAllText(path, JsonSerializer.Serialize(s, BuildJsonOptions()));
        }

        public static ITicTacToePolicy FromFile(string path)
        {
            var fileContents = File.ReadAllText(path);
            var doc = JsonDocument.Parse(fileContents);
            var docTypeString = doc.RootElement.GetProperty("Type").GetString();
            if (docTypeString == null) throw new InvalidOperationException("type must not be null");
            var docType = Enum.Parse(typeof(PolicyFileType), docTypeString, true);

            return docType switch
            {
                PolicyFileType.StateValue => FromStateValueJson(fileContents),
                PolicyFileType.StateAction => FromStateActionJson(fileContents),
                _ => throw new InvalidOperationException($"Unknown policy file type {docTypeString}")
            };
        }

        public static bool TryFromFile(string path, out ITicTacToePolicy? policy)
        {
            try
            {
                policy = FromFile(path);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                policy = null;
                return false;
            }
        }

        public static StateValueTable LoadStateValueTable(string path)
        {
            var policy = FromStateValueJson(File.ReadAllText(path));
            var table = new StateValueTable(policy.Tile);

            foreach (var (board, value) in policy.Values)
            {
                table.SetValue(Board.CreateFromString(board), value);
            }

            return table;
        }

        public static ActionValueTable LoadStateActionTable(string path)
        {
            var policy = FromStateActionJson(File.ReadAllText(path));
            var table = new ActionValueTable();

            foreach (var (board, value, position) in policy.Actions)
            {
                var action = new TicTacToeAction {Position = position, Tile = policy.Tile};
                table.Set(Board.CreateFromString(board), action, value);
            }

            return table;
        }

        private static SerializableStateActionPolicy FromStateActionJson(string text)
        {
            var file = JsonSerializer.Deserialize<SerializableStateActionPolicy>(text, BuildJsonOptions());

            if (file == null) throw new InvalidOperationException("Policy file deserialised to null :(");

            return file;
        }

        private static SerializableStateValueTable FromStateValueJson(string text)
        {
            var file = JsonSerializer.Deserialize<SerializableStateValueTable>(text, BuildJsonOptions());

            if (file == null) throw new InvalidOperationException("Policy file deserialised to null :(");

            return file;
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