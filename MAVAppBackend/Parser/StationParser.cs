using HtmlAgilityPack;
using MAVAppBackend.MAV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace MAVAppBackend.Parser
{
    public static class StationParser
    {
        /// <summary>
        /// Parses an API response from the STATION API
        /// </summary>
        /// <param name="response">API response to parse</param>
        /// <returns>Parsed information or null if the request failed</returns>
        public static StationInfo? Parse(APIResponse response)
        {
            if (!response.RequestSucceded || response.Result == null) return null;


            StationInfo? stationInfo = null;
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(HttpUtility.HtmlDecode(response.Result.ToString()));
            var table = document.DocumentNode.Descendants("table").Where(d => d.HasClass("af")).FirstOrDefault()?.ChildNodes.Where(n => n.Name == "tbody").FirstOrDefault();
            if (table != null)
            {
                stationInfo = new StationInfo(new StationReference(
                    response.RequestObject["jo"]["i"] != null ? CSExtensions.ParseInt(response.RequestObject["jo"]["i"].ToString()) : null,
                    response.RequestObject["jo"]["a"].ToString().Trim()));

                foreach (HtmlNode tr in table.ChildNodes.Where(n => n.Name == "tr" && n.Attributes["onmouseover"] != null && n.Attributes["onmouseout"] != null))
                {
                    var tds = tr.ChildNodes.Where(n => n.Name == "td").ToArray();
                    var arrival = TimeTuple.Parse(tds[0]);
                    var departure = TimeTuple.Parse(tds[1]);
                    //var trainReference = TrainReference.Parse(tds[tds.Length > 3 ? 3 : 2]);
                    var platform = tds.Length > 3 ? tds[2].InnerText.Trim() : null;
                    if (platform?.Length == 0) platform = null;

                    /*if (trainReference != null)
                    {
                        stationInfo.StationTrains.Add(new StationEntry(arrival, departure, trainReference, platform));
                    }*/
                }
            }

            return stationInfo;
        }
    }
}
