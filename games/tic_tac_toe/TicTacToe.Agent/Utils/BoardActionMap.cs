﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
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
            var actions = JsonSerializer
                .Deserialize<List<SerializableStateAction>>(fileContents);

            if (actions == null) throw new InvalidOperationException("loaded actions must not be null");

            var map = new BoardActionMap();
            foreach (var (serializableBoard, serializableAction) in actions)
            {
                map.SetAction(serializableBoard.ToBoard(), serializableAction.ToAction());
            }

            return map;
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

        public void SaveToFile(string path)
        {
            var serializableActions = AllActions().Select(a => new SerializableStateAction(
                SerializableBoard.FromBoard(a.Item1),
                SerializableTicTacToeAction.FromAction(a.Item2)));

            File.WriteAllText(path, JsonSerializer.Serialize(
                serializableActions, new JsonSerializerOptions {WriteIndented = true}));
        }
    }
}