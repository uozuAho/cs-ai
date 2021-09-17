using System.Linq;
using NUnit.Framework;
using pandemic.GameObjects;
using pandemic.States;

namespace pandemic.test
{
    public class NewGame
    {
        private PandemicGameState _state;

        [SetUp]
        public void Setup()
        {
            _state = PandemicGame.Init(PandemicBoard.CreateRealGameBoard());
        }

        [Test]
        public void Infection_rate_is_2()
        {
            Assert.AreEqual(2, _state.InfectionRate);
        }

        [Test]
        public void Outbreak_counter_is_2()
        {
            Assert.AreEqual(0, _state.OutbreakCounter);

        }

        [Test]
        public void Infection_deck_has_9_cards_drawn()
        {
            Assert.AreEqual(48 - 9, _state.InfectionDeck.Count);
            Assert.AreEqual(9, _state.InfectionDiscardPile.Count);
        }

        [Test]
        public void Nine_cities_have_cubes()
        {
            Assert.AreEqual(3, _state.CityStates.Count(c => c.NumCubes(c.Colour) == 3));
            Assert.AreEqual(3, _state.CityStates.Count(c => c.NumCubes(c.Colour) == 2));
            Assert.AreEqual(3, _state.CityStates.Count(c => c.NumCubes(c.Colour) == 1));
        }

        [Test]
        public void Cubes_have_been_removed_from_pile()
        {
            var sumOfAllCubesInPile = ColourExtensions.AllColours()
                .Select(colour => _state.CubePile.NumCubes(colour))
                .Sum();

            Assert.AreEqual(4 * 24 - 9 - 6 - 3, sumOfAllCubesInPile);
        }
    }
}