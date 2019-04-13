namespace pandemic.StateMachine
{
    public class PandemicStateMachine
    {
        public PandemicGameState State { get; }

        public PandemicStateMachine(PandemicGameState initialState)
        {
            State = initialState;
        }
        
        public void ProcessAction(InitGameAction initGameAction)
        {
            InitialInfectCities(State);
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
                        city.AddCube();
                    }

                    state.InfectionDiscardPile.Push(cityName);
                }
            }
        }
    }
}