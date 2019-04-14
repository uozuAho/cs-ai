namespace pandemic
{
    // todo: extend City?
    public class CityState
    {
        public City City { get; }
        
        // todo: make cubes public
        private readonly Cubes _cubes;

        public CityState(City city)
        {
            City = city;
            _cubes = new Cubes();
        }

        private CityState(City city, Cubes cubes)
        {
            City = city;
            _cubes = cubes;
        }

        public CityState Clone()
        {
            return new CityState(City, _cubes.Clone());
        }

        public int NumCubes()
        {
            return _cubes.NumCubes(City.Colour);
        }
        
        public int NumCubes(Colour colour)
        {
            return _cubes.NumCubes(colour);
        }

        public void AddCube()
        {
            _cubes.AddCube(City.Colour);
        }

        public void AddCube(Colour colour)
        {
            _cubes.AddCube(colour);
        }

        public void RemoveCube()
        {
            _cubes.RemoveCube(City.Colour);
        }

        public void RemoveCube(Colour colour)
        {
            _cubes.RemoveCube(colour);
        }
    }
}