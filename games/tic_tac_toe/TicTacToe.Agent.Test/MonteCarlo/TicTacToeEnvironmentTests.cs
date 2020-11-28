using System;
using NUnit.Framework;
using TicTacToe.Agent.MonteCarlo;
using TicTacToe.Game;

namespace TicTacToe.Agent.Test.MonteCarlo
{
    public class TicTacToeEnvironmentTests
    {
        [Test]
        public void Reset_ClearsBoard()
        {
            var env = new TicTacToeEnvironment();

            var observation = env.Reset();

            Assert.True(Board.CreateEmptyBoard().IsSameStateAs(observation.Board));
        }

        [Test]
        public void Step_ModifiesBoard()
        {
            var env = new TicTacToeEnvironment();
            env.Reset();

            var placeXAtTopLeft = new TicTacToeAction {Position = 0, Tile = BoardTile.X};
            var expectedBoard = Board.CreateEmptyBoard();
            expectedBoard.Update(placeXAtTopLeft);

            // act
            var observation = env.Step(placeXAtTopLeft);

            // assert
            Assert.AreEqual(expectedBoard.AsString(), observation.Board.AsString());
        }

        [Test]
        public void PlacingTileOnNonEmptySquare_Throws()
        {
            var env = new TicTacToeEnvironment();
            env.SetState(Board.CreateFromString("x  " +
                                                "   " +
                                                "   "));

            var placeXAtTopLeft = new TicTacToeAction { Position = 0, Tile = BoardTile.X };

            Assert.Throws<InvalidOperationException>(() => env.Step(placeXAtTopLeft));
        }

        [Test]
        public void CreatingInvalidState_Throws()
        {
            var env = new TicTacToeEnvironment();
            env.SetState(Board.CreateFromString("x  " +
                                                "   " +
                                                "   "));

            var placeXAtTopLeft = new TicTacToeAction { Position = 1, Tile = BoardTile.X };

            Assert.Throws<InvalidOperationException>(() => env.Step(placeXAtTopLeft));
        }

        [Test]
        public void RewardIs1_ForWin()
        {
            var env = new TicTacToeEnvironment();
            env.SetState(Board.CreateFromString("xx " +
                                                "oo " +
                                                "   "));

            var placeXAtTopRight = new TicTacToeAction { Position = 2, Tile = BoardTile.X };

            // act
            var observation = env.Step(placeXAtTopRight);

            // assert
            Assert.AreEqual(1, observation.Reward);
        }

        [Test]
        public void RewardIsNegative1_ForLoss()
        {
            var env = new TicTacToeEnvironment();
            env.SetState(Board.CreateFromString("xx " +
                                                "oo " +
                                                "   "));

            var placeOAtMiddleRight = new TicTacToeAction { Position = 5, Tile = BoardTile.O };

            // act
            var observation = env.Step(placeOAtMiddleRight);

            // assert
            Assert.AreEqual(-1, observation.Reward);
        }

        [Test]
        public void RewardIs0_WhenGameIsNotOver()
        {
            var env = new TicTacToeEnvironment();
            env.Reset();

            var placeOAtMiddleRight = new TicTacToeAction { Position = 5, Tile = BoardTile.O };

            // act
            var observation = env.Step(placeOAtMiddleRight);

            // assert
            Assert.AreEqual(0, observation.Reward);
        }
    }
}
