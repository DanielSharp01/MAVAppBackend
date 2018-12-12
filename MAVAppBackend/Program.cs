using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;

namespace MAVAppBackend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //CreateWebHostBuilder(args).Build().Run();

            var response = Task.Run(async () =>
            {
                return await new MAV.TrainsAPIRequest().GetResponse();
                //return await new MAV.TrainAPIRequest(trainId: 2610).GetResponse();
                //return await new MAV.StationAPIRequest("Monor").GetResponse();
                //return await new MAV.RouteAPIRequest("Monor", "Nyugati").GetResponse();
            }).GetAwaiter().GetResult();

            Console.WriteLine(response.Result?.ToString());

            Console.ReadLine();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
