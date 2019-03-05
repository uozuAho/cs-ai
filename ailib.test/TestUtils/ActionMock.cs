namespace ailib.test.TestUtils
{
    public class ActionMock<T>
    {
        public T Value { get; set; }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}