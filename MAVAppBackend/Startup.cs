using MAVAppBackend.ServerSentEvents;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using System;
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

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            var connection = @"Server=localhost;Database=mavapp;User=root;Password=mysql;";
            services.AddDbContext<AppContext>(options => options.UseMySql(connection));
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
                var sseClient = new ServerSentEventClient(new Guid(), context.Response);
                await sseClient.SendEventAsync(new ServerSentEvent() { Id = 1, Name = "hello", Data = new JObject { ["key"] = "Hello SSE!" } });
                await Task.Delay(TimeSpan.FromSeconds(2));
                await sseClient.SendEventAsync(new ServerSentEvent() { Id = 1, Name = "hello", Data = new JObject { ["key"] = "I see you are curious!" } });
            }));
        }
    }
}
