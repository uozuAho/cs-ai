using System;
using System.Collections.Generic;
using System.Linq;
using TicTacToe.Game;

namespace TicTacToe.Agent.Utils
{
    public class StateValueTable
    {
        private readonly BoardTile _playerTile;
        private readonly Dictionary<Board, double> _values;

        public StateValueTable(BoardTile playerTile)
        {
            _playerTile = playerTile;
            _values = new Dictionary<Board, double>();
        }

        public IEnumerable<(Board, double)> All()
        {
            // tolist prevents modifying _values via `Value()` during enumeration
            return _values.Select(v => (v.Key, v.Value)).ToList();
        }

        public double Value(Board board)
        {
            if (!board.IsValid())
                throw new ArgumentException("invalid board: " + board);

            if (_values.ContainsKey(board))
                return _values[board];

            var winProbability = InitialValue(board);

            _values[board] = winProbability;
            return winProbability;
        }

        public void SetValue(Board board, double winProbability)
        {
            if (!board.IsValid()) throw new ArgumentException("invalid board: " + board);

            _values[board] = winProbability;
        }

        private double InitialValue(Board board)
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
