using System;
using System.Collections.Generic;
using System.Linq;

namespace pandemic.GameObjects
{
    public class Cubes
    {
        private readonly Dictionary<Colour, int> _counts = new Dictionary<Colour, int>
        {
            {Colour.Red, 0},
            {Colour.Yellow, 0},
            {Colour.Black, 0},
            {Colour.Blue, 0}
        };

        public Cubes()
        {
        }

        private Cubes(Dictionary<Colour, int> counts)
        {
            _counts = counts.ToDictionary(c => c.Key, c => c.Value);
        }
        
        public Cubes Clone()
        {
            return new Cubes(_counts);
        }

        public int NumCubes(Colour colour)
        {
            return _counts[colour];
        }

        public void AddCube(Colour colour)
        {
            _counts[colour]++;
        }

        public void RemoveCube(Colour colour)
        {
            if (_counts[colour] == 0) throw new InvalidOperationException("no cubes to remove");
            _counts[colour]--;
        }
    }
}