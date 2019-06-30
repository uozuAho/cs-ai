using System.Collections.Generic;
using System.Linq;
using pandemic.GameObjects;

namespace pandemic.States
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
        public Cubes CubePile { get; set; }
        public List<Player> Players { get; set; }

        private readonly PandemicBoard _board;
        private readonly Dictionary<string, CityState> _cityNameLookup;

        public PandemicGameState(PandemicBoard board) : this(board, new List<Player>()) { }

        public PandemicGameState(PandemicBoard board, IEnumerable<Player> players)
        {
            _board = board;
            InfectionRate = 2;
            // todo: shuffle
            InfectionDeck = new Stack<string>(_board.Cities.Select(c => c.Name));
            InfectionDiscardPile = new Stack<string>();
            CityStates = _board.Cities.Select(c => new CityState(c)).ToList();
            _cityNameLookup = BuildCityNameLookup(CityStates);
            CubePile = CreateNewCubePile();
            InitPlayers(players);
        }

        private void InitPlayers(IEnumerable<Player> players)
        {
            Players = players.ToList();
            var atlanta = GetCity("Atlanta");
            Players.ForEach(p => p.Location = atlanta);
        }

        public CityState GetCity(string name)
        {
            return _cityNameLookup[name];
        }

        private static Dictionary<string, CityState> BuildCityNameLookup(IEnumerable<CityState> cityStates)
        {
            return cityStates.ToDictionary(c => c.Name, c => c);
        }

        private static Cubes CreateNewCubePile()
        {
            var cubes = new Cubes();
            var allColours = new[] {Colour.Red, Colour.Blue, Colour.Black, Colour.Yellow};
            
            foreach (var colour in allColours)
            {
                for (var i = 0; i < 24; i++)
                {
                    cubes.AddCube(colour);
                }
            }

            return cubes;
        }
    }
}
