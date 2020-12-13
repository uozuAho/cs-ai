using System.Collections.Generic;

namespace TicTacToe.Game.Test.Utils
{
    public class TestTicTacToePlayer : ITicTacToePlayer
    {
        public BoardTile Tile { get; }

        private readonly Queue<int> _moves;

        public TestTicTacToePlayer(BoardTile playerTile)
        {
            Tile = playerTile;
            _moves = new Queue<int>();
        }

        public void SetMoves(IEnumerable<int> positions)
        {
            _moves.Clear();
            foreach (var position in positions)
            {
                _moves.Enqueue(position);
            }
        }

        public TicTacToeAction GetAction(Board board)
        {
            return new TicTacToeAction
            {
                Tile = Tile,
                Position = _moves.Dequeue()
            };
        }
    }
}