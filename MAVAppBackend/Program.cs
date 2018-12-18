using MAVAppBackend.Parser;
using MAVAppBackend.TestData;
using MAVAppBackend.TestDataPrep;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Web;

namespace MAVAppBackend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //CreateWebHostBuilder(args).Build().Run();

            while (true)
            {
                var response = Task.Run(async () =>
                {
                    //return await new MAV.TrainsAPIRequest().GetResponse();
                    //return await new MAV.TrainAPIRequest(trainId: int.Parse(Console.ReadLine())).GetResponse();
                    return await new MAV.StationAPIRequest(Console.ReadLine()).GetResponse();
                    //return await new MAV.RouteAPIRequest(Console.ReadLine(), Console.ReadLine()).GetResponse();
                }).GetAwaiter().GetResult();
                Console.Clear();

                //Console.WriteLine(HttpUtility.HtmlDecode(response.Result?.ToString()));
                //Console.WriteLine(JObject.FromObject(TrainParser.Parse(response)).ToString());
                //Console.WriteLine(response.ResponseObject?.ToString());
                StationTestData.WriteHtmlResponse(@"C:\Users\DanielSharp\Desktop\TestData\station_test.html", response);
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
