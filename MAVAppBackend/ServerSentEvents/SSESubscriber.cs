using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAVAppBackend.ServerSentEvents
{
    public class SSESubscriber
    {
        private readonly HttpResponse response;

        public Guid Id { get; private set; }

        public int? LastEventId { get; private set; }

        public SSESubscriber(Guid id, HttpResponse response, int? lastEventId)
        {
            this.response = response;
            Id = id;
            LastEventId = lastEventId;
            response.ContentType = "text/event-stream";
        }

        public Task SendEventAsync(ServerSentEvent sse)
        {
            LastEventId = sse.Id;
            return response.WriteAsync(sse.ToResponseString());
        }
    }
}
