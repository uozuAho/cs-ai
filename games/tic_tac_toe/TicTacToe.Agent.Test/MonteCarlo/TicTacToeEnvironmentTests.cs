using System;
using System.Linq;
using NUnit.Framework;
using TicTacToe.Agent.MonteCarlo;
using TicTacToe.Game;

namespace TicTacToe.Agent.Test.MonteCarlo
{
    public class TicTacToeEnvironmentTests
    {
        private TicTacToeEnvironment _env;

        [SetUp]
        public void Setup()
        {
            _env = new TicTacToeEnvironment();
        }

        [Test]
        public void Reset_ClearsBoard()
        {
            var observation = _env.Reset();

            Assert.True(Board.CreateEmptyBoard().IsSameStateAs(observation.Board));
        }

        [Test]
        public void Step_ModifiesBoard()
        {
            var placeXAtTopLeft = new TicTacToeAction {Position = 0, Tile = BoardTile.X};
            var expectedBoard = Board.CreateEmptyBoard();
            expectedBoard.Update(placeXAtTopLeft);

            // act
            var observation = _env.Step(placeXAtTopLeft);

            // assert
            Assert.AreEqual(expectedBoard.AsString(), observation.Board.AsString());
        }

        [Test]
        public void PlacingTileOnNonEmptySquare_Throws()
        {
            _env.SetState(Board.CreateFromString("x  " +
                                                 "   " +
                                                 "   "));

            var placeXAtTopLeft = new TicTacToeAction { Position = 0, Tile = BoardTile.X };

            Assert.Throws<InvalidOperationException>(() => _env.Step(placeXAtTopLeft));
        }

        [Test]
        public void CreatingInvalidState_Throws()
        {
            _env.SetState(Board.CreateFromString("x  " +
                                                 "   " +
                                                 "   "));

            var placeXAtTopLeft = new TicTacToeAction { Position = 1, Tile = BoardTile.X };

            Assert.Throws<InvalidOperationException>(() => _env.Step(placeXAtTopLeft));
        }

        [Test]
        public void RewardIs1_ForWin()
        {
            _env.SetState(Board.CreateFromString("xx " +
                                                 "oo " +
                                                 "   "));

            var placeXAtTopRight = new TicTacToeAction { Position = 2, Tile = BoardTile.X };

            // act
            var observation = _env.Step(placeXAtTopRight);

            // assert
            Assert.AreEqual(1, observation.Reward);
        }

        [Test]
        public void RewardIsNegative1_ForLoss()
        {
            _env.SetState(Board.CreateFromString("xx " +
                                                 "oo " +
                                                 "   "));

            var placeOAtMiddleRight = new TicTacToeAction { Position = 5, Tile = BoardTile.O };

            // act
            var observation = _env.Step(placeOAtMiddleRight);

            // assert
            Assert.AreEqual(-1, observation.Reward);
        }

        [Test]
        public void RewardIs0_WhenGameIsNotOver()
        {
            var placeOAtMiddleRight = new TicTacToeAction { Position = 5, Tile = BoardTile.O };

            // act
            var observation = _env.Step(placeOAtMiddleRight);

            // assert
            Assert.AreEqual(0, observation.Reward);
        }

        [Test]
        public void NewEnv_Has9AvailableActions()
        {
            Assert.AreEqual(9, _env.AvailableActions().Count());
        }

        // env assumes agent uses X tiles
        // note that playing these available actions will result in invalid states
        [TestCase("x        ")]
        [TestCase("xox      ")]
        public void AvailableActions_AreAlwaysX(string board)
        {
            _env.SetState(Board.CreateFromString(board));

            Assert.True(_env.AvailableActions().All(a => a.Tile == BoardTile.X));
        }
    }
}
