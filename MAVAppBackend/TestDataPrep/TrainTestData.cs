using MAVAppBackend;
using MAVAppBackend.MAV;
using MAVAppBackend.Parser;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MAVAppBackend.TestData
{
    /// <summary>
    /// Test data preparation utilities for the TRAIN API
    /// </summary>
    public static class TrainTestData
    {
        /// <summary>
        /// Requests train test data and writes it to the project's folder
        /// </summary>
        public static async Task Request()
        {
            var response = await new TrainAPIRequest(trainId: int.Parse(Console.ReadLine())).GetResponse();
            using (StreamWriter writer = new StreamWriter("train_test_" + response.RequestObject["jo"]["vsz"].ToString().Substring(2) + ".json"))
            {
                writer.Write(response.ResponseObject?.ToString());
            }
        }

        /// <summary>
        /// Writes the HTML response of the API response object into a .html file
        /// </summary>
        /// <param name="file">File to write to</param>
        /// <param name="responseObject">API response object</param>
        public static void WriteHtmlResponse(string file, APIResponse responseObject)
        {
            using (StreamWriter writer = new StreamWriter(File.Open(file, FileMode.Create), Encoding.UTF8))
            {
                writer.Write("<style>.row_past_even, .row_past_odd {background: gray;}</style>");
                writer.Write(HttpUtility.HtmlDecode(responseObject.Result?["html"].ToString()));
            }
        }

        /// <summary>
        /// Gets an API response from a test dump
        /// </summary>
        /// <param name="file">Test file</param>
        /// <param name="requestDate">Date of the request you want to enforce</param>
        /// <param name="trainId">Train id you want the request to have</param>
        /// <param name="elviraId">Elvira id you want the request to have</param>
        /// <returns></returns>
        public static APIResponse GetAPIResponseForTestFile(string file, DateTime requestDate, int? trainId, string? elviraId = null)
        {
            var request = new JObject
            {
                ["a"] = "TRAIN",
                ["jo"] = new JObject(),
                ["request-date"] = requestDate
            };
            if (trainId != null) request["jo"]["vsz"] = "55" + trainId;
            if (elviraId != null) request["jo"]["v"] = elviraId;

            using (StreamReader reader = new StreamReader(file, Encoding.UTF8))
            {
                var responseObject = JObject.Parse(reader.ReadToEnd());
                if (trainId != null) responseObject["d"]["param"]["vsz"] = "55" + trainId;
                if (elviraId != null) responseObject["d"]["param"]["v"] = elviraId;

                return new APIResponse(HttpStatusCode.OK, request, responseObject);
            }
        }
    }
}
