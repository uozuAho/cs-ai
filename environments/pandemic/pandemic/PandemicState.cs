using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;

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
        public PlayerState CurrentPlayer => Players[_currentPlayerIdx];
        public Stack<PlayerCard> PlayerDeck { get; set; }

        private readonly Dictionary<string, CityState> _cityNameLookup;
        private readonly PandemicBoard _board;
        private int _currentPlayerIdx = 0;

        public static PandemicState Init(PandemicBoard board, int numEpidemicCards, params Role[] roles)
        {
            var state = new PandemicState(board, numEpidemicCards, roles);
            InitialInfectCities(state);
            DrawPlayerCards(state);
            return state;
        }

        public PandemicState Apply(DriveFerry driveFerry)
        {
            var currentCity = _board.GetCity(CurrentPlayer.Position);
            var destination = _board.GetCity(driveFerry.City);

            if (!_board.Adjacent(currentCity).Contains(destination))
                throw new InvalidOperationException($"Invalid drive/ferry: {currentCity.Name} to {destination.Name}");

            return this;
        }

        private PandemicState(PandemicBoard board, int numEpidemicCards, Role[] roles)
        {
            Debug.Assert(numEpidemicCards >= 4 && numEpidemicCards <= 6);
            Debug.Assert(roles.Length >= 2);

            _board = board;
            InfectionRate = 2;
            // todo: shuffle
            InfectionDeck = new Stack<string>(board.Cities.Select(c => c.Name));
            InfectionDiscardPile = new Stack<string>();
            CityStates = board.Cities.Select(c => new CityState(c)).ToList();
            _cityNameLookup = BuildCityNameLookup(CityStates);
            CubePile = CreateNewCubePile();
            Players = roles.Select(c => new PlayerState("Atlanta")).ToArray();
            var playerDeck = new List<PlayerCard>();
            playerDeck.AddRange(board.Cities.Select(c => new PlayerCityCard(c.Name)));
            playerDeck.AddRange(Enumerable.Range(0, numEpidemicCards).Select(_ => new EpidemicCard()));
            PlayerDeck = new Stack<PlayerCard>(playerDeck);
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

        private CityState GetCity(string name)
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
