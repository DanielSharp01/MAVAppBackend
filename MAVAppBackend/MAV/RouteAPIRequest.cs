using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;

namespace MAVAppBackend.MAV
{
    /// <summary>
    /// Request to the MAV ROUTE API endpoint
    /// </summary>
    public class RouteAPIRequest
    {
        /// <summary>
        /// Underlying request to the common MAV API endpoint
        /// </summary>
        private readonly APIRequest api;

        /// <summary>
        /// Constructs the underlying common API request
        /// </summary>
        /// <param name="fromStation">Name of the station to search route from</param>
        /// <param name="toStation">Name of the station to search route to</param>
        /// <param name="touchingStation">Name of the station to touch</param>
        /// <param name="date">Date to search at (time part is ignored)</param>
        public RouteAPIRequest(string fromStation, string toStation, string? touchingStation = null, DateTime? date = null)
        {
            date = date ?? DateTime.Now;
            var request = new JObject
            {
                ["a"] = "ROUTE",
                ["jo"] = new JObject
                {
                    ["i"] = fromStation,
                    ["e"] = toStation,
                    ["d"] = date.Value.ToString("yyyy.MM.dd.")
                }
            };
            if (touchingStation != null) request["jo"]["v"] = touchingStation;
            api = new APIRequest(request);
        }

        /// <summary>
        /// Requests the API and returns the response
        /// </summary>
        /// <returns>API response contains HTML result, JSON object and status code</returns>
        public async Task<APIResponse> GetResponse()
        {
            return await api.GetResponse();
        }
    }
}
