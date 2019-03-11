using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace MAVAppBackend.ServerSentEvents
{
    public struct ServerSentEvent
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Data { get; set; }

        public ServerSentEvent(int id, string name, string data)
        {
            Id = id;
            Name = name;
            Data = data;
        }

        public string ToResponseString()
        {
            var builder = new StringBuilder();
            builder.Append($"id: {Id}\n");
            builder.Append($"event: {Name}\n");
            builder.Append($"data: {Data}\n");
            builder.Append("\n");
            return builder.ToString();
        }
    }
}
