using NUnit.Framework;

namespace pandemic.test
{
    public class PandemicGameStateTests
    {
        private PandemicGameState _state;

        [SetUp]
        public void Setup()
        {
            _state = new PandemicGameState(new PandemicBoard());
        }

        [Test]
        public void AfterInit_ShouldHaveValidInfectionDeck()
        {
            // should have drawn 9 infection cards
            Assert.AreEqual(48 - 9, _state.InfectionDeck.Count);
            Assert.AreEqual(9, _state.InfectionDiscardPile.Count);
            Assert.AreEqual(2, _state.InfectionRate);
        }
    }
}