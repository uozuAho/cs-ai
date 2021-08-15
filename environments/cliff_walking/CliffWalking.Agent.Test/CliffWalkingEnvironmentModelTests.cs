using System.Collections.Generic;
using System.Linq;
using CliffWalking.Agent.DataStructures;
using NUnit.Framework;

namespace CliffWalking.Agent.Test
{
    public class CliffWalkingEnvironmentModel_WhenEmpty
    {
        [Test]
        public void DoAction_Throws()
        {
            var model = new CliffWalkingEnvironmentModel();

            Assert.Throws<KeyNotFoundException>(() => model.DoAction(new Position(2, 3), CliffWalkingAction.Down));
        }
    }

    public class CliffWalkingEnvironmentModelTests_WithOneObservedAction
    {
        private readonly CliffWalkingEnvironmentModel _model = new();
        private readonly Position _currentState = new (1, 2);
        private const CliffWalkingAction Action = CliffWalkingAction.Left;
        private readonly Position _nextState = new (0, 2);
        private const double Reward = 0.33;

        [SetUp]
        public void Setup()
        {
            _model.Update(_currentState, Action, _nextState, Reward);
        }

        [Test]
        public void Has_one_observed_state()
        {
            Assert.AreEqual(1, _model.ObservedStates.Count());
        }

        [Test]
        public void Stores_action_at_current_state()
        {
            Assert.AreEqual(1, _model.ActionsTakenAt(_currentState).Count());
        }

        [Test]
        public void Actions_taken_at_next_state_throws()
        {
            Assert.Throws<KeyNotFoundException>(() => _model.ActionsTakenAt(_nextState));
        }

        [Test]
        public void DoAction_replays_observed_action()
        {
            var (observation, reward, isDone) = _model.DoAction(_currentState, Action);

            Assert.AreEqual(false, isDone);
            Assert.AreEqual(_nextState, observation);
            Assert.AreEqual(Reward, reward);
        }
    }
}
