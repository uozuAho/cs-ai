using System;
using System.Collections.Generic;
using System.IO;
using TicTacToe.Agent.Agents;
using TicTacToe.Agent.Utils;
using TicTacToe.Game;

namespace TicTacToe.Console
{
    public class PlayerRegister
    {
        private readonly Dictionary<string, Func<BoardTile, ITicTacToePlayer>> _players = new();

        public PlayerRegister()
        {
            AddPlayer(nameof(ConsoleInputTicTacToePlayer), tile => new ConsoleInputTicTacToePlayer(tile));
            AddPlayer(nameof(FirstAvailableSlotPlayer), tile => new FirstAvailableSlotPlayer(tile));
            AddPlayer(nameof(RandomTicTacToePlayer), tile => new RandomTicTacToePlayer(tile));
        }

        public IEnumerable<string> AvailablePlayers()
        {
            return _players.Keys;
        }

        public bool TryFindPlayer(string name, BoardTile tile, out ITicTacToePlayer? player)
        {
            try
            {
                player = GetPlayerByName(name, tile);
                return true;
            }
            catch (KeyNotFoundException)
            {
                player = null;
                return false;
            }
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
            foreach (var filename in Directory.EnumerateFiles(".", "*.agent.json"))
            {
                var agentName = filename
                    .Replace(".\\", "")
                    .Replace(".agent.json", "");

                var policy = PolicyFileIo.FromFile(filename);
                AddPlayer(agentName, tile =>
                {
                    if (policy.Tile != tile) throw new ArgumentException("tile does not match policy tile");

                    return policy.ToPlayer();
                });
            }
        }
    }
}