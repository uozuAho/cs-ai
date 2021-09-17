using System.Linq;
using NUnit.Framework;

namespace pandemic.test
{
    public class New_2_player_game
    {
        private PandemicState _state;

        [SetUp]
        public void Setup()
        {
            _state = PandemicState.Init(
                PandemicBoard.CreateRealGameBoard(),
                Role.Medic,
                Role.Scientist);
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

        [Test]
        public void Players_have_4_city_cards()
        {
            Assert.AreEqual(4, _state.Players[0].Hand.Count);
        }
    }

    public class NewGame_then_player_moves_4_times
    {
        private PandemicState _state;

        [SetUp]
        public void Setup()
        {
            _state = PandemicState.Init(PandemicBoard.CreateRealGameBoard(), Role.Medic);

            _state = _state.Apply(new DriveFerry("Chicago"));
            _state = _state.Apply(new DriveFerry("Atlanta"));
            _state = _state.Apply(new DriveFerry("Chicago"));
            _state = _state.Apply(new DriveFerry("Atlanta"));
        }

        [Test]
        public void Player_picks_up_2_city_cards()
        {
            // _state.Players[0]
        }
    }
}