using HtmlAgilityPack;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MAVAppBackend.MAV
{
    /// <summary>
    /// Request to the common MAV API endpoint, use different API classes for specific request types
    /// </summary>
    public class APIRequest
    {
        /// <summary>
        /// Internal HttpClient
        /// </summary>
        private static readonly HttpClient client = new HttpClient(new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate });

        static APIRequest()
        {
            client.DefaultRequestHeaders.AcceptEncoding.TryParseAdd("gzip, deflate");
            client.DefaultRequestHeaders.Host = "vonatinfo.mav-start.hu";
            client.DefaultRequestHeaders.Referrer = new Uri("http://vonatinfo.mav-start.hu");
        }

        /// <summary>
        /// JSON Object used for the request
        /// </summary>
        public JObject RequestObject { get; }

        /// <param name="requestObject">JSON Object used for the request</param>
        public APIRequest(JObject requestObject)
        {
            RequestObject = requestObject;
        }

        /// <summary>
        /// Requests the API and returns the response
        /// </summary>
        /// <returns>API response contains JSON object and status code</returns>
        public async Task<APIResponse> GetResponse()
        {
            HttpStatusCode statusCode = HttpStatusCode.NotFound;
            try
            {
                var response = await client.PostAsync("http://vonatinfo.mav-start.hu/map.aspx/getData", new StringContent(RequestObject.ToString(), Encoding.UTF8, "application/json"));
                statusCode = response.StatusCode;
                var responseString = await response.Content.ReadAsStringAsync();
                return new APIResponse(statusCode, RequestObject, JObject.Parse(responseString));
            }
            catch (HttpRequestException)
            {
                return new APIResponse(statusCode, RequestObject, null);
            }
            catch (JsonReaderException)
            {
                return new APIResponse(statusCode, RequestObject, null);
            }
        }
    }
}
