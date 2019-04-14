using NUnit.Framework;
using pandemic.GameObjects;

namespace pandemic.test
{
    public class PandemicGameTests
    {
        private PandemicGame _game;

        [SetUp]
        public void Setup()
        {
            _game = new PandemicGame();
        }
        
        [Test]
        public void DoMove_ShouldntBreak()
        {
            _game.DoMove(new PlayerMove());
        }

        [Test]
        public void State_ShouldReturnState()
        {
            var state = _game.State;
            
            Assert.IsNotNull(state);
        }
    }
}