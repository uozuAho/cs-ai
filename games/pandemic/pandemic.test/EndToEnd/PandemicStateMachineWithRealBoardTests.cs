using NUnit.Framework;
using pandemic.GameObjects;
using pandemic.StateMachine;
using pandemic.StateMachine.ActionProcessors;
using pandemic.StateMachine.Actions;
using pandemic.States;

namespace pandemic.test.EndToEnd
{
    internal class PandemicStateMachineWithRealBoardTests
    {
        private PandemicStateMachine _stateMachine;

        [SetUp]
        public void Setup()
        {
            var initialState = new PandemicGameState(PandemicBoard.CreateRealGameBoard());
            var actionProcessorFactory = new ActionProcessorFactory();

            _stateMachine = new PandemicStateMachine(initialState, actionProcessorFactory);
            _stateMachine.ProcessAction(new InitGameAction());
        }

        [Test]
        public void GivenStartOfGame_MoveToMiami_ShouldMoveFirstPlayerToMiami()
        {
            _stateMachine.ProcessAction(new MoveAction("Miami"));

            Assert.AreEqual("Miami", _stateMachine.State.Players[0].Location.Name);
        }
    }

    internal class MoveAction : IAction
    {
        public string CityName { get; }

        public MoveAction(string cityName)
        {
            CityName = cityName;
        }
    }
}
