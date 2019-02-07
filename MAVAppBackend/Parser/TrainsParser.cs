using MAVAppBackend.MAV;
using MAVAppBackend.Parser.Statements;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAVAppBackend.Parser
{
    public class TrainsParser
    {
        /// <summary>
        /// Parses an API response from the TRAINS API
        /// </summary>
        /// <param name="response">API response to parse</param>
        /// <returns>IEnumerable of ParserStatements</returns>
        public static IEnumerable<ParserStatement> Parse(APIResponse response)
        {
            if (!response.RequestSucceded)
            {
                yield return new ErrorStatement(response, ErrorTypes.RequestUnsuccessful);
                yield break;
            }

            if (response.Result?["Trains"]?["Train"] is JArray trainArray)
            {
                foreach (var train in trainArray)
                {
                    var tid = new TrainIdentification(response, null, train["@ElviraID"]?.ToString());
                    if (tid.ElviraId == null) continue;

                    yield return tid;
                    if (train["@Delay"] != null)
                    {
                        yield return new TrainDelay(response, tid, train["@Delay"].ToObject<int>());
                    }
                    if (train["@Lat"] != null && train["@Lon"] != null)
                    {
                        yield return new TrainPosition(response, tid, train["@Lat"].ToObject<double>(), train["@Lon"].ToObject<double>());
                    }
                }
            }
            else
            {
                yield return new ErrorStatement(response, ErrorTypes.NoResult);
                yield break;
            }
        }
    }
}
