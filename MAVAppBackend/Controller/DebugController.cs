using MAVAppBackend.Debugger;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MAVAppBackend.Controller
{
    public class DebugController : ControllerBase
    {
        private static Logger logger = new Logger(new RollingList<LogMessage>(10000));

        public static void Run()
        {
            new Thread(async () =>
            {
                int i = 0;
                while (true)
                {
                    await Task.Delay(1000);
                    logger.Log("Parser log " + i, new[] { "parser" });
                    logger.Log("Parser other log " + i, new[] { "parser", "other" });
                    logger.Log("Dispatcher log " + i++, new[] { "dispatcher", });
                }
            }).Start();
        }

        public async Task<IActionResult?> Logger([FromRoute] string? loggerName)
        {
            Response.Headers.Add("Cache-Control", "no-cache");

            if (Request.Headers.ContainsKey("Accept") && Request.Headers["Accept"] == "text/event-stream")
            {
                var client = new LoggerClient(new Guid(), loggerName, Request, Response, ControllerContext.HttpContext.RequestAborted);
                logger.ConnectClient(client);
                await ControllerContext.HttpContext.RequestAborted.WhenCancelled();
                logger.DisconnectClient(client);
                return null;
            }
            else
            {
                return Ok(logger.GetFirstTimeResult(loggerName, Request));
            }
        }
    }
}
