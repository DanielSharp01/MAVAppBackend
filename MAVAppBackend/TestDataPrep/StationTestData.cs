using MAVAppBackend.MAV;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MAVAppBackend.TestDataPrep
{
    /// <summary>
    /// Test data preparation utilities for the STATION API
    /// </summary>
    public class StationTestData
    {
        /// <summary>
        /// Requests station test data and writes it to the project's folder
        /// </summary>
        /// <param name="name">Name of the test file will be station_test_&lt;this parameter&gt;.json</param>
        public static async Task Request(string name)
        {
            var response = await new StationAPIRequest(Console.ReadLine()).GetResponse();
            using (StreamWriter writer = new StreamWriter("station_test_" + name + ".json"))
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
                writer.Write(HttpUtility.HtmlDecode(responseObject.Result?.ToString()));
            }
        }

        /// <summary>
        /// Gets an API response from a test dump
        /// </summary>
        /// <param name="file">Test file</param>
        /// <param name="station">Name of the station in the request object</param>
        /// <returns></returns>
        public static APIResponse GetAPIResponseForTestFile(string file, string? station)
        {
            var request = new JObject
            {
                ["a"] = "STATION",
                ["jo"] = new JObject(),
                ["a"] = station
            };

            using (StreamReader reader = new StreamReader(file, Encoding.UTF8))
            {
                var responseObject = JObject.Parse(reader.ReadToEnd());
                if (station != null) responseObject["d"]["param"]["a"] = station;

                return new APIResponse(HttpStatusCode.OK, request, responseObject);
            }
        }
    }
}
