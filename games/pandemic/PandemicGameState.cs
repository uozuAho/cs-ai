using System.Collections.Generic;

namespace pandemic
{
    public class PandemicGameState
    {
        /** infection cards per turn */
        public int InfectionRate { get; set; }
        
        /** city name[] */
        public List<string> InfectionDeck { get; set; }
        
        /** city name[] */
        public List<string> InfectionDiscardPile { get; set; }

//        public win_condition?: WinCondition;
//        public lose_condition?: LoseCondition;
//        public unused_cubes: Cubes;
        public int OutbreakCounter { get; set; }

        /** map of city name : city state */
//        private _city_states: Map<string, CityState>;

        private PandemicBoard _board;
//        private _graph: GraphT<CityState>;

        public PandemicGameState(PandemicBoard board) {
            _board = board;
        }
    }
}
