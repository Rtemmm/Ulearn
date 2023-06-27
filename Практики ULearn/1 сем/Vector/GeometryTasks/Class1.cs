using System;

namespace GeometryTasks
{
    public class Vector
    {
        public double X;
        public double Y;

        public Vector()
        {
        }
        
        public Vector(Vector vector1, Vector vector2)
        {
            X = vector1.X + vector2.X;
            Y = vector1.Y + vector2.Y;
        }

        public double GetLength()
        {
            return Geometry.GetLength(this);
        }
        
        public Vector Add(Vector vector)
        {
            return Geometry.Add(this, vector);
        }
        
        public bool Belongs(Segment segment)
        {
            return Geometry.IsVectorInSegment(this, segment);
        }
    }    
    
    public class Geometry
    {
        public static double GetLength(double x, double y)
        {
            return Math.Sqrt(x * x + y * y);
        }

        public static double GetLength(Vector vector)
        {
            return GetLength(vector.X, vector.Y);
        }

        public static double GetLength(Segment segment)
        {
            return GetLength(segment.End.X - segment.Begin.X, segment.End.Y - segment.Begin.Y);
        }
        
        public static bool IsVectorInSegment(Vector vector, Segment segment)
        {
            return Math.Abs((GetLength(new Segment(segment.Begin, vector)) +
                             GetLength(new Segment(vector, segment.End))) - GetLength(segment)) < 1e-9;
        }
        
        public static Vector Add(Vector vector1, Vector vector2)
        {
            return new Vector(vector1, vector2);
        }
    }

    public class Segment
    {
        public Vector Begin;
        public Vector End;

        public Segment()
        {
        }
        
        public Segment(Vector begin, Vector end)
        {
            Begin = begin;
            End = end;
        }
        
        public double GetLength()
        {
            return Geometry.GetLength(this);
        }

        public bool Contains(Vector vector)
        {
            return Geometry.IsVectorInSegment(vector, this);
        }
    }
}