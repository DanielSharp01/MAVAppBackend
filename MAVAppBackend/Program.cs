using MAVAppBackend.Parser;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
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

                var list = StationParser.Parse(response).ToList();
                Console.Clear();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
