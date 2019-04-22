namespace ailib.test.TestUtils
{
    public class ActionMock
    {
        private readonly string _value;

        public ActionMock(string value)
        {
            _value = value;
        }
        
        public override string ToString()
        {
            return _value;
        }
    }
}