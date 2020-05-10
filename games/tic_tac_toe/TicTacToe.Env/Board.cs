using System;
using System.Linq;
using System.Text;

namespace TicTacToe.Env
{
    public class Board : IBoard
    {
        private readonly BoardTile[] _tiles;

        public Board()
        {
            _tiles = new[]
            {
                BoardTile.Empty, BoardTile.Empty, BoardTile.Empty,
                BoardTile.Empty, BoardTile.Empty, BoardTile.Empty,
                BoardTile.Empty, BoardTile.Empty, BoardTile.Empty,
            };
        }

        private Board(BoardTile[] tiles)
        {
            _tiles = tiles;
        }

        public static Board CreateFromString(string values)
        {
            if (values.Length != 9) throw new ArgumentException("Must have 9 characters");

            var chars = values.ToLowerInvariant().ToCharArray();
            var board = new Board();
            for (var i = 0; i < 9; i++)
            {
                var c = chars[i];
                switch (c)
                {
                    case 'x': board._tiles[i] = BoardTile.X; break;
                    case 'o': board._tiles[i] = BoardTile.O; break;
                    default: board._tiles[i] = BoardTile.Empty; break;
                }
            }

            return board;
        }

        public void Update(TicTacToeAction action)
        {
            RangeCheck(action.Position);
            ThrowIfNotEmpty(action.Position);

            _tiles[action.Position] = action.Tile;
        }

        public string AsString()
        {
            var sb = new StringBuilder();

            foreach (var tile in _tiles)
            {
                switch (tile)
                {
                    case BoardTile.Empty: sb.Append(' '); break;
                    case BoardTile.O: sb.Append('o'); break;
                    case BoardTile.X: sb.Append('x'); break;
                }
            }

            return sb.ToString();
        }

        public IBoard Clone()
        {
            var newTiles = _tiles.Select(t => t).ToArray();
            return new Board(newTiles);
        }

        public bool IsSameStateAs(IBoard otherBoard)
        {
            for (var i = 0; i < 9; i++)
            {
                if (GetTileAt(i) != otherBoard.GetTileAt(i)) return false;
            }

            return true;
        }

        public bool IsFull()
        {
            return _tiles.All(tile => tile != BoardTile.Empty);
        }

        public bool IsValid()
        {
            var numOs = _tiles.Count(t => t == BoardTile.O);
            var numXs = _tiles.Count(t => t == BoardTile.X);
            if (numOs > 5) return false;
            if (numXs > 5) return false;
            if (Math.Abs(numOs - numXs) > 1) return false;
            if (WinnerInternal() == WinnerState.BothWon) return false;

            return true;
        }

        public BoardTile? Winner()
        {
            var winner = WinnerInternal();

            if (winner == WinnerState.BothWon)
                throw new InvalidOperationException("Multiple winners!");

            if (winner == WinnerState.OWon) return BoardTile.O;
            if (winner == WinnerState.XWon) return BoardTile.X;
            return null;
        }

        public BoardTile GetTileAt(int pos)
        {
            RangeCheck(pos);
            return _tiles[pos];
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append(_tiles[0].AsString());
            sb.Append(_tiles[1].AsString());
            sb.Append(_tiles[2].AsString());
            sb.AppendLine();
            sb.Append(_tiles[3].AsString());
            sb.Append(_tiles[4].AsString());
            sb.Append(_tiles[5].AsString());
            sb.AppendLine();
            sb.Append(_tiles[6].AsString());
            sb.Append(_tiles[7].AsString());
            sb.Append(_tiles[8].AsString());

            return sb.ToString();
        }

        private WinnerState WinnerInternal()
        {
            var lines = new[]
            {
                new[] {0, 1, 2},
                new[] {3, 4, 5},
                new[] {6, 7, 8},
                new[] {0, 3, 6},
                new[] {1, 4, 7},
                new[] {2, 5, 8},
                new[] {0, 4, 8},
                new[] {2, 4, 6},
            };
            var hasXWon = false;
            var hasOWon = false;
            foreach (var line in lines)
            {
                var tiles = line.Select(i => _tiles[i]).ToArray();
                if (tiles.All(t => t == BoardTile.O)) hasOWon = true;
                if (tiles.All(t => t == BoardTile.X)) hasXWon = true;
            }

            if (hasOWon && hasXWon) return WinnerState.BothWon;
            if (hasOWon) return WinnerState.OWon;
            if (hasXWon) return WinnerState.XWon;
            return WinnerState.NoWinner;
        }

        private void ThrowIfNotEmpty(int pos)
        {
            if (!IsEmpty(pos)) throw new ArgumentException($"Position {pos} isn't empty");
        }

        private bool IsEmpty(int pos)
        {
            return _tiles[pos] == BoardTile.Empty;
        }

        private static void RangeCheck(int pos)
        {
            if (pos < 0 || pos > 8) throw new ArgumentException($"Invalid position: {pos}");
        }

        private enum WinnerState
        {
            XWon,
            OWon,
            BothWon,
            NoWinner
        }
    }
}
