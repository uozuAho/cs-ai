using System;
using System.Collections.Generic;
using TicTacToe.Game;

namespace TicTacToe.Agent
{
    public class TicTacToePTable : ITicTacToePTable
    {
        private readonly Dictionary<string, double> _pTable;

        public TicTacToePTable(BoardTile playerTile)
        {
            _pTable = CreateNewPTable(playerTile);
        }

        public double GetWinProbability(IBoard board)
        {
            if (!board.IsValid()) throw new ArgumentException("invalid board: " + board);
            return _pTable[board.AsString()];
        }

        public void UpdateWinProbability(IBoard board, double winProbability)
        {
            if (!board.IsValid()) throw new ArgumentException("invalid board: " + board);
            _pTable[board.AsString()] = winProbability;
        }

        private static Dictionary<string, double> CreateNewPTable(BoardTile playerTile)
        {
            var table = new Dictionary<string, double>();

            foreach (var state in GenerateAllStatesOfLength(9))
            {
                var board = Board.CreateFromString(state);
                if (!board.IsValid())
                    continue;

                double p;
                var winner = board.Winner();
                if (winner == playerTile) p = 1;
                else if (winner == null) p = 0.5;
                else p = 0;

                table[state] = p;
            }

            return table;
        }

        private static IEnumerable<string> GenerateAllStatesOfLength(int i)
        {
            if (i <= 1)
            {
                yield return "x";
                yield return "o";
                yield return " ";
            }
            else
            {
                foreach (var tile in new[] {"x", "o", " "})
                {
                    foreach (var state in GenerateAllStatesOfLength(i - 1))
                    {
                        yield return tile + state;
                    }
                }
            }
        }
    }
}
