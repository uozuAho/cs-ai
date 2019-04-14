using System;
using System.Collections.Generic;
using System.Linq;

namespace pandemic
{
    public enum Colour
    {
        Red,
        Black,
        Blue,
        Yellow
    }

    public static class ColourExtensions
    {
        public static IEnumerable<Colour> AllColours()
        {
            return Enum.GetValues(typeof(Colour)).Cast<Colour>();
        }
    }
}