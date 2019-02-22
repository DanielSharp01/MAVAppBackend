using System;

namespace MAVAppBackend
{
    public static class ProjectionConversions
    {
        /// <summary>
        /// Radius of the Earth in kms
        /// </summary>
        public const int EarthRadius = 6371;

        /// <summary>
        /// Radians to degrees
        /// </summary>
        /// <param name="rad">Radians</param>
        public static double Deg(double rad)
        {
            return rad / Math.PI * 180.0;
        }

        /// <summary>
        /// Degrees to radians
        /// </summary>
        /// <param name="deg">Degrees</param>
        public static double Rad(double deg)
        {
            return deg / 180.0 * Math.PI;
        }

        public static Vector2 ProjectionToProjection(Projection from, Projection to, Vector2 vec)
        {
            switch (from)
            {
                case Projection.LatitudeLongitude:
                    switch (to)
                    {
                        case Projection.UnscaledWebMercator: return LatitudeLongitudeToUnscaledWebMercator(vec);
                        case Projection.WebMercatorHungary: return LatitudeLongitudeToWebMercatorHungary(vec);
                        default: throw new NotImplementedException();
                    }
                case Projection.UnscaledWebMercator:
                    switch (to)
                    {
                        case Projection.LatitudeLongitude: return UnscaledWebMercatorToLatitudeLongitude(vec);
                        case Projection.WebMercatorHungary: return UnscaledWebMercatorToWebMercatorHungary(vec);
                        default: throw new NotImplementedException();
                    }
                case Projection.WebMercatorHungary:
                    switch (to)
                    {
                        case Projection.LatitudeLongitude: return WebMercatorHungaryToLatitudeLongitude(vec);
                        case Projection.UnscaledWebMercator: return WebMercatorHungaryToUnscaledWebMercator(vec);
                        default: throw new NotImplementedException();
                    }
                default: throw new NotImplementedException();
            }
        }

        public static Vector2 LatitudeLongitudeToUnscaledWebMercator(Vector2 vec)
        {
            return new Vector2((vec.Y / 180 + 1) / 2, (Math.PI - Math.Log(Math.Tan(Math.PI / 4 + vec.X * Math.PI / 360))) / (2 * Math.PI));
        }

        public static Vector2 UnscaledWebMercatorToLatitudeLongitude(Vector2 vec)
        {
            return new Vector2((Math.Atan(Math.Exp(Math.PI - 2 * Math.PI * vec.Y)) - Math.PI / 4 * 360 / Math.PI), 180 * (vec.X * 2 - 1));
        }

        public const int DefaultTileSize = 256;
        public static Vector2 HungaryCenter => new Vector2(47.1569903, 18.4769959);

        private static Vector2? hungaryCenterWebMercator = null;
        public static Vector2 HungaryCenterWebMercator => hungaryCenterWebMercator ??= LatitudeLongitudeToUnscaledWebMercator(new Vector2(47.1569903, 18.4769959));
        public const double HungaryZoom = 8.13;

        public static Vector2 UnscaledWebMercatorToWebMercatorHungary(Vector2 vec)
        {
            return (vec - HungaryCenterWebMercator) * DefaultTileSize * Math.Pow(2, HungaryZoom);
        }

        public static Vector2 WebMercatorHungaryToUnscaledWebMercator(Vector2 vec)
        {
            return vec / DefaultTileSize / Math.Pow(2, HungaryZoom) + HungaryCenterWebMercator;
        }

        public static Vector2 LatitudeLongitudeToWebMercatorHungary(Vector2 vec)
        {
            return UnscaledWebMercatorToWebMercatorHungary(LatitudeLongitudeToUnscaledWebMercator(vec));
        }

        public static Vector2 WebMercatorHungaryToLatitudeLongitude(Vector2 vec)
        {
            return UnscaledWebMercatorToLatitudeLongitude(WebMercatorHungaryToUnscaledWebMercator(vec));
        }

        public static double MeterPerUnit(Projection projection)
        {
            switch (projection)
            {
                case Projection.UnscaledWebMercator: return ProjectionConversions.MeterPerUnitUnscaledWebMercator();
                case Projection.WebMercatorHungary: return ProjectionConversions.MeterPerUnitWebMercatorHungary();
                default: throw new NotImplementedException();
            }
        }

        public static double MeterPerUnitUnscaledWebMercator()
        {
            return Math.Cos(HungaryCenter.X * Math.PI / 180) * 6378137 * 2 * Math.PI;
        }

        public static double MeterPerUnitWebMercatorHungary()
        {
            return Math.Cos(HungaryCenter.X * Math.PI / 180) * 6378137 * 2 * Math.PI / (DefaultTileSize * Math.Pow(2, HungaryZoom));
        }
    }
}
