namespace MAVAppBackend
{
    public enum Projection
    {
        Unprojected,
        LatitudeLongitude,
        UnscaledWebMercator,
        WebMercatorHungary
    }

    public class ProjVector2
    {
        public Vector2 Vec { get; }
        public Projection Projection { get; }
        public double X { get => Vec.X; set => Vec.X = value; }
        public double Y { get => Vec.Y; set => Vec.Y = value; }

        public ProjVector2(Projection projection = Projection.Unprojected)
        {
            Vec = new Vector2();
            Projection = projection;
        }

        public ProjVector2(double x, double y, Projection projection = Projection.Unprojected)
        {
            Vec = new Vector2(x, y);
            Projection = projection;
        }

        public ProjVector2(Vector2 vec, Projection projection = Projection.Unprojected)
        {
            Vec = new Vector2(vec.X, vec.Y);
            Projection = projection;
        }

        public static ProjVector2 operator+(ProjVector2 a, ProjVector2 b)
        {
            Projection projection = a.Projection == Projection.Unprojected ? b.Projection : a.Projection;

            if (a.Projection != b.Projection && a.Projection != Projection.Unprojected && b.Projection != Projection.Unprojected)
                projection = Projection.Unprojected;

            return new ProjVector2(a.Vec + b.Vec, projection);
        }

        public static ProjVector2 operator-(ProjVector2 a, ProjVector2 b)
        {
            Projection projection = a.Projection == Projection.Unprojected ? b.Projection : a.Projection;

            if (a.Projection != b.Projection && a.Projection != Projection.Unprojected && b.Projection != Projection.Unprojected)
                projection = Projection.Unprojected;

            return new ProjVector2(a.Vec - b.Vec, projection);
        }

        public static ProjVector2 operator-(ProjVector2 a)
        {
            return new ProjVector2(-a.Vec, a.Projection);
        }

        public static ProjVector2 operator*(ProjVector2 a, double b)
        {
            return new ProjVector2(a.Vec * b, a.Projection);
        }

        public static ProjVector2 operator/(ProjVector2 a, double b)
        {
            return new ProjVector2(a.Vec / b, a.Projection);
        }

        public static ProjVector2 operator*(double a, ProjVector2 b)
        {
            return new ProjVector2(a * b.Vec, b.Projection);
        }

        public static double Dot(ProjVector2 a, ProjVector2 b)
        {
            return Vector2.Dot(a.Vec, b.Vec);
        }

        public double Dot(ProjVector2 other)
        {
            return Dot(this, other);
        }

        public double LengthSquared => Vec.LengthSquared;

        public double Length => Vec.Length;

        public ProjVector2 ToProjection(Projection newProjection)
        {
            if (Projection == newProjection)
                return new ProjVector2(Vec, Projection);

            if (newProjection == Projection.Unprojected)
                return new ProjVector2(Vec, Projection.Unprojected);

            if (Projection == Projection.Unprojected)
                return new ProjVector2(Vec, newProjection);


            return new ProjVector2(ProjectionConversions.ProjectionToProjection(Projection, newProjection, Vec), newProjection);
        }

        public double MeterLength => Length * ProjectionConversions.MeterPerUnit(Projection);

        /// <summary>
        /// Returns the string representation of this projection vector
        /// </summary>
        public override string ToString()
        {
            return $"{Vec} in {Projection}";
        }
    }
}
