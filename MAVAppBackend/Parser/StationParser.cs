using HtmlAgilityPack;
using MAVAppBackend.MAV;
using MAVAppBackend.Parser.Statements;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
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
        public static IEnumerable<ParserStatement> Parse(APIResponse response)
        {
            if (!response.RequestSucceded)
            {
                yield return new ErrorStatement(response, ErrorTypes.RequestUnsuccessful);
                yield break;
            }

            if (response.Result != null)
            {
                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(HttpUtility.HtmlDecode(response.Result.ToString()));
                var table = document.DocumentNode.Descendants("table").Where(d => d.HasClass("af")).FirstOrDefault();
                if (table != null)
                {
                    var stationName = table.Descendants("th").Where(d => d.HasClass("title")).FirstOrDefault().ChildNodes.FirstOrDefault().InnerText;
                    var stationId = new StationIdentification(response, stationName, null);
                    yield return stationId;

                    foreach (HtmlNode tr in table.ChildNodes.Where(n => n.Name == "tr" && n.Attributes["onmouseover"] != null && n.Attributes["onmouseout"] != null))
                    {
                        var tds = tr.ChildNodes.Where(n => n.Name == "td").ToArray();
                        var arrival = TimeTuple.Parse(tds[0]);
                        var departure = TimeTuple.Parse(tds[1]);
                        var trainReference = tds[tds.Length > 3 ? 3 : 2];
                        var trainRefStatements = ParseTrainReference(response, trainReference).ToList();

                        foreach (var s in trainRefStatements)
                        {
                            yield return s;
                        }

                        if (!(trainRefStatements.First() is TrainIdentification trainId)) yield break;

                        var trainStationId = new TrainStation(response, trainId, stationId, arrival, departure);
                        yield return trainStationId;
                        var platform = tds.Length > 3 ? tds[2].InnerText.Trim() : null;
                        if (platform?.Length == 0) platform = null;
                        yield return new TrainStationPlatform(response, trainStationId, platform);
                    }
                }
                else yield return new ErrorStatement(response, ErrorTypes.NoTable);
            }
            else yield return new ErrorStatement(response, ErrorTypes.NoHtml);
        }

        public static IEnumerable<ParserStatement> ParseTrainReference(APIResponse response, HtmlNode trainReference)
        {
            var enumerator = trainReference.ChildNodes.AsEnumerable().GetEnumerator();
            TrainIdentification id;
            if (enumerator.MoveNext())
            {
                if (enumerator.Current.Name == "a")
                {
                    var trainNumber = CSExtensions.ParseInt(enumerator.Current.InnerText);
                    var elviraId = ElviraIdFromScript(enumerator.Current.Attributes["onclick"]?.Value);

                    if (trainNumber != null)
                    {
                        yield return id = new TrainIdentification(response, trainNumber.Value, elviraId);
                    }
                    else
                    {
                        yield return new ErrorStatement(response, ErrorTypes.TrainReferenceUnparsable);
                        yield break;
                    }
                }
                else
                {
                    yield return new ErrorStatement(response, ErrorTypes.TrainReferenceUnparsable);
                    yield break;
                }
            }
            else
            {
                yield return new ErrorStatement(response, ErrorTypes.TrainReferenceUnparsable);
                yield break;
            }

            if (enumerator.MoveNext())
            {
                var type = TrainParser.DetermineTrainType(enumerator.Current.InnerText.Trim());
                if (type != null)
                {
                    yield return new TrainHasType(response, id, type.Value);
                }
                else throw new Exception("Type does not map!");
            }
            else yield break;

            if (!enumerator.MoveNext()) yield break;

            if (enumerator.MoveNext())
            {
                var text = enumerator.Current.InnerText;
                var parts = text.Split(" -- ");
                var p0 = parts[0].Split((char)160);
                var p1 = parts[1].Split((char)160);

                if (TimeSpan.TryParse(p0[0].Trim(), out TimeSpan fromTime) && TimeSpan.TryParse(p1[1].Trim(), out TimeSpan toTime))
                {
                    var from = new StationIdentification(response, p0[1].Trim(), null);
                    var to = new StationIdentification(response, p1[0].Trim(), null);
                    yield return new TrainRelation(response, id, from, to, fromTime, toTime);
                }
            }
        }

        /// <summary>
        /// Parses the map.getData({ ... }) like looking javascript from the attribute
        /// </summary>
        /// <param name="origin">API response that was processed to make this statement</param>
        /// <param name="script">JavaScript to parse</param>
        /// <returns>StationIdentification statement or null if parsing fails</returns>
        public static string? ElviraIdFromScript(string? script)
        {
            if (script == null) return null;

            var mapGetData = script.Split(new char[] { ';' }, 2)[0];
            try
            {
                var data = JObject.Parse(mapGetData.Substr(mapGetData.IndexOf("{"), mapGetData.LastIndexOf("}")));
                var id = data["v"]?.ToString();
                return id;
            }
            catch (JsonReaderException)
            {
                return null;
            }
        }
    }
}
