using System;
using System.Collections.Generic;
using System.Linq;

namespace vacuum_world
{
    public class VacuumWorldState
    {
        public int WorldSize => _squares.Count;

        private Point2D _vacuumPos;
        public Point2D VacuumPos
        {
            get => _vacuumPos;
            set
            {
               BoundsCheck(value);
               _vacuumPos = value;
            }
        }
        
        /** NxN array of floor squares in the world */
        private List<List<Square>> _squares;

        public VacuumWorldState(int size)
        {
            _squares = new List<List<Square>>();
            for (var i = 0; i < size; i++)
            {
                _squares.Add(Enumerable.Range(0, size).Select(x => new Square()).ToList());
            }
        }
    
        public VacuumWorldState Clone()
        {
            var size = _squares.Count;
            var copy = new VacuumWorldState(_squares.Count) {_squares = new List<List<Square>>()};
            for (var i = 0; i < size; i++)
            {
                copy._squares.Add(_squares[i].Select(s => new Square {IsDirty = s.IsDirty}).ToList());
            }
            copy.VacuumPos = VacuumPos;
            return copy;
        }

        public Square GetSquare(Point2D pos)
        {
            BoundsCheck(pos);
            return _squares[pos.X][pos.Y];
        }
        
        public Square GetSquare(int x, int y)
        {
            BoundsCheck(x);
            BoundsCheck(y);
            return _squares[x][y];
        }
        
        private void BoundsCheck(Point2D pos)
        {
            BoundsCheck(pos.X);
            BoundsCheck(pos.Y);
        }

        private void BoundsCheck(int n)
        {
            if (n < 0 || n > _squares.Count)
            {
                throw new IndexOutOfRangeException();
            }
        }
    }

    public class Square
    {
        public bool IsDirty { get; set; } = false;
    }

    public struct Point2D
    {
        public int X { get; }
        public int Y { get; }
        
        public Point2D(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}