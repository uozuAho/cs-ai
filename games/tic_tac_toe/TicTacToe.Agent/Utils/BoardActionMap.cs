using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using TicTacToe.Game;

namespace TicTacToe.Agent.Utils
{
    public class BoardActionMap
    {
        private readonly Dictionary<string, TicTacToeAction> _actionMap = new();
        public int NumStates => _actionMap.Count;

        public static BoardActionMap LoadFromFile(string path)
        {
            var fileContents = File.ReadAllText(path);
            return FromJsonString(fileContents);
        }

        public TicTacToeAction ActionFor(Board board)
        {
            return _actionMap[board.ToString()];
        }

        public void SetAction(Board state, TicTacToeAction action)
        {
            _actionMap[state.ToString()] = action;
        }

        public bool HasActionFor(Board board)
        {
            return _actionMap.ContainsKey(board.ToString());
        }

        public IEnumerable<(Board, TicTacToeAction)> AllActions()
        {
            return _actionMap.Select(action => (Board.CreateFromString(action.Key), action.Value));
        }

        public void SaveToFile(string path, BoardTile tile)
        {
            var policyFile = PolicyFile.FromBoardActionMap("", "", tile, this);

            File.WriteAllText(path, JsonSerializer.Serialize(policyFile, BuildJsonOptions()));
        }

        public static BoardActionMap FromJsonString(string text)
        {
            var deser = JsonSerializer.Deserialize<PolicyFile>(text, BuildJsonOptions());

            if (deser == null) throw new InvalidOperationException("map deserialised to null :(");

            var map = new BoardActionMap();
            foreach (var (boardString, _, position) in deser.Actions)
            {
                var board = Board.CreateFromString(boardString, deser.Tile);
                var action = new TicTacToeAction {Tile = deser.Tile, Position = position};
                map.SetAction(board, action);
            }

            return map;
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