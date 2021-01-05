using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TicTacToe.Agent.Utils
{
    public class PolicyFileLoader
    {
        public static void Save(IPolicyFile file, string path)
        {
            if (file is PolicyFile policyFile)
                File.WriteAllText(path, JsonSerializer.Serialize(policyFile, BuildJsonOptions()));
            else
            {
                throw new InvalidOperationException("Unknown policy");
            }
        }

        public static IPolicyFile FromFile(string path)
        {
            var fileContents = File.ReadAllText(path);
            return FromJsonString(fileContents);
        }

        public static IPolicyFile FromJsonString(string text)
        {
            var file = JsonSerializer.Deserialize<PolicyFile>(text, BuildJsonOptions());

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