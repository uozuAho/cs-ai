using System.Linq;
using NUnit.Framework;
using pandemic.GameObjects;
using pandemic.States;

namespace pandemic.test
{
    public class PandemicTests
    {
        [Test]
        public void GivenRealBoard_StateShouldBeReadyToInit()
        {
            var state = new PandemicGameState(PandemicBoard.CreateRealGameBoard());
            
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

        [Test]
        public void asdf()
        {
            var state = PandemicGame.Init(PandemicBoard.CreateRealGameBoard());

            Assert.AreEqual(2, state.InfectionRate);
            Assert.AreEqual(0, state.OutbreakCounter);

            // should have drawn 9 infection cards
            Assert.AreEqual(48 - 9, state.InfectionDeck.Count);
            Assert.AreEqual(9, state.InfectionDiscardPile.Count);

            // 9 cities should have cubes on them
            Assert.AreEqual(3, state.CityStates.Count(c => c.NumCubes(c.Colour) == 3));
            Assert.AreEqual(3, state.CityStates.Count(c => c.NumCubes(c.Colour) == 2));
            Assert.AreEqual(3, state.CityStates.Count(c => c.NumCubes(c.Colour) == 1));

            // cubes should have been removed from pile
            var sumOfAllCubesInPile = ColourExtensions.AllColours()
                .Select(colour => state.CubePile.NumCubes(colour))
                .Sum();

            Assert.AreEqual(4 * 24 - 9 - 6 - 3, sumOfAllCubesInPile);
        }
    }
}