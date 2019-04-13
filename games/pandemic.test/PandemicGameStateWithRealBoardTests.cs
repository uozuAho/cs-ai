using System.Linq;
using NUnit.Framework;

namespace pandemic.test
{
    public class PandemicGameStateWithRealBoardTests
    {
        private PandemicGameState _state;

        [SetUp]
        public void Setup()
        {
            _state = new PandemicGameState(PandemicBoard.CreateRealGameBoard());
        }

        [Test]
        public void InitialState_ShouldHaveValidInfectionDeck()
        {
            // should have drawn 9 infection cards
            Assert.AreEqual(48 - 9, _state.InfectionDeck.Count);
            Assert.AreEqual(9, _state.InfectionDiscardPile.Count);
            Assert.AreEqual(2, _state.InfectionRate);
        }

        [Test]
        public void InitialState_ShouldHave9InfectedCities()
        {
            Assert.AreEqual(3, _state.CityStates.Count(c => c.NumCubes() == 3));
            Assert.AreEqual(3, _state.CityStates.Count(c => c.NumCubes() == 2));
            Assert.AreEqual(3, _state.CityStates.Count(c => c.NumCubes() == 1));
        }
    }
}