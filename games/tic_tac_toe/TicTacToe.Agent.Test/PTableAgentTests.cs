using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using TicTacToe.Game;

namespace TicTacToe.Agent.Test
{
    public class PTableAgentTests
    {
        private static readonly PTableAgentConfig AgentConfig = new()
        {
            PlayerTile = BoardTile.O,
            RandomActionProbability = 0.0,
            LearningRate = 1.0
        };

        private static readonly Board InitialBoard = Board.CreateEmptyBoard();
        private const double CurrentBoardWinProbability = 0.5;

        private ITicTacToePTable _pTable;
        private TicTacToeGame _game;

        private RlBookPTableAgent _agent;

        [SetUp]
        public void Setup()
        {
            _game = NewGame();
            _pTable = new TicTacToePTableFake();
            _pTable.UpdateWinProbability(InitialBoard, CurrentBoardWinProbability);
            _agent = new RlBookPTableAgent(_pTable, AgentConfig);
        }

        [Test]
        public void GivenNoRandomActions_AgentShouldPickHighestActionWithHighestProbability()
        {
            var board = Board.CreateFromString("oxo|" +
                                               "xox|" +
                                               "   ",
                                                BoardTile.O);

            _pTable.UpdateWinProbability(board, 0.0);
            _pTable.UpdateWinProbability(Board.CreateFromString("oxo|xox|o  "), 0.8);
            _pTable.UpdateWinProbability(Board.CreateFromString("oxo|xox| o "), 0.2);
            _pTable.UpdateWinProbability(Board.CreateFromString("oxo|xox|  o"), 0.2);

            // act
            var action = _agent.GetAction(board);

            // assert
            Assert.AreEqual(6, action.Position);
        }

        [Test]
        public void GivenNoRandomActions_AgentShouldUpdatePTable()
        {
            var currentBoard = Board.CreateFromString("oxo|" +
                                                      "xox|" +
                                                      "xo ",
                                                        BoardTile.O);

            var nextBoard = Board.CreateFromString("oxo|" +
                                                   "xox|" +
                                                   "xoo");
            
            _pTable.UpdateWinProbability(currentBoard, 0.5);
            _pTable.UpdateWinProbability(nextBoard, 1.0);

            // act
            var action = _agent.GetAction(currentBoard);

            // assert
            // P(t) = P(t) + LearningRate * (P(t + 1) - P(t))
            //      = 0.5  +          1.0 * (1.0      - 0.5)
            //      = 0.5 + 0.5
            //      = 1.0
            const double expectedNewProbability = 1.0;
            Assert.AreEqual(expectedNewProbability, _pTable.GetWinProbability(currentBoard));
        }

        private static TicTacToeGame NewGame()
        {
            return new(
                Board.CreateEmptyBoard(),
                new FirstAvailableSlotAgent(BoardTile.X),
                new FirstAvailableSlotAgent(BoardTile.O));
        }
    }

    public class TicTacToePTableFake : ITicTacToePTable
    {
        private readonly Dictionary<string, double> _pTable = new();

        public double GetWinProbability(Board board)
        {
            return _pTable[board.ToString()];
        }

        public void UpdateWinProbability(Board board, double winProbability)
        {
            _pTable[board.ToString()] = winProbability;
        }
    }
}
