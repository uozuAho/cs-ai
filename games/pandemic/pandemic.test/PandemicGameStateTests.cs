using System.Linq;
using NUnit.Framework;

namespace pandemic.test
{
    public class PandemicGameStateTests
    {
        [Test]
        public void GivenRealBoard_StateShouldBeReadyToInit()
        {
            var state = new PandemicGameState(PandemicBoard.CreateRealGameBoard());
            
            // no cubes on any cities
            Assert.AreEqual(0, state.CityStates.Sum(c => c.NumCubes(Colour.Red)));
            Assert.AreEqual(0, state.CityStates.Sum(c => c.NumCubes(Colour.Blue)));
            Assert.AreEqual(0, state.CityStates.Sum(c => c.NumCubes(Colour.Black)));
            Assert.AreEqual(0, state.CityStates.Sum(c => c.NumCubes(Colour.Yellow)));
            
            Assert.AreEqual(48, state.InfectionDeck.Count);
            Assert.AreEqual(0, state.InfectionDiscardPile.Count);
            
            Assert.AreEqual(24, state.CubePile.NumCubes(Colour.Red));
            Assert.AreEqual(24, state.CubePile.NumCubes(Colour.Blue));
            Assert.AreEqual(24, state.CubePile.NumCubes(Colour.Black));
            Assert.AreEqual(24, state.CubePile.NumCubes(Colour.Yellow));
        }
    }
}