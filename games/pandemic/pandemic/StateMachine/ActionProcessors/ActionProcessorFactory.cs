using pandemic.StateMachine.Actions;

namespace pandemic.StateMachine.ActionProcessors
{
    public class ActionProcessorFactory : IActionProcessorFactory
    {
        public IActionProcessor ProcessorFor(IAction action)
        {
            return new InitGameProcessor();
        }
    }
}