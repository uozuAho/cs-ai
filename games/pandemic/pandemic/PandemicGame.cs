using pandemic.StateMachine;
using pandemic.StateMachine.ActionProcessors;

namespace pandemic
{
    public class PandemicGame
    {
        public bool IsFinished => false;
        public PandemicGameState State => _stateMachine.State;
        
        private readonly PandemicStateMachine _stateMachine;

        public PandemicGame()
        {
            var initialState = new PandemicGameState(PandemicBoard.CreateRealGameBoard());
            var actionProcessorFactory = new ActionProcessorFactory();
        
            _stateMachine = new PandemicStateMachine(initialState, actionProcessorFactory);
        }

        public void DoMove(PlayerMove move)
        {
        }
    }
}