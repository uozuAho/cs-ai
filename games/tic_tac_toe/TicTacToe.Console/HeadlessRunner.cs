using System.Collections.Generic;
using System.Linq;
using TicTacToe.Game;

namespace TicTacToe.Console
{
    class HeadlessRunner
    {
        private readonly ITicTacToePlayer _player1;
        private readonly ITicTacToePlayer _player2;
        private readonly List<BoardTile?> _winnerRecord;

        public HeadlessRunner(ITicTacToePlayer player1, ITicTacToePlayer player2)
        {
            _player1 = player1;
            _player2 = player2;
            _winnerRecord = new List<BoardTile?>();
        }

        public int NumberOfGames => _winnerRecord.Count;

        public void PlayGames(int numberOfGames)
        {
            for (var i = 0; i < numberOfGames; i++)
            {
                var game = RunSingleGame(_player1, _player2);
                _winnerRecord.Add(game.Winner());
            }
        }

        public int NumberOfWins(BoardTile tile)
        {
            return _winnerRecord.Count(winner => winner == tile);
        }

        private static TicTacToeGame RunSingleGame(ITicTacToePlayer player1, ITicTacToePlayer player2)
        {
            var game = new TicTacToeGame(Board.CreateEmptyBoard(), player1, player2);

            while (!game.IsFinished())
            {
                game.DoNextTurn();
            }

            return game;
        }
    }
}
