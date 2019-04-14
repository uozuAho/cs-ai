using System.Linq;
using NUnit.Framework;
using pandemic.GameObjects;
using pandemic.StateMachine;
using pandemic.StateMachine.ActionProcessors;
using pandemic.StateMachine.Actions;
using pandemic.States;

namespace pandemic.test
{
    public class InitGameProcessorTests
    {
        private InitGameProcessor _processor;

        [SetUp]
        public void Setup()
        {
            _processor = new InitGameProcessor();
        }
        
        [Test]
        public void GivenRealBoard_ProcessInit_ShouldInitialiseGameCorrectly()
        {
            var state = new PandemicGameState(PandemicBoard.CreateRealGameBoard());
            
            _processor.ProcessAction(state, new InitGameAction());
            
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