using System.Linq;
using NUnit.Framework;
using pandemic.GameObjects;
using pandemic.StateMachine.ActionProcessors;
using pandemic.StateMachine.Actions;
using pandemic.States;

namespace pandemic.test
{
    public class InitGameProcessorWithRealBoardTests
    {
        private InitGameProcessor _processor;
        private PandemicGameState _state;

        [SetUp]
        public void Setup()
        {
            _processor = new InitGameProcessor();
            _state = new PandemicGameState(PandemicBoard.CreateRealGameBoard());
        }

        [Test]
        public void ProcessInit_ShouldSetCountersCorrectly()
        {
            _processor.ProcessAction(_state, new InitGameAction(new[] { Character.Medic }));

            Assert.AreEqual(2, _state.InfectionRate);
            Assert.AreEqual(0, _state.OutbreakCounter);
        }

        [Test]
        public void ProcessInit_ShouldDraw9InfectionCards()
        {
            _processor.ProcessAction(_state, new InitGameAction(new[] { Character.Medic }));

            Assert.AreEqual(48 - 9, _state.InfectionDeck.Count);
            Assert.AreEqual(9, _state.InfectionDiscardPile.Count);
        }

        [Test]
        public void ProcessInit_ShouldInfectCities()
        {
            _processor.ProcessAction(_state, new InitGameAction(new[] { Character.Medic }));

            Assert.AreEqual(3, _state.CityStates.Count(c => c.NumCubes(c.Colour) == 3));
            Assert.AreEqual(3, _state.CityStates.Count(c => c.NumCubes(c.Colour) == 2));
            Assert.AreEqual(3, _state.CityStates.Count(c => c.NumCubes(c.Colour) == 1));
        }

        [Test]
        public void ProcessInit_ShouldRemoveCubesFromPiles()
        {
            _processor.ProcessAction(_state, new InitGameAction(new[] { Character.Medic }));

            var sumOfAllCubesInPile = ColourExtensions.AllColours()
                .Select(colour => _state.CubePile.NumCubes(colour))
                .Sum();

            Assert.AreEqual(4 * 24 - 9 - 6 - 3, sumOfAllCubesInPile);
        }

        [Test]
        public void ProcessInitWithMedic_ShouldCreateOneMedicPlayer()
        {
            _processor.ProcessAction(_state, new InitGameAction(new[] { Character.Medic }));

            Assert.AreEqual(1, _state.Players.Count);
            var player = _state.Players.Single();
            Assert.AreEqual(Character.Medic, player.Character);
        }

        [Test]
        public void ProcessInit_ShouldPutAllPlayersInAtlanta()
        {
            _processor.ProcessAction(_state, new InitGameAction(new[] { Character.Medic }));

            var atlanta = _state.GetCity("Atlanta");
            Assert.IsTrue(_state.Players.All(p => p.Location == atlanta));
        }
    }
}
