using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            AddPlayer(nameof(FirstAvailableSlotAgent), tile => new FirstAvailableSlotAgent(tile));
            AddPlayer(nameof(RlBookPTableAgent), RlBookPTableAgent.CreateDefaultAgent);
            AddPlayer(nameof(RlBookModifiedPTableAgent), RlBookModifiedPTableAgent.CreateDefaultAgent);
            AddPlayer(nameof(RandomTicTacToeAgent), tile => new RandomTicTacToeAgent(tile));
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
            foreach (var filename in Directory.EnumerateFiles(".", "*.agent.json"))
            {
                var agentName = filename
                    .Replace(".\\", "")
                    .Replace(".agent.json", "");

                var policy = BoardActionMap.LoadFromFile(filename);
                AddPlayer(agentName, tile => new TicTacToeFixedPolicyPlayer(tile, policy));
            }
        }
    }
}