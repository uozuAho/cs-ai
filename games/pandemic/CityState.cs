namespace pandemic
{
    public class CityState : City
    {
        // todo: make cubes public
        private readonly Cubes _cubes;

        public CityState(City city)
        {
            Name = city.Name;
            Location = city.Location;
            Colour = city.Colour;
            
            _cubes = new Cubes();
        }

        private CityState(City city, Cubes cubes) : this(city)
        {
            _cubes = cubes;
        }

        public CityState Clone()
        {
            return new CityState(this, _cubes.Clone());
        }

        public int NumCubes()
        {
            return _cubes.NumCubes(Colour);
        }
        
        public int NumCubes(Colour colour)
        {
            return _cubes.NumCubes(colour);
        }

        public void AddCube()
        {
            _cubes.AddCube(Colour);
        }

        public void AddCube(Colour colour)
        {
            _cubes.AddCube(colour);
        }

        public void RemoveCube()
        {
            _cubes.RemoveCube(Colour);
        }

        public void RemoveCube(Colour colour)
        {
            _cubes.RemoveCube(colour);
        }
    }
}