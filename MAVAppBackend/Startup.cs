using MAVAppBackend.ServerSentEvents;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MAVAppBackend
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        SSEPublisher ssePublisher;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            var connection = @"Server=localhost;Database=mavapp;User=root;Password=mysql;";
            services.AddDbContext<AppContext>(options => options.UseMySql(connection));

            ssePublisher = new SSEPublisher();

            int cnt = 0;
            new Thread(async () =>
            {
                while (cnt < 20)
                {
                    await Task.Delay(TimeSpan.FromSeconds(1));
                    _ = ssePublisher.SendToAll(new ServerSentEvent(cnt++, "test", $"{cnt} seconds passed"));
                }
            }).Start();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();

            app.Map("/sse-test", sse => sse.Run(async context =>
            {
                if (!context.Request.Headers.ContainsKey("Accept") || context.Request.Headers["Accept"] != "text/event-stream") return;

                int? lastEventId = context.Request.Headers.ContainsKey("Last-Event-ID") ? CSExtensions.ParseInt(context.Request.Headers["Last-Event-ID"]) : null;

                context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
                context.Response.Headers.Add("Cache-Control", "no-cache");
                var subscriber = new SSESubscriber(new Guid(), context.Response, lastEventId);
                ssePublisher.Subscribe(subscriber);

                await context.RequestAborted.WhenCancelled();

                ssePublisher.Unsubscribe(subscriber);
            }));
        }
    }
}
