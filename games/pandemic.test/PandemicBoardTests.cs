using System.Linq;
using NUnit.Framework;

namespace pandemic.test
{
    public class PandemicBoardTests
    {
        [Test]
        public void RealBoard_ShouldHave48Cities()
        {
            var board = PandemicBoard.CreateRealGameBoard();

            Assert.AreEqual(48, board.Cities.Count);
        }

        [Test]
        public void RealBoard_AdjacentToAtlanta()
        {
            var board = PandemicBoard.CreateRealGameBoard();
            var city = board.GetCity("Atlanta");

            var adjacent = board.Adjacent(city);
            
            Assert.AreEqual(3, adjacent.Count);
            Assert.AreEqual(1, adjacent.Count(c => c.Name == "Chicago"));
            Assert.AreEqual(1, adjacent.Count(c => c.Name == "Washington"));
            Assert.AreEqual(1, adjacent.Count(c => c.Name == "Miami"));
        }
    }
}