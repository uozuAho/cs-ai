using System;
using System.Collections.Generic;
using System.IO;
using TicTacToe.Agent;
using TicTacToe.Game;

namespace TicTacToe.Console
{
    public class PlayerRegister
    {
        private readonly Dictionary<string, Func<BoardTile, IPlayer>> _players = new();

        public PlayerRegister()
        {
            AddPlayer(nameof(ConsoleInputPlayer), tile => new ConsoleInputPlayer(tile));
            AddPlayer(nameof(FirstAvailableSlotAgent), tile => new FirstAvailableSlotAgent(tile));
            AddPlayer(nameof(RlBookPTableAgent), RlBookPTableAgent.CreateDefaultAgent);
            AddPlayer(nameof(RlBookModifiedPTableAgent), RlBookModifiedPTableAgent.CreateDefaultAgent);
        }

        public IPlayer GetPlayerByKey(string key, BoardTile playerTile)
        {
            switch (key)
            {
                case "a": return new ConsoleInputPlayer(playerTile);
                case "b": return new FirstAvailableSlotAgent(playerTile);
                case "c": return RlBookPTableAgent.CreateDefaultAgent(playerTile);
                case "d": return RlBookModifiedPTableAgent.CreateDefaultAgent(playerTile);
            }
            throw new InvalidOperationException($"Unknown player key: {key}");
        }

        public IEnumerable<string> AvailablePlayers()
        {
            yield return "a: ConsoleInputPlayer";
            yield return "b: FirstAvailableSlotAgent";
            yield return "c: PTableAgent";
            yield return "d: ModifiedPTableAgent";
        }

        public IPlayer GetPlayerByName(string name, BoardTile playerTile)
        {
            return _players[name](playerTile);
        }

        public void AddPlayer(string name, Func<BoardTile, IPlayer> player)
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