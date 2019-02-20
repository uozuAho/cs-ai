using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace vacuum_world
{
    public class VacuumWorldState : IEquatable<VacuumWorldState>
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

        public bool Equals(VacuumWorldState other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return _vacuumPos.Equals(other._vacuumPos)
                   && SquaresAreEqual(_squares, other._squares);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            
            return Equals((VacuumWorldState) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (_vacuumPos.GetHashCode() * 397) ^ GetSquaresHashCode();
            }
        }

        private int GetSquaresHashCode()
        {
            var hashCode = 17;
            
            for (var i = 0; i < WorldSize; i++)
            {
                for (var j = 0; j < WorldSize; j++)
                {
                    unchecked
                    {
                        hashCode += _squares[i][j].IsDirty.GetHashCode();
                    }
                }
            }

            return hashCode;
        }

        public static bool operator ==(VacuumWorldState left, VacuumWorldState right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(VacuumWorldState left, VacuumWorldState right)
        {
            return !Equals(left, right);
        }

        private static bool SquaresAreEqual(IReadOnlyList<List<Square>> squaresA, IReadOnlyList<List<Square>> squaresB)
        {
            if (squaresA.Count != squaresB.Count) return false;

            for (var i = 0; i < squaresA.Count; i++)
            {
                if (squaresA[i].Count != squaresB[i].Count) return false;
                
                for (var j = 0; j < squaresA.Count; j++)
                {
                    if (squaresA[i][j].IsDirty != squaresB[i][j].IsDirty) return false;
                }
            }

            return true;
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
        
        public override string ToString()
        {
            var sb = new StringBuilder();
            
            for (var i = 0; i < WorldSize; i++)
            {
                for (var j = 0; j < WorldSize; j++)
                {
                    if (VacuumPos.Equals(new Point2D(i, j)))
                    {
                        sb.Append("V");
                    }
                    else
                    {
                        sb.Append(GetSquare(i, j).IsDirty ? "X" : ".");
                    }
                }
                sb.AppendLine();
            }

            return sb.ToString();
        }
    }

    public class Square
    {
        public bool IsDirty { get; set; }
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