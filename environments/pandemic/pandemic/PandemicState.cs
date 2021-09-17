using System.Collections.Generic;
using System.Linq;

namespace pandemic
{
    public record PandemicState
    {
        public int InfectionRate { get; }
        public int OutbreakCounter { get; set; }
        public Stack<string> InfectionDeck { get; }
        public Stack<string> InfectionDiscardPile { get; }
        public List<CityState> CityStates { get; }
        public Cubes CubePile { get; set; }
        public PlayerState[] Players { get; set; }
        public Stack<PlayerCard> PlayerDeck { get; set; }

        private readonly PandemicBoard _board;
        private readonly Dictionary<string, CityState> _cityNameLookup;
        
        public PandemicState(PandemicBoard board, int numEpidemicCards, Role[] characters)
        {
            _board = board;
            InfectionRate = 2;
            // todo: shuffle
            InfectionDeck = new Stack<string>(_board.Cities.Select(c => c.Name));
            InfectionDiscardPile = new Stack<string>();
            CityStates = _board.Cities.Select(c => new CityState(c)).ToList();
            _cityNameLookup = BuildCityNameLookup(CityStates);
            CubePile = CreateNewCubePile();
            Players = characters.Select(c => new PlayerState()).ToArray();
            var playerDeck = new List<PlayerCard>();
            playerDeck.AddRange(_board.Cities.Select(c => new PlayerCityCard(c.Name)));
            playerDeck.AddRange(Enumerable.Range(0, numEpidemicCards).Select(_ => new EpidemicCard()));
            PlayerDeck = new Stack<PlayerCard>(playerDeck);
        }

        public static PandemicState Init(PandemicBoard board, int numEpidemicCards, params Role[] roles)
        {
            var state = new PandemicState(board, numEpidemicCards, roles);
            InitialInfectCities(state);
            DrawPlayerCards(state);
            return state;
        }

        private static void InitialInfectCities(PandemicState state)
        {
            for (var numCubes = 1; numCubes <= 3; numCubes++)
            {
                for (var i = 0; i < 3; i++)
                {
                    var cityName = state.InfectionDeck.Pop();
                    var city = state.GetCity(cityName);
                    for (var j = 0; j < numCubes; j++)
                    {
                        state.CubePile.RemoveCube(city.Colour);
                        city.AddCube(city.Colour);
                    }

                    state.InfectionDiscardPile.Push(cityName);
                }
            }
        }

        private static void DrawPlayerCards(PandemicState state)
        {
            foreach (var player in state.Players)
            {
                player.Hand.Add(state.PlayerDeck.Pop());
                player.Hand.Add(state.PlayerDeck.Pop());
                player.Hand.Add(state.PlayerDeck.Pop());
                player.Hand.Add(state.PlayerDeck.Pop());
            }
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

        public PandemicState Apply(DriveFerry driveFerry)
        {
            return this;
        }
    }
}
