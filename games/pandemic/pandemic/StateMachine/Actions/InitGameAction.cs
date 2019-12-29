using System.Collections.Generic;
using pandemic.GameObjects;

namespace pandemic.StateMachine.Actions
{
    public class InitGameAction : IAction
    {
        public ICollection<Character> PlayerCharacters { get; }

        public InitGameAction(ICollection<Character> playerCharacters)
        {
            PlayerCharacters = playerCharacters;
        }
    }
}