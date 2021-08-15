using System.Collections.Generic;

namespace CliffWalking.Agent.DataStructures
{
    public class CliffWalkingEnvironmentModel
    {
        private readonly HashSet<Position>
            _observedStates = new();
        private readonly Dictionary<Position, HashSet<CliffWalkingAction>>
            _actionsTakenAt = new();
        private readonly Dictionary<(Position, CliffWalkingAction), (Position, double)>
            _map = new();

        public IEnumerable<Position>
            ObservedStates => _observedStates;
        public IEnumerable<CliffWalkingAction>
            ActionsTakenAt(Position state) => _actionsTakenAt[state];

        public void Update(Position state, CliffWalkingAction action, Position nextState, double reward)
        {
            _observedStates.Add(state);
            if (_actionsTakenAt.ContainsKey(state))
            {
                _actionsTakenAt[state].Add(action);
            }
            else
            {
                _actionsTakenAt[state] = new HashSet<CliffWalkingAction> {action};
            }
            _map[(state, action)] = (nextState, reward);
        }

        public Step DoAction(Position state, CliffWalkingAction action)
        {
            var (nextState, reward) = _map[(state, action)];
            return new Step(nextState, reward, false);
        }
    }
}