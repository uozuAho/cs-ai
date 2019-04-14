using pandemic.StateMachine.Actions;

namespace pandemic.StateMachine.ActionProcessors
{
    public interface IActionProcessorFactory
    {
        IActionProcessor ProcessorFor(IAction action);
    }
}