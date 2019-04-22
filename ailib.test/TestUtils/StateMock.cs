namespace ailib.test.TestUtils
{
    public class StateMock
    {
        public string Value { get; set; }

        public StateMock(string value)
        {
            Value = value;
        }
        
        public override string ToString()
        {
            return Value;
        }
    }
}