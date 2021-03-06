using pandemic.GameObjects;

namespace pandemic.States
{
    public class CityState : City
    {
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

        public int NumCubes(Colour colour)
        {
            return _cubes.NumCubes(colour);
        }

        public void AddCube(Colour colour)
        {
            _cubes.AddCube(colour);
        }
    }
}