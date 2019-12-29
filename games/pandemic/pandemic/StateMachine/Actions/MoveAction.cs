namespace pandemic.StateMachine.Actions
{
    public class MoveAction : IAction
    {
        public string CityName { get; }

        public MoveAction(string cityName)
        {
            CityName = cityName;
        }
    }
}