using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace TicTacToe.Game
{
    public class Board
    {
        public BoardTile CurrentPlayer { get; private init; }
        public bool IsGameOver => Winner().HasValue || IsFull();

        private readonly ImmutableArray<BoardTile> _tiles;

        private Board()
        {
            _tiles = ImmutableArray.Create(
                BoardTile.Empty,
                BoardTile.Empty,
                BoardTile.Empty,
                BoardTile.Empty,
                BoardTile.Empty,
                BoardTile.Empty,
                BoardTile.Empty,
                BoardTile.Empty,
                BoardTile.Empty
            );
        }

        public static Board CreateEmptyBoard() => new() {CurrentPlayer = BoardTile.X};

        public static Board CreateEmptyBoard(BoardTile currentPlayer) => new() { CurrentPlayer = currentPlayer };

        private Board(IEnumerable<BoardTile> tiles, BoardTile currentPlayer = BoardTile.X)
        {
            _tiles = tiles.Select(t => t).ToImmutableArray();
            CurrentPlayer = currentPlayer;
        }

        public Board Clone()
        {
            return new(_tiles) {CurrentPlayer = CurrentPlayer};
        }

        public static Board CreateFromString(string values, BoardTile currentPlayer = BoardTile.X)
        {
            if (values.Length != 11) throw new ArgumentException("Must have 11 characters");

            var chars = values.ToLowerInvariant().Replace("|", "").ToCharArray();

            return new Board(chars.Select(c => c switch
            {
                'x' => BoardTile.X,
                'o' => BoardTile.O,
                _ => BoardTile.Empty
            }), currentPlayer);
        }

        public IEnumerable<TicTacToeAction> AvailableActions()
        {
            for (var pos = 0; pos < 9; pos++)
            {
                if (GetTileAt(pos) == BoardTile.Empty)
                {
                    yield return new TicTacToeAction
                    {
                        Position = pos,
                        Tile = CurrentPlayer
                    };
                }
            }
        }

        public Board DoAction(TicTacToeAction action)
        {
            ThrowIfIncorrectTile(action);
            RangeCheck(action.Position);
            ThrowIfNotEmpty(action.Position);

            var tiles = _tiles.Select(x => x).ToArray();
            tiles[action.Position] = action.Tile;

            return new Board(tiles) {CurrentPlayer = CurrentPlayer.Other()};
        }

        protected bool Equals(Board other)
        {
            if (CurrentPlayer != other.CurrentPlayer) return false;

            for (var i = 0; i < 9; i++)
            {
                if (_tiles[i] != other._tiles[i]) return false;
            }

            return true;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Board) obj);
        }

        public override int GetHashCode()
        {
            var tilesHash = 19;

            unchecked
            {
                foreach (var tile in _tiles)
                {
                    tilesHash = tilesHash * 31 + tile.GetHashCode();
                }
            }

            return HashCode.Combine(tilesHash, (int) CurrentPlayer);
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
            sb.Append('|');
            sb.Append(_tiles[3].AsString());
            sb.Append(_tiles[4].AsString());
            sb.Append(_tiles[5].AsString());
            sb.Append('|');
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

        private void ThrowIfIncorrectTile(TicTacToeAction action)
        {
            if (action.Tile != CurrentPlayer)
            {
                throw new InvalidOperationException($"It's not {action.Tile}'s turn!");
            }
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
