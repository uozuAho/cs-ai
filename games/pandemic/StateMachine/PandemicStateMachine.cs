using pandemic.StateMachine.ActionProcessors;
using pandemic.StateMachine.Actions;

namespace pandemic.StateMachine
{
    public class PandemicStateMachine
    {
        public PandemicGameState State { get; }
        
        private readonly IActionProcessorFactory _processorFactory;

        public PandemicStateMachine(PandemicGameState initialState, IActionProcessorFactory processorFactory)
        {
            State = initialState;
            _processorFactory = processorFactory;
        }

        public void ProcessAction(IAction action)
        {
            var processor = _processorFactory.ProcessorFor(action);
            processor.ProcessAction(State, action);
        }
    }
}