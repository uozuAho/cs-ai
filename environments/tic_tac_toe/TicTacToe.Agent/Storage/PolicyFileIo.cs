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
        public static void Save(ITicTacToePolicy file, string path)
        {
            switch (file)
            {
                case SerializableStateActionPolicy policyFile:
                    File.WriteAllText(path, JsonSerializer.Serialize(policyFile, BuildJsonOptions()));
                    break;
                case SerializableStateValuePolicy policyFile:
                    File.WriteAllText(path, JsonSerializer.Serialize(policyFile, BuildJsonOptions()));
                    break;
                default:
                    throw new InvalidOperationException("Unknown policy");
            }
        }

        public static void Save(StateValueTable stateValues, string name, string description, string path)
        {
            var s = new SerializableStateValuePolicy(name, description, stateValues.Tile);
            s.SetStateValues(stateValues);
            Save(s, path);
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

        private static SerializableStateActionPolicy FromStateActionJson(string text)
        {
            var file = JsonSerializer.Deserialize<SerializableStateActionPolicy>(text, BuildJsonOptions());

            if (file == null) throw new InvalidOperationException("Policy file deserialised to null :(");

            return file;
        }

        private static SerializableStateValuePolicy FromStateValueJson(string text)
        {
            var file = JsonSerializer.Deserialize<SerializableStateValuePolicy>(text, BuildJsonOptions());

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