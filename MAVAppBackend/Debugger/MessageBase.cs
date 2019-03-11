using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace MAVAppBackend.Debugger
{
    public abstract class MessageBase : IMessage
    {
        public int Id { get; }
        public string Name { get; set; }

        public MessageBase(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public abstract string Data { get; }

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
