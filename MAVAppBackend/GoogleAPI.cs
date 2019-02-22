using MAVAppBackend.Entities;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MAVAppBackend
{
    public class PlacesAPIException : Exception
    {
        public PlacesAPIException(string message) : base(message)
        { }
    }

    public class PlacesAPI
    {
        /// <summary>
        /// Your own Google Places API key
        /// Note: You do have to set this up in your environment variables.
        /// </summary>
        private static string APIKey = Environment.GetEnvironmentVariable("PlacesAPIKey", EnvironmentVariableTarget.User);

        private struct PlacesAPIData
        {
            public string StationName { get; }
            public ProjVector2 Position { get; }

            public PlacesAPIData(string stationName, ProjVector2 position)
            {
                StationName = stationName;
                Position = position;
            }
        }

        public static ProjVector2? Search(string stationName, ProjVector2 position, double radius)
        {
            int minDist = 0;
            PlacesAPIData? minRes = null;
            var results = Scan(position, radius);
            stationName = Station.NormalizeName(stationName);
            foreach (var result in results)
            {
                int dist = LevenshteinDistance(stationName, Station.NormalizeName(result.StationName));
                if (minRes == null || dist < minDist)
                {
                    minRes = result;
                    minDist = dist;
                }
            }

            return minRes?.Position;
        }

        private static List<PlacesAPIData> Scan(ProjVector2 position, double radius)
        {
            List<PlacesAPIData> ret = new List<PlacesAPIData>();
            HttpWebRequest request = WebRequest.CreateHttp("https://maps.googleapis.com/maps/api/place/nearbysearch/json?location=" + position.Vec.ToString() + "&radius=" + radius + "&type=train_station&key=" + APIKey);
            request.Method = "GET";
            try
            {
                using (WebResponse response = request.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                    {
                        string json = reader.ReadToEnd();
                        JObject whole = JObject.Parse(json);
                        string status = whole["status"].ToString();
                        if (status == "OK" || status == "ZERO_RESULTS")
                        {
                            if (whole["results"] is JArray results)
                            {
                                foreach (JObject place in results)
                                {
                                    ret.Add(new PlacesAPIData(
                                        place["name"].ToString(),
                                        new ProjVector2(double.Parse(place["geometry"]["location"]["lat"].ToString()), double.Parse(place["geometry"]["location"]["lng"].ToString()), Projection.LatitudeLongitude)));
                                }
                            }
                        }
                        else
                        {
                            throw new PlacesAPIException("Places API is unavailable. Status code: " + status);
                        }
                    }
                }
            }
            catch (WebException e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e.Message);
                Console.ResetColor();
                throw new PlacesAPIException("Places API is unavailable.");
            }

            return ret;
        }

        private static int LevenshteinDistance(string a, string b)
        {
            if (string.IsNullOrEmpty(a) && string.IsNullOrEmpty(b))
            {
                return 0;
            }
            if (string.IsNullOrEmpty(a))
            {
                return b.Length;
            }
            if (string.IsNullOrEmpty(b))
            {
                return a.Length;
            }

            int lengthA = a.Length;
            int lengthB = b.Length;

            var distances = new int[lengthA + 1, lengthB + 1];
            for (int i = 0; i <= lengthA; distances[i, 0] = i++) ;
            for (int j = 0; j <= lengthB; distances[0, j] = j++) ;

            for (int i = 1; i <= lengthA; i++)
                for (int j = 1; j <= lengthB; j++)
                {
                    int cost = b[j - 1] == a[i - 1] ? 0 : 1;
                    distances[i, j] = Math.Min(
                        Math.Min(distances[i - 1, j] + 1, distances[i, j - 1] + 1),
                        distances[i - 1, j - 1] + cost);
                }

            return distances[lengthA, lengthB];
        }
    }
}
