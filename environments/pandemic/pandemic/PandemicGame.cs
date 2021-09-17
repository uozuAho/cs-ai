using pandemic.GameObjects;
using pandemic.States;

namespace pandemic
{
    public class PandemicGame
    {
        public bool IsFinished => false;

        public void DoMove(PlayerMove move)
        {
        }

        public static PandemicGameState Init(PandemicBoard board, params Role[] roles)
        {
            var state = new PandemicGameState(board, roles);
            InitialInfectCities(state);
            return state;
        }

        private static void InitialInfectCities(PandemicGameState state)
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
    }
}