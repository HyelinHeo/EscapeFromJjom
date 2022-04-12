using System;

namespace MPXObject
{
    [Serializable]
    public struct Point3
    {
        public float X;
        public float Y;
        public float Z;

        public Point3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Point3(Point2 point2) : this(point2.X, point2.Y, 0) { }

        public static Point3 Zero { get { return new Point3(0, 0, 0); } }
        public static Point3 One { get { return new Point3(1, 1, 1); } }

        public static bool operator ==(Point3 a, Point3 b) => a.X == b.X && a.Y == b.Y && a.Z == b.Z ? true : false;
        public static bool operator !=(Point3 a, Point3 b) => a.X == b.X && a.Y == b.Y && a.Z == b.Z ? false : true;

        public override bool Equals(object obj)
        {
            Point3 vec = (Point3)obj;
            return vec.X == X && vec.Y == Y && vec.Z == Z;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("({0},{1},{2})", X, Y, Z);
        }
    }
}
