using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MAVAppBackend.Debugger
{
    public class LogMessage : MessageBase
    {
        public string Message { get; }
        public string[] Tags { get; }

        public LogMessage(int id, string message, string[] tags)
            : base(id, "logger")
        {
            Message = message;
            Tags = tags;
        }

        public override string Data => (new JObject() { ["Message"] = Message, ["Tags"] = new JArray(Tags) }).ToString(Formatting.None);
    }

    public class LoggerClient : MessageClientBase<LogMessage>
    {
        public string[] FilteredTags { get; }
        public LoggerClient(Guid id, string? loggerName, HttpRequest request, HttpResponse response, CancellationToken cancellationToken)
            : base(id, request, response, cancellationToken)
        {
            List<string> filteredTags = new List<string>();
            if (loggerName != null)
                filteredTags.Add(loggerName);

            filteredTags.AddRange(request.Query.Select(kvp => kvp.Key));
            FilteredTags = filteredTags.ToArray();
        }

        public override Task SendMessageAsync(LogMessage message)
        {
            if (FilteredTags.All(t => message.Tags.Contains(t)))
                return base.SendMessageAsync(message);
            else
                return Task.CompletedTask;
        }
    }

    public class Logger : MessageServerBase<LogMessage, LoggerClient>
    {
        private int counter = 0;
        public Logger(IList<LogMessage> startingMessages)
            : base(startingMessages)
        { }

        public void Log(string message, string[] tags)
        {
            SendToAll(new LogMessage(counter++, message, tags));

            if (counter < 0) counter = 0; // should it ever overflow
        }

        public string GetFirstTimeResult(string? loggerName, HttpRequest request)
        {
            List<string> filteredTags = new List<string>();
            if (loggerName != null)
                filteredTags.Add(loggerName);

            filteredTags.AddRange(request.Query.Select(kvp => kvp.Key));

            var filteredMessages = messages.Where(message => filteredTags.All(t => message.Tags.Contains(t)));
            return new JArray(filteredMessages.Select(message => JObject.FromObject(message))).ToString(Formatting.None);
        }
    }
}
