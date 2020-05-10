using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using TicTacToe.Env;

namespace TicTacToe.Agent.Test
{
    public class PTableAgentTests
    {
        private static readonly PTableAgentConfig AgentConfig = new PTableAgentConfig
        {
            PlayerTile = BoardTile.O,
            RandomActionProbability = 0.0,
            LearningRate = 1.0
        };
        private static readonly Board InitialBoard = new Board();
        private const double CurrentBoardWinProbability = 0.5;

        private ITicTacToePTable _pTable;
        private ITicTacToeGame _game;

        private RlBookPTableAgent _agent;

        [SetUp]
        public void Setup()
        {
            _game = Substitute.For<ITicTacToeGame>();
            _game.Board.Returns(InitialBoard);
            _pTable = new TicTacToePTableFake();
            _pTable.UpdateWinProbability(InitialBoard, CurrentBoardWinProbability);
            _agent = new RlBookPTableAgent(_pTable, AgentConfig);
        }

        [Test]
        public void GivenNoRandomActions_AgentShouldPickHighestActionWithHighestProb()
        {
            var highProbAction = new TicTacToeAction {Tile = BoardTile.O, Position = 0};
            var lowProbAction = new TicTacToeAction {Tile = BoardTile.O, Position = 1};

            _game.GetAvailableActions().Returns(new[] {lowProbAction, highProbAction});

            _pTable.UpdateWinProbability(Board.CreateFromString("o        "), 0.9);
            _pTable.UpdateWinProbability(Board.CreateFromString(" o       "), 0.1);

            // act
            var action = _agent.GetAction(_game);

            // assert
            Assert.AreEqual(highProbAction.Tile, action.Tile);
            Assert.AreEqual(highProbAction.Position, action.Position);
        }

        [Test]
        public void GivenNoRandomActions_AgentShouldUpdatePTable()
        {
            var nextBoard = Board.CreateFromString("o        ");
            const double nextBoardWinProbability = 1.0;
            _pTable.UpdateWinProbability(nextBoard, nextBoardWinProbability);

            var onlyAction = new TicTacToeAction { Tile = BoardTile.O, Position = 0 };
            _game.GetAvailableActions().Returns(new[] { onlyAction });

            // act
            var action = _agent.GetAction(_game);

            // assert
            // P(t) = P(t) + LearningRate * (P(t + 1) - P(t))
            //      = 0.5  +          1.0 * (1.0      - 0.5)
            //      = 0.5 + 0.5
            //      = 1.0
            const double expectedNewProbability = 1.0;
            Assert.AreEqual(expectedNewProbability, _pTable.GetWinProbability(InitialBoard));
        }
    }

    public class TicTacToePTableFake : ITicTacToePTable
    {
        private readonly Dictionary<string, double> _pTable = new Dictionary<string, double>();

        public double GetWinProbability(IBoard board)
        {
            return _pTable[board.AsString()];
        }

        public void UpdateWinProbability(IBoard board, double winProbability)
        {
            _pTable[board.AsString()] = winProbability;
        }
    }
}
