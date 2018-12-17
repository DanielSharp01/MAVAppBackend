using MAVAppBackend.Parser;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Threading.Tasks;

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
                    return await new MAV.TrainAPIRequest(trainId: int.Parse(Console.ReadLine())).GetResponse();
                    //return await new MAV.StationAPIRequest(Console.ReadLine()).GetResponse();
                    //return await new MAV.RouteAPIRequest(Console.ReadLine(), Console.ReadLine()).GetResponse();
                }).GetAwaiter().GetResult();
                Console.Clear();

                //Console.WriteLine(response.Result?["html"].ToString());
                Console.WriteLine(JObject.FromObject(TrainParser.Parse(response)).ToString());
                /*Console.WriteLine(response.ResponseObject?.ToString());
                using (StreamWriter writer = new StreamWriter("train_test_" + response.RequestObject["jo"]["vsz"].ToString().Substring(2) + ".json"))
                {
                    writer.Write(response.ResponseObject?.ToString());
                }*/
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
