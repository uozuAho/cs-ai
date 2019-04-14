using pandemic.StateMachine.Actions;

namespace pandemic.StateMachine.ActionProcessors
{
    public interface IActionProcessor
    {
        void ProcessAction(PandemicGameState state, IAction action);
    }
}