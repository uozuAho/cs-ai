using pandemic.StateMachine.Actions;

namespace pandemic.StateMachine.ActionProcessors
{
    public class InitGameProcessor : IActionProcessor
    {
        public void ProcessAction(PandemicGameState state, IAction action)
        {
            InitialInfectCities(state);
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