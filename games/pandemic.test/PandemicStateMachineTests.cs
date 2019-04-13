using System.Linq;
using NUnit.Framework;
using pandemic.StateMachine;

namespace pandemic.test
{
    public class PandemicStateMachineTests
    {
        private PandemicStateMachine _machine;
        
        [SetUp]
        public void Setup()
        {
            var board = PandemicBoard.CreateRealGameBoard();
            var initialState = new PandemicGameState(board);
            
            _machine = new PandemicStateMachine(initialState);
        }

        [Test]
        public void GivenNoActions_BoardShouldBeReadyToInit()
        {
            var state = _machine.State;
            
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
        
        [Test]
        public void AfterInit_ShouldHaveValidInfectionDeck()
        {
            _machine.ProcessAction(new InitGameAction());
            
            var state = _machine.State;
            
            // should have drawn 9 infection cards
            Assert.AreEqual(48 - 9, state.InfectionDeck.Count);
            Assert.AreEqual(9, state.InfectionDiscardPile.Count);
            Assert.AreEqual(2, state.InfectionRate);
        }

        [Test]
        public void AfterInit_ShouldHave9InfectedCities()
        {
            _machine.ProcessAction(new InitGameAction());
            
            var state = _machine.State;
            
            Assert.AreEqual(3, state.CityStates.Count(c => c.NumCubes() == 3));
            Assert.AreEqual(3, state.CityStates.Count(c => c.NumCubes() == 2));
            Assert.AreEqual(3, state.CityStates.Count(c => c.NumCubes() == 1));
        }
    }
}