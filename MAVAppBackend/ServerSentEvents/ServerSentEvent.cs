using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAVAppBackend.ServerSentEvents
{
    public struct ServerSentEvent
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public JObject Data { get; set; }

        public string ToResponseString()
        {
            var builder = new StringBuilder();
            builder.Append($"id: {Id}\n");
            builder.Append($"event: {Name}\n");
            builder.Append($"data: {Data.ToString()}\n");
            builder.Append("\n");
            return builder.ToString();
        }
    }
}
