using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MAVAppBackend.Entities
{
    public class Station
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool FullKnowledge { get; set; }

        private Station() { }

        public Station(string name)
        {
            Name = name;
        }

        public static string NormalizeName(string stationName)
        {
            stationName = stationName.ToLower();

            stationName = stationName.Replace('á', 'a');
            stationName = stationName.Replace('é', 'e');
            stationName = stationName.Replace('í', 'i');
            stationName = stationName.Replace('ó', 'o');
            stationName = stationName.Replace('ö', 'o');
            stationName = stationName.Replace('ő', 'o');
            stationName = stationName.Replace('ú', 'u');
            stationName = stationName.Replace('ü', 'u');
            stationName = stationName.Replace('ű', 'u');

            stationName = stationName.Replace("railway station crossing", "");
            stationName = stationName.Replace("railway station", "");
            stationName = stationName.Replace("train station", "");
            stationName = stationName.Replace("station", "");
            stationName = stationName.Replace("vonatallomas", "");
            stationName = stationName.Replace("vasutallomas", "");
            stationName = stationName.Replace("mav pu", "");
            stationName = stationName.Replace("-", " ");

            Regex regex = new Regex("[ ]{2,}", RegexOptions.None);
            stationName = regex.Replace(stationName, " ");

            return stationName.Trim();
        }
    }
}
