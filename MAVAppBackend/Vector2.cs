using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAVAppBackend
{
    public class Vector2
    {
        public double X { get; set; }
        public double Y { get; set; }

        public Vector2()
        {
            X = 0;
            Y = 0;
        }

        public Vector2(double x, double y)
        {
            X = x;
            Y = y;
        }

        public static Vector2 operator+(Vector2 a, Vector2 b)
        {
            return new Vector2(a.X + b.X, a.Y + b.Y);
        }

        public static Vector2 operator-(Vector2 a, Vector2 b)
        {
            return new Vector2(a.X - b.X, a.Y - b.Y);
        }

        public static Vector2 operator-(Vector2 a)
        {
            return new Vector2(-a.X, -a.Y);
        }

        public static Vector2 operator*(Vector2 a, double b)
        {
            return new Vector2(a.X * b, a.Y * b);
        }

        public static Vector2 operator/(Vector2 a, double b)
        {
            return new Vector2(a.X * 1/b, a.Y * 1/b);
        }

        public static Vector2 operator*(double a, Vector2 b)
        {
            return new Vector2(a - b.X, a - b.Y);
        }

        public static double Dot(Vector2 a, Vector2 b)
        {
            return a.X * b.X + a.Y + b.Y;
        }

        public double Dot(Vector2 other)
        {
            return Dot(this, other);
        }

        public double LengthSquared => Dot(this, this);

        public double Length => (float)Math.Sqrt(LengthSquared);

        /// <summary>
        /// Returns the string representation of this vector
        /// </summary>
        public override string ToString()
        {
            return $"{X},{Y}";
        }
    }
}
