using NUnit.Framework;

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
    }
}