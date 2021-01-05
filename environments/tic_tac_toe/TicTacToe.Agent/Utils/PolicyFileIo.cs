﻿using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TicTacToe.Agent.Utils
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
                case StateValuePolicy policyFile:
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
            var docType = doc.RootElement.GetProperty("Type");

            return FromJsonString(fileContents);
        }

        public static ITicTacToePolicy FromJsonString(string text)
        {
            var file = JsonSerializer.Deserialize<SerializableStateActionPolicy>(text, BuildJsonOptions());

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