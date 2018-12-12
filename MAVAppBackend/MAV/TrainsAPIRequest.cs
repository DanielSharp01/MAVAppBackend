using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace MAVAppBackend.MAV
{
    /// <summary>
    /// Request to the MAV TRAINS API endpoint
    /// </summary>
    public class TrainsAPIRequest
    {
        /// <summary>
        /// Underlying request to the common MAV API endpoint
        /// </summary>
        private readonly APIRequest api;

        /// <summary>
        /// Constructs the underlying common API request
        /// </summary>
        public TrainsAPIRequest()
        {
            var request = new JObject
            {
                ["a"] = "TRAINS",
                ["jo"] = new JObject() { ["id"] = false }
            };
            api = new APIRequest(request);
        }

        /// <summary>
        /// Requests the API and returns the response
        /// </summary>
        /// <returns>API response contains JSON object and status code</returns>
        public async Task<APIResponse> GetResponse()
        {
            return await api.GetResponse();
        }
    }
}
