using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAVAppBackend.ServerSentEvents
{
    public class SSEPublisher
    {
        private readonly ConcurrentDictionary<Guid, SSESubscriber> subscribers = new ConcurrentDictionary<Guid, SSESubscriber>();

        private readonly List<ServerSentEvent> pastEvents = new List<ServerSentEvent>();

        public async void Subscribe(SSESubscriber subscriber)
        {
            subscribers.TryAdd(subscriber.Id, subscriber);

            if (subscriber.LastEventId.HasValue)
            {
                for (int i = subscriber.LastEventId.Value; i < pastEvents.Count; i++)
                {
                    await subscriber.SendEventAsync(pastEvents[i]);
                }
            }
        }

        public Task SendToAll(ServerSentEvent sse)
        {
            pastEvents.Add(sse);
            return Task.WhenAll(subscribers.Select(kvp => kvp.Value.SendEventAsync(sse)));
        }

        public void Unsubscribe(SSESubscriber client)
        {
            subscribers.TryRemove(client.Id, out _);
        }
    }
}
