using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAVAppBackend.ServerSentEvents
{
    public class ServerSentEventClient
    {
        private readonly HttpResponse response;

        public Guid Id { get; private set; }

        public ServerSentEventClient(Guid id, HttpResponse response)
        {
            this.response = response;
            Id = Id;
        }

        public Task SendEventAsync(ServerSentEvent sse)
        {
            return response.WriteAsync(sse.ToResponseString());
        }
    }
}
