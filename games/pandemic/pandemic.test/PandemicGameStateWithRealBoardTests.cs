using System.Linq;
using NUnit.Framework;
using pandemic.GameObjects;
using pandemic.States;

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
    }
}