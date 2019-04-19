using System;
using System.Text;

namespace vacuum_world
{
    public class VacuumWorldState : IEquatable<VacuumWorldState>
    {
        public int WorldSize { get; }

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
        
        /** NxN array of floor squares in the world. value = isDirty? */
        private readonly bool[,] _squares;

        public VacuumWorldState(int size)
        {
            WorldSize = size;
            _squares = new bool[size, size];
        }
    
        public VacuumWorldState Clone()
        {
            var copy = new VacuumWorldState(WorldSize) {VacuumPos = VacuumPos};

            for (var y = 0; y < WorldSize; y++)
            {
                for (var x = 0; x < WorldSize; x++)
                {
                    copy._squares[x, y] = _squares[x, y];
                }
            }

            return copy;
        }

        public bool SquareIsDirty(int x, int y)
        {
            BoundsCheck(x);
            BoundsCheck(y);

            return _squares[x, y];
        }
        
        public bool SquareIsDirty(Point2D pos)
        {
            return SquareIsDirty(pos.X, pos.Y);
        }

        public void SetSquareIsDirty(int x, int y, bool isDirty)
        {
            BoundsCheck(x);
            BoundsCheck(y);

            _squares[x, y] = isDirty;
        }
        
        public void SetSquareIsDirty(Point2D pos, bool isDirty)
        {
            SetSquareIsDirty(pos.X, pos.Y, isDirty);
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
            var size = WorldSize;

            for (var y = 0; y < size; y++)
            {
                for (var x = 0; x < size; x++)
                {
                    unchecked
                    {
                        hashCode += (_squares[x, y] ? 1 : 0) << (y * size + x);
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

        private static bool SquaresAreEqual(bool[,] squaresA, bool [,] squaresB)
        {
            // assume same size
            var size = squaresA.GetLength(0);

            for (var y = 0; y < size; y++)
            {
                for (var x = 0; x < size; x++)
                {
                    if (squaresA[x, y] != squaresB[x, y]) return false;
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
            if (n < 0 || n >= WorldSize)
            {
                throw new IndexOutOfRangeException();
            }
        }
        
        public override string ToString()
        {
            var sb = new StringBuilder();
            
            for (var y = 0; y < WorldSize; y++)
            {
                for (var x = 0; x < WorldSize; x++)
                {
                    if (VacuumPos.Equals(new Point2D(x, y)))
                    {
                        sb.Append("V");
                    }
                    else
                    {
                        sb.Append(_squares[x, y] ? "X" : ".");
                    }
                }
                sb.AppendLine();
            }

            return sb.ToString();
        }
    }
}
