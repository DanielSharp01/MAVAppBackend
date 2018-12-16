using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAVAppBackend.MAV
{
    /// <summary>
    /// Request to the MAV TRAIN API endpoint
    /// </summary>
    public class TrainAPIRequest
    {
        /// <summary>
        /// Underlying request to the common MAV API endpoint
        /// </summary>
        private readonly APIRequest api;

        /// <summary>
        /// Constructs the underlying common API request
        /// </summary>
        /// <param name="trainId">Train number (usually 4 digits)</param>
        /// <param name="elviraId">Elvira ID is a train instance identifier with the format xxxxxx_yymmdd</param>
        public TrainAPIRequest(int? trainId = null, string? elviraId = null)
        {
            var request = new JObject
            {
                ["a"] = "TRAIN",
                ["jo"] = new JObject(),
                ["request-date"] = DateTime.Now
            };
            if (trainId != null) request["jo"]["vsz"] = "55" + trainId;
            if (elviraId != null) request["jo"]["v"] = elviraId;
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
