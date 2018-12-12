using HtmlAgilityPack;
using Newtonsoft.Json.Linq;
using System.Net;

namespace MAVAppBackend.MAV
{
    /// <summary>
    /// MAV API Response
    /// </summary>
    public class APIResponse
    {
        /// <summary>
        /// Http status code
        /// </summary>
        public HttpStatusCode StatusCode { get; }

        /// <summary>
        /// Response JSON object, null if failed
        /// </summary>
        public JObject? ResponseObject { get; }

        /// <summary>
        /// Parsing succeded the response object is definetely non null
        /// </summary>
        public bool ParsingSucceded => ResponseObject != null;

        /// <summary>
        /// Result part of the response object
        /// </summary>
        public JToken? Result => ResponseObject?["d"]["result"];

        /// <summary>
        /// Param part of the response object
        /// </summary>
        public JObject? Param => ResponseObject?["d"]["param"] as JObject;

        /// <param name="statusCode">Http status code</param>
        /// <param name="responseObject">Response JSON object, null if failed</param>
        public APIResponse(HttpStatusCode statusCode, JObject? responseObject)
        {
            StatusCode = statusCode;
            ResponseObject = responseObject;
        }
    }
}
