using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace MAVAppBackend.MAV
{
    /// <summary>
    /// Request to the MAV STATION API endpoint
    /// </summary>
    public class StationAPIRequest
    {
        /// <summary>
        /// Underlying request to the common MAV API endpoint
        /// </summary>
        private readonly APIRequest api;

        /// <summary>
        /// Constructs the underlying common API request
        /// </summary>
        /// <param name="station">Name of the station</param>
        public StationAPIRequest(string station)
        {
            var request = new JObject
            {
                ["a"] = "STATION",
                ["jo"] = new JObject { ["a"] = station }
            };
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
