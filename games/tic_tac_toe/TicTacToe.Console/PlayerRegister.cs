using System;
using System.Collections.Generic;
using System.IO;
using TicTacToe.Agent.Agents;
using TicTacToe.Game;

namespace TicTacToe.Console
{
    public class PlayerRegister
    {
        private readonly Dictionary<string, Func<BoardTile, ITicTacToePlayer>> _players = new();

        public PlayerRegister()
        {
            AddPlayer(nameof(ConsoleInputTicTacToePlayer), tile => new ConsoleInputTicTacToePlayer(tile));
            AddPlayer(nameof(FirstAvailableSlotAgent), tile => new FirstAvailableSlotAgent(tile));
            AddPlayer(nameof(RlBookPTableAgent), RlBookPTableAgent.CreateDefaultAgent);
            AddPlayer(nameof(RlBookModifiedPTableAgent), RlBookModifiedPTableAgent.CreateDefaultAgent);
        }

        public ITicTacToePlayer GetPlayerByKey(string key, BoardTile playerTile)
        {
            switch (key)
            {
                case "a": return new ConsoleInputTicTacToePlayer(playerTile);
                case "b": return new FirstAvailableSlotAgent(playerTile);
                case "c": return RlBookPTableAgent.CreateDefaultAgent(playerTile);
                case "d": return RlBookModifiedPTableAgent.CreateDefaultAgent(playerTile);
            }
            throw new InvalidOperationException($"Unknown ticTacToePlayer key: {key}");
        }

        public IEnumerable<string> AvailablePlayers()
        {
            return _players.Keys;
        }

        public ITicTacToePlayer GetPlayerByName(string name, BoardTile playerTile)
        {
            return _players[name](playerTile);
        }

        public void AddPlayer(string name, Func<BoardTile, ITicTacToePlayer> player)
        {
            _players[name] = player;
        }

        public void LoadPolicyFiles()
        {
            foreach (var filename in Directory.EnumerateFiles(".", "*.json"))
            {
                var name = filename.Replace(".\\", "").Replace(".json", "");
                AddPlayer(name, tile => new FirstAvailableSlotAgent(tile));
            }
        }
    }
}