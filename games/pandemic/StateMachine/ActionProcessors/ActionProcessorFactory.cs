using pandemic.StateMachine.Actions;

namespace pandemic.StateMachine.ActionProcessors
{
    internal class ActionProcessorFactory : IActionProcessorFactory
    {
        public IActionProcessor ProcessorFor(IAction action)
        {
            return new InitGameProcessor();
        }
    }
}