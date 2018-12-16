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
        /// Request JSON object
        /// </summary>
        public JObject RequestObject { get; }

        /// <summary>
        /// Response JSON object, null if failed
        /// </summary>
        public JObject? ResponseObject { get; }

        /// <summary>
        /// Parsing succeded the response object is definetely non null
        /// </summary>
        public bool RequestSucceded => ResponseObject != null;

        /// <summary>
        /// Result part of the response object
        /// </summary>
        public JToken? Result => ResponseObject?["d"]["result"];

        /// <summary>
        /// Param part of the response object
        /// </summary>
        public JObject? Param => ResponseObject?["d"]["param"] as JObject;

        /// <param name="statusCode">Http status code</param>
        /// <param name="requestObject">Request JSON object</param>
        /// <param name="responseObject">Response JSON object, null if failed</param>
        public APIResponse(HttpStatusCode statusCode, JObject requestObject, JObject? responseObject)
        {
            StatusCode = statusCode;
            RequestObject = requestObject;
            ResponseObject = responseObject;
        }
    }
}
