using HtmlAgilityPack;
using MAVAppBackend.MAV;
using MAVAppBackend.Parser.Statements;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace MAVAppBackend.Parser
{
    public static class TrainParser
    {
        /// <summary>
        /// Parses an API response from the TRAIN API
        /// </summary>
        /// <param name="response">API response to parse</param>
        /// <returns>Parser statements</returns>
        public static IEnumerable<ParserStatement> Parse(APIResponse response)
        {
            if (!response.RequestSucceded)
            {
                yield return new ErrorStatement(response, ErrorTypes.RequestUnsuccessful);
                yield break;
            }

            int? trainNumber = null;
            string? elviraId = null;
            TrainIdentification? id = null;

            if (response.Param?["vsz"] != null)
            {
                trainNumber = CSExtensions.ParseInt(response.Param["vsz"].ToString().Substring(2));
            }
            if (response.Param?["v"] != null)
            {
                elviraId = response.Param["v"].ToString();
            }

            if (trainNumber != null)
            {
                yield return id = new TrainIdentification(response, trainNumber.Value, elviraId);
            }

            if (response.Result?["html"] != null)
            {
                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(HttpUtility.HtmlDecode(response.Result?["html"].ToString()));

                var table = document.DocumentNode.Descendants("table").Where(d => d.HasClass("vt")).FirstOrDefault()?.ChildNodes.Where(n => n.Name == "tbody").FirstOrDefault();
                if (table != null)
                {
                    foreach (ParserStatement statement in ProcessHeader(response, table, ref id, elviraId))
                    {
                        yield return statement;
                    }

                    if (id != null)
                    {
                        var expiryDateLink = document.DocumentNode.FirstChild.ChildNodes.SkipWhile(h => h.InnerText.Trim() != "Menetrend").SkipWhile(ul => ul.Name != "ul").FirstOrDefault()?.Descendants("li")
                                .FirstOrDefault(li => li.Attributes.Contains("style") && li.Attributes["style"].Value.Contains("bolder"))
                                ?.Descendants("a").FirstOrDefault();

                        yield return new TrainExpiry(response, id, expiryDateLink == null ? (DateTime?)null : DateTime.Parse(expiryDateLink.InnerText.Split('-')[1]));

                        StationIdentification? lastStation = null;
                        foreach (HtmlNode tr in table.ChildNodes.Where(n => n.Name == "tr" && n.Attributes["onmouseover"] != null && n.Attributes["onmouseout"] != null))
                        {
                            foreach (ParserStatement statement in ProcessStationTableRow(response, tr, id, ref lastStation))
                            {
                                yield return statement;
                            }
                        }
                    }
                }
                else yield return new ErrorStatement(response, ErrorTypes.NoTable);
            }
            else yield return new ErrorStatement(response, ErrorTypes.NoHtml);

            if (id != null && response.Result?["line"][0]["points"] != null)
            {
                yield return new TrainPolyline(response, id, response.Result?["line"][0]["points"]?.ToString());
            }
        }

        /// <summary>
        /// Processes the train header part of the HTML response
        /// </summary>
        /// <param name="response">API response</param>
        /// <param name="table">Table HtmlNode</param>
        /// <param name="id">Identifies the train</param>
        /// <param name="elviraId"></param>
        /// <returns>List of parser statements</returns>
        private static List<ParserStatement> ProcessHeader(APIResponse response, HtmlNode table, ref TrainIdentification? id, string? elviraId)
        {
            List<ParserStatement> ret = new List<ParserStatement>();
            TrainType? type = null;
            var headerRow = table.ChildNodes.Where(n => n.Name == "tr").FirstOrDefault();
            if (headerRow == null)
            {
                ret.Add(new ErrorStatement(response, ErrorTypes.NoTableRows));
                return ret;
            }

            var headerEnum = headerRow.ChildNodes.Elements().GetEnumerator();
            if (!headerEnum.MoveNext())
            {
                ret.Add(new ErrorStatement(response, ErrorTypes.HeaderEmpty));
                return ret;
            }

            var headerTextSplit = headerEnum.Current.InnerText.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
            if (id == null)
            {
                int? trainNumber = null;
                if (headerTextSplit.Length > 0)
                {
                    trainNumber = CSExtensions.ParseInt(headerTextSplit[0]);
                }

                if (trainNumber != null)
                {
                    ret.Add(id = new TrainIdentification(response, trainNumber.Value, elviraId));
                }
                else
                {
                    ret.Add(new ErrorStatement(response, ErrorTypes.NoTrainIdentification));
                }
            }

            if (id == null) return ret;

            string? name = null;
            for (int i = 1; i < headerTextSplit.Length; i++)
            {
                if ((type = DetermineTrainType(headerTextSplit[i])) != null)
                {
                    break;
                }
                else
                {
                    if (name == null) name = headerTextSplit[i];
                    else name += " " + headerTextSplit[i];
                }
            }

            ret.Add(new TrainName(response, id, name));

            while (headerEnum.MoveNext())
            {
                TrainType? detType = DetermineTrainType(headerEnum.Current);

                if (detType != null)
                {
                    if (type != null)
                    {
                        type = detType;
                    }
                }
                else if (headerEnum.Current.Name == "span" && headerEnum.Current.HasClass("viszszam2"))
                {
                    ret.Add(new TrainVisz(response, id, headerEnum.Current.InnerText));
                }
                else if (headerEnum.Current.Name == "font")
                {
                    var relationText = headerEnum.Current.InnerText.Substr(1, -1).Split(',')[0].Trim();
                    var stationNames = relationText.Split(" - ").Select(s => s.Trim()).ToArray();
                    
                    var from = new StationIdentification(response, stationNames[0], null);
                    var to = new StationIdentification(response, stationNames[1], null);
                    ret.Add(from);
                    ret.Add(to);
                    ret.Add(new TrainRelation(response, id, from, to));

                    if (type != null)
                    {
                        ret.Add(new TrainHasType(response, id, type.Value));
                    }
                }
                else if (headerEnum.Current.Name == "ul")
                {
                    StationIdentification? firstFrom = null;
                    StationIdentification? lastTo = null;
                    foreach (HtmlNode li in headerEnum.Current.ChildNodes.Where(n => n.Name == "li"))
                    {
                        var liEnum = li.ChildNodes.AsEnumerable().GetEnumerator();
                        string? fromStation = null, toStation = null;
                        TrainType? relType = null;
                        if (liEnum.MoveNext())
                        {
                            var text = liEnum.Current.InnerText.Trim();
                            var colonSplit = text.Split(':');
                            var relationSplit = colonSplit[0].Split(" - ");
                            fromStation = relationSplit[0].Trim();
                            toStation = relationSplit[1].Trim();
                            relType = DetermineTrainType(colonSplit[1].Trim());
                        }
                        if (liEnum.MoveNext())
                        {
                            if (liEnum.Current.Name == "img")
                            {
                                relType = DetermineTrainType(liEnum.Current);
                            }
                        }
                        if (fromStation != null && toStation != null && relType != null)
                        {
                            var from = new StationIdentification(response, fromStation, null);
                            if (firstFrom != null) firstFrom = from;
                            var to = new StationIdentification(response, toStation, null);
                            lastTo = to;
                            ret.Add(from);
                            ret.Add(to);
                            ret.Add(new TrainSubrelation(response, id, from, to, relType.Value));
                        }
                    }

                    if (firstFrom != null && lastTo != null)
                    {
                        ret.Add(new TrainRelation(response, id, firstFrom, lastTo));
                    }
                }
            }

            return ret;
        }

        /// <summary>
        /// Processes the station rows in the HTML response
        /// </summary>
        /// <param name="response">API response</param>
        /// <param name="row">Station row tr</param>
        /// <param name="number">Number of the train station (ordinal)</param>
        /// <param name="id">Identifies the train</param>
        /// <param name="lastStation">Last station (to link sequential stations together)</param>
        /// <returns>Parser statements</returns>
        private static List<ParserStatement> ProcessStationTableRow(APIResponse response, HtmlNode row, TrainIdentification id, ref StationIdentification? lastStation)
        {
            List<ParserStatement> ret = new List<ParserStatement>();
            var tds = row.ChildNodes.Where(n => n.Name == "td").ToArray();
            var stationLink = tds[1].Descendants("a").FirstOrDefault();
            var stationId = StationIdentification.FromScript(response, stationLink?.Attributes["onclick"]?.Value);
            if (stationId != null)
            {
                ret.Add(stationId);
            }
            else
            {
                ret.Add(new ErrorStatement(response, ErrorTypes.StationLinkUnparsable));
                ret.Add(stationId = new StationIdentification(response, stationLink?.InnerText.Trim(), null));
            }

            var hit = row.HasClass("row_past_odd") || row.HasClass("row_past_even");
            var distance = CSExtensions.ParseInt(tds[0].InnerText);
            var arrival = TimeTuple.Parse(tds[2]);
            var departure = TimeTuple.Parse(tds[3]);
            var platform = (tds.Length > 4) ? tds[4].InnerText.Trim() : null;
            if (platform?.Length == 0) platform = null;

            var trainStationId = new TrainStation(response, id, stationId, arrival, departure);
            ret.Add(trainStationId);
            if (lastStation != null)
            {
                ret.Add(new TrainStationLink(response, id, lastStation, stationId, true));
            }

            lastStation = stationId;

            ret.Add(new TrainStationDistance(response, trainStationId, distance));
            ret.Add(new TrainStationPlatform(response, trainStationId, platform));
            ret.Add(new TrainStationHit(response, trainStationId, hit));

            return ret;
        }

        /// <summary>
        /// Determines the train type from an HTML node
        /// </summary>
        /// <param name="node">HtmlNode which may contain train type information</param>
        /// <returns>Train type determined or null if the node does not refer to a train type</returns>
        public static TrainType? DetermineTrainType(HtmlNode node)
        {
            if (node.NodeType == HtmlNodeType.Text) return DetermineTrainType(node.InnerText);
            else if (node.Name == "img") return DetermineTrainType(node.Attributes["alt"].Value);
            else return null;
        }

        /// <summary>
        /// Determines the train type from a string
        /// </summary>
        /// <param name="str">String which may refer to a train type</param>
        /// <returns>Train type determined or null if the string does not refer to a train type</returns>
        public static TrainType? DetermineTrainType(string str)
        {
            switch (str.ToLower())
            {
                case "személyvonat": return TrainType.Local;
                case "személy": return TrainType.Local;
                case "gyorsított": return TrainType.Local;
                case "zónázó": return TrainType.Local;
                case "gyorsvonat": return TrainType.Fast;
                case "sebesvonat": return TrainType.Fast;
                case "sebes": return TrainType.Fast;
                case "gyors": return TrainType.Fast;
                case "InterCity": return TrainType.InterCity;
                case "nemz.ic": return TrainType.InterCity;
                case "ic": return TrainType.InterCity;
                case "interrégió": return TrainType.InterRegion;
                case "eurocity": return TrainType.EuroCity;
                case "ec": return TrainType.EuroCity;
                case "vonatpótló autóbusz": return TrainType.SubstitutionBus;
                case "intercity pótló busz": return TrainType.SubstitutionBus;
                default: return null;
            }
        }
    }
}
