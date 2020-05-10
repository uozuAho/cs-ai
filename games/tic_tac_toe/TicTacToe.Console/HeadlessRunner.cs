using System.Collections.Generic;
using System.Linq;
using TicTacToe.Env;

namespace TicTacToe.Console
{
    class HeadlessRunner
    {
        private readonly IPlayer _player1;
        private readonly IPlayer _player2;
        private readonly List<BoardTile?> _winnerRecord;

        public HeadlessRunner(IPlayer player1, IPlayer player2)
        {
            _player1 = player1;
            _player2 = player2;
            _winnerRecord = new List<BoardTile?>();
        }

        public void PlayGames(int numberOfGames)
        {
            for (var i = 0; i < numberOfGames; i++)
            {
                var game = RunSingleGame(_player1, _player2);
                _winnerRecord.Add(game.Winner());
            }
        }

        public int NumberOfWins(BoardTile tile, int lastNGames)
        {
            return _winnerRecord
                .TakeLast(lastNGames)
                .Count(winner => winner == tile);
        }

        private static TicTacToeGame RunSingleGame(IPlayer player1, IPlayer player2)
        {
            var game = new TicTacToeGame(new Board(), player1, player2);

            while (!game.IsFinished())
            {
                game.DoNextTurn();
            }

            return game;
        }
    }
}
