using System;
using System.Collections.Generic;
using TicTacToe.Agent;
using TicTacToe.Agent.Agents.MonteCarlo;
using TicTacToe.Game;

namespace TicTacToe.Console
{
    public class LearningAgentRegister
    {
        private readonly Dictionary<string, AgentWithDescription> _agents = new();

        private record AgentWithDescription(
            string Name,
            string Description,
            Func<BoardTile, ITicTacToeAgent> AgentCreator);

        public LearningAgentRegister()
        {
            // AddAgent(nameof(RlBookPTableAgent), RlBookPTableAgent.CreateDefaultAgent);
            // AddAgent(nameof(RlBookModifiedPTableAgent), RlBookModifiedPTableAgent.CreateDefaultAgent);
            AddAgent("mc", "First-visit Monte Carlo agent with exploring starts",
                tile => new MonteCarloTicTacToeAgent(tile));
        }

        public IEnumerable<string> AvailableAgents()
        {
            return _agents.Keys;
        }

        public ITicTacToeAgent GetAgentByName(string name, BoardTile tile)
        {
            return _agents[name].AgentCreator(tile);
        }

        public void AddAgent(
            string name, string description, Func<BoardTile, ITicTacToeAgent> agentCreator)
        {
            _agents[name] = new AgentWithDescription(name, description, agentCreator);
        }
    }
}
