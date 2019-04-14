using pandemic.StateMachine.Actions;
using pandemic.States;

namespace pandemic.StateMachine.ActionProcessors
{
    public interface IActionProcessor
    {
        void ProcessAction(PandemicGameState state, IAction action);
    }
}