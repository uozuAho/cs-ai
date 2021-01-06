using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using TicTacToe.Agent.Utils;

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

        public static SerializableStateActionPolicy FromStateActionJson(string text)
        {
            var file = JsonSerializer.Deserialize<SerializableStateActionPolicy>(text, BuildJsonOptions());

            if (file == null) throw new InvalidOperationException("Policy file deserialised to null :(");

            return file;
        }

        public static SerializableStateValuePolicy FromStateValueJson(string text)
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