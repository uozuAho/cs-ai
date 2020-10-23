using System.Collections.Generic;
using System.Linq;

namespace Blackjack
{
    public class BlackjackProblem
    {
        private readonly BlackjackAction[] _allActions = { BlackjackAction.Hit, BlackjackAction.Stay};

        public IEnumerable<BlackjackAction> AvailableActions(BlackjackState state)
        {
            return IsGameOver(state) ? Enumerable.Empty<BlackjackAction>() : _allActions;
        }

        public bool IsGameOver(BlackjackState state)
        {
            if (IsPlayerBust(state)) return true;
            if (Sum(state.PlayerCards) == 21) return true;
            if (state.IsPlayerStaying) return true;
            return false;
        }

        public bool IsPlayerBust(BlackjackState state)
        {
            return Sum(state.PlayerCards) > 21;
        }

        public int Sum(int[] cards)
        {
            var sum = cards.Sum();

            if (!cards.Contains(1)) return sum;

            if (sum < 12)
                return sum + 10;
            return sum;
        }
    }
}
