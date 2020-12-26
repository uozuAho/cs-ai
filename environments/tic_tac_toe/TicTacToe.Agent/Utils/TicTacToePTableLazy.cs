using System;
using System.Collections.Generic;
using System.Linq;
using TicTacToe.Game;

namespace TicTacToe.Agent.Utils
{
    [Obsolete("Use " + nameof(StateValueTable))]
    public class TicTacToePTableLazy : ITicTacToePTable
    {
        private readonly BoardTile _playerTile;
        private readonly Dictionary<string, double> _pTable;

        public TicTacToePTableLazy(BoardTile playerTile)
        {
            _playerTile = playerTile;
            _pTable = new Dictionary<string, double>();
        }

        public IEnumerable<(Board, double)> All()
        {
            return _pTable.Select(item =>
                (Board.CreateFromString(item.Key), item.Value)
            );
        }

        public double GetWinProbability(Board board)
        {
            if (!board.IsValid())
                throw new ArgumentException("invalid board: " + board);

            var key = board.ToString();

            if (_pTable.ContainsKey(key))
                return _pTable[key];

            var winProbability = CalculateWinProbability(board);

            _pTable[key] = winProbability;
            return winProbability;
        }

        public void UpdateWinProbability(Board board, double winProbability)
        {
            if (!board.IsValid()) throw new ArgumentException("invalid board: " + board);

            _pTable[board.ToString()] = winProbability;
        }

        private double CalculateWinProbability(Board board)
        {
            var winner = board.Winner();
            var isFinished = board.IsFull();

            if (winner == _playerTile)         return 1.0;
            if (winner == _playerTile.Other()) return 0.0;
            if (isFinished)                    return 0.0;  // draw

            // no winner, not finished
            return 0.5;
        }
    }
}
