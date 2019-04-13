﻿using System.Collections.Generic;
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

        public int OutbreakCounter { get; set; }

        public List<CityState> CityStates { get; }

        private readonly PandemicBoard _board;
        private readonly Dictionary<string, CityState> _cityNameLookup;
        
        public PandemicGameState(PandemicBoard board)
        {
            _board = board;
            InfectionRate = 2;
            // todo: shuffle
            InfectionDeck = new Stack<string>(_board.Cities.Select(c => c.Name));
            InfectionDiscardPile = new Stack<string>();
            CityStates = _board.Cities.Select(c => new CityState(c)).ToList();
            _cityNameLookup = BuildCityNameLookup(CityStates);
        }

        public CityState GetCity(string name)
        {
            return _cityNameLookup[name];
        }

        private static Dictionary<string, CityState> BuildCityNameLookup(IEnumerable<CityState> cityStates)
        {
            return cityStates.ToDictionary(c => c.City.Name, c => c);
        }
    }
}
