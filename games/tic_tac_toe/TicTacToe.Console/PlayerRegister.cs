using System;
using System.Collections.Generic;
using TicTacToe.Agent;
using TicTacToe.Game;

namespace TicTacToe.Console
{
    public class PlayerRegister
    {
        public IPlayer NewPlayer(string key, BoardTile playerTile)
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
    }
}