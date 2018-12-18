﻿using HtmlAgilityPack;
using MAVAppBackend.MAV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace MAVAppBackend.Parser
{
    public class TrainParser
    {
        /// <summary>
        /// Parses an API response from the TRAIN API
        /// </summary>
        /// <param name="response">API response to parse</param>
        /// <returns>Parsed information or null if the parsing can not even determine the train number</returns>
        public static TrainInfo? Parse(APIResponse response)
        {
            if (!response.RequestSucceded) return null;

            int? trainNumber = null;
            string? elviraId = null;
            DateTime requestDate = response.RequestObject["request-date"].ToObject<DateTime>();

            TrainInfo? trainInfo = null;
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
                trainInfo = new TrainInfo(trainNumber.Value, requestDate, elviraId);
            }

            if (response.Result?["html"] != null)
            {
                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(HttpUtility.HtmlDecode(response.Result?["html"].ToString()));

                TrainType? type = null;
                var table = document.DocumentNode.Descendants("table").Where(d => d.HasClass("vt")).FirstOrDefault()?.ChildNodes.Where(n => n.Name == "tbody").FirstOrDefault();
                if (table != null)
                {
                    void setupTrainHeader()
                    {
                        var headerRow = table.ChildNodes.Where(n => n.Name == "tr").FirstOrDefault();
                        if (headerRow == null) return;

                        var headerEnum = headerRow.ChildNodes.Elements().GetEnumerator();
                        if (!headerEnum.MoveNext()) return;

                        var headerTextSplit = headerEnum.Current.InnerText.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
                        if (headerTextSplit.Length > 0)
                        {
                            trainNumber = CSExtensions.ParseInt(headerTextSplit[0]);
                        }
                        if (trainNumber == null) return;

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

                        if (trainInfo == null)
                        {
                            trainInfo = new TrainInfo(trainNumber.Value, requestDate, elviraId);
                        }

                        trainInfo.Name = name;

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
                                trainInfo.ViszNumber = headerEnum.Current.InnerText;
                            }
                            else if (headerEnum.Current.Name == "font")
                            {
                                var relationText = headerEnum.Current.InnerText.Substr(1, -1).Split(',')[0].Trim();
                                var stationNames = relationText.Split(" - ").Select(s => s.Trim()).ToArray();
                                if (type != null)
                                {
                                    trainInfo.TrainRelations.Add(new TrainRelation(stationNames[0], stationNames[1], type.Value));
                                }
                            }
                            else if (headerEnum.Current.Name == "ul")
                            {
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
                                        trainInfo.TrainRelations.Add(new TrainRelation(fromStation, toStation, relType.Value));
                                    }
                                }
                            }
                        }
                    }
                    setupTrainHeader();
                    
                    if (trainInfo == null)
                        return null;

                    var expiryDateLink = document.DocumentNode.FirstChild.ChildNodes.SkipWhile(h => h.InnerText.Trim() != "Menetrend").SkipWhile(ul => ul.Name != "ul").FirstOrDefault()?.Descendants("li")
                            .FirstOrDefault(li => li.Attributes.Contains("style") && li.Attributes["style"].Value.Contains("bolder"))
                            ?.Descendants("a").FirstOrDefault();

                    trainInfo.EstimatedExpiry = expiryDateLink == null ? (DateTime?)null : DateTime.Parse(expiryDateLink.InnerText.Split('-')[1]);

                    foreach (HtmlNode tr in table.ChildNodes.Where(n => n.Name == "tr" && n.Attributes["onmouseover"] != null && n.Attributes["onmouseout"] != null))
                    {
                        var hit = tr.HasClass("row_past_odd") || tr.HasClass("row_past_even");
                        var tds = tr.ChildNodes.Where(n => n.Name == "td").ToArray();

                        var distance = CSExtensions.ParseInt(tds[0].InnerText);
                        var stationLink = tds[1].Descendants("a").FirstOrDefault();
                        var station = StationReference.FromScript(stationLink?.Attributes["onclick"]?.Value);
                        if (station == null) station = new StationReference(null, stationLink?.InnerText.Trim());
                        var arrival = TimeTuple.Parse(tds[2]);
                        var departure = TimeTuple.Parse(tds[3]);
                        var platform = (tds.Length > 4) ? tds[4].InnerText.Trim() : null;
                        if (platform?.Length == 0) platform = null;

                        if (station != null)
                        {
                            trainInfo.TrainStations.Add(new TrainStation(distance, station.Value, arrival, departure, hit, platform));
                        }
                    }
                }
            }

            if (trainInfo != null)
            {
                trainInfo.EncodedPolyline = response.Result?["line"][0]["points"]?.ToString();
            }

            return trainInfo;
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
            switch (str)
            {
                case "személyvonat": return TrainType.Local;
                case "gyorsvonat": return TrainType.Fast;
                case "sebesvonat": return TrainType.Fast;
                case "InterCity": return TrainType.InterCity;
                case "InterRégió": return TrainType.InterRegion;
                case "EuroCity": return TrainType.EuroCity;
                case "vonatpótló autóbusz": return TrainType.SubstitutionBus;
                case "InterCity pótló busz": return TrainType.SubstitutionBus;
                default: return null;
            }
        }
    }
}
