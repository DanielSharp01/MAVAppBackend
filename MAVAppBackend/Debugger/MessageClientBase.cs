using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MAVAppBackend.Debugger
{
    public class MessageClientBase<TMessage> : IMessageClient<TMessage> where TMessage : IMessage
    {
        private readonly HttpResponse response;
        private readonly CancellationToken cancellationToken;

        public Guid Id { get; }

        public int? LastMessageId { get; protected set; }

        public MessageClientBase(Guid id, HttpRequest request, HttpResponse response, CancellationToken cancellationToken)
        {
            this.response = response;
            this.cancellationToken = cancellationToken;
            Id = id;
            LastMessageId = request.Headers.ContainsKey("Last-Event-ID") ? CSExtensions.ParseInt(request.Headers["Last-Event-ID"]) : null;
            response.ContentType = "text/event-stream";
        }

        public virtual Task SendMessageAsync(TMessage message)
        {
            LastMessageId = message.Id;
            return response.WriteAsync(message.ToResponseString(), cancellationToken);
        }
    }
}
