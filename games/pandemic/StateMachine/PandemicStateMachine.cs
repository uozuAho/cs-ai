namespace pandemic.StateMachine
{
    public class PandemicStateMachine
    {
        public PandemicGameState State { get; }
        
        private readonly IActionProcessorFactory _processorFactory;

        public PandemicStateMachine(PandemicGameState initialState, IActionProcessorFactory processorFactory)
        {
            State = initialState;
            _processorFactory = processorFactory;
        }

        public void ProcessAction(IAction action)
        {
            var processor = _processorFactory.ProcessorFor(action);
            processor.ProcessAction(State, action);
        }
    }

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
                        state.CubePile.RemoveCube(city.City.Colour);
                        city.AddCube(city.City.Colour);
                    }

                    state.InfectionDiscardPile.Push(cityName);
                }
            }
        }
    }
    
    public interface IAction
    {
    }

    internal class ActionProcessorFactory : IActionProcessorFactory
    {
        public IActionProcessor ProcessorFor(IAction action)
        {
            return new InitGameProcessor();
        }
    }

    public interface IActionProcessorFactory
    {
        IActionProcessor ProcessorFor(IAction action);
    }

    public interface IActionProcessor
    {
        void ProcessAction(PandemicGameState state, IAction action);
    }
}