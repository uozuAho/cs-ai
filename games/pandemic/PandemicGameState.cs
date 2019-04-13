using System.Collections.Generic;
using System.Linq;

namespace pandemic
{
    public class PandemicGameState
    {
        /** infection cards per turn */
        public int InfectionRate { get; }
        
        /** city name[] */
        public Stack<string> InfectionDeck { get; }
        
        /** city name[] */
        public Stack<string> InfectionDiscardPile { get; }

//        public win_condition?: WinCondition;
//        public lose_condition?: LoseCondition;
//        public unused_cubes: Cubes;
        public int OutbreakCounter { get; set; }

        /** map of city name : city state */
//        private _city_states: Map<string, CityState>;

        private PandemicBoard _board;
//        private _graph: GraphT<CityState>;

        public PandemicGameState(PandemicBoard board)
        {
            _board = board;
            InfectionRate = 2;
            // todo: shuffle
            InfectionDeck = new Stack<string>(_board.Cities.Select(c => c.Name));
            InfectionDiscardPile = new Stack<string>();
            
            InitNewGameState();
        }

        private void InitNewGameState()
        {
            for (var i = 0; i < 9; i++)
            {
                InfectionDiscardPile.Push(InfectionDeck.Pop());
            }
        }
    }
}
