using System;

namespace MPXObject
{
    [Serializable]
    public struct Point2
    {
        public float X;
        public float Y;

        public static Point2 Zero { get { return new Point2(0, 0); } }
        public static Point2 One { get { return new Point2(1, 1); } }

        public Point2(float x, float y)
        {
            X = x;
            Y = y;
        }

        public override bool Equals(object obj)
        {
            Point2 vec = (Point2)obj;
            return vec.X == X && vec.Y == Y;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("({0},{1})", X, Y);
        }
    }
}
