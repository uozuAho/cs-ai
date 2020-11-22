using System.Collections.Generic;

namespace TicTacToe.Game.Test.Utils
{
    public class TestPlayer : IPlayer
    {
        public BoardTile Tile { get; }

        private readonly Queue<int> _moves;

        public TestPlayer(BoardTile playerTile)
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

        public TicTacToeAction GetAction(ITicTacToeGame game)
        {
            return new TicTacToeAction
            {
                Tile = Tile,
                Position = _moves.Dequeue()
            };
        }
    }
}