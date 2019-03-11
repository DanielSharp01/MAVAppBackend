using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MAVAppBackend.Debugger
{
    public abstract class MessageServerBase<TMessage, TClient> where TMessage : IMessage where TClient : IMessageClient<TMessage>
    {
        protected readonly ConcurrentDictionary<Guid, TClient> clients = new ConcurrentDictionary<Guid, TClient>();

        protected readonly IList<TMessage> messages;

        public MessageServerBase(IList<TMessage> startingMessages)
        {
            messages = startingMessages;
        }

        public async void ConnectClient(TClient client)
        {
            clients.TryAdd(client.Id, client);

            if (client.LastMessageId.HasValue)
            {
                for (int i = client.LastMessageId.Value; i < messages.Count; i++)
                {
                    await client.SendMessageAsync(messages[i]);
                }
            }
        }

        public Task SendToAll(TMessage message)
        {
            messages.Add(message);
            return Task.WhenAll(clients.Select(kvp => kvp.Value.SendMessageAsync(message)));
        }

        public void DisconnectClient(TClient client)
        {
            clients.TryRemove(client.Id, out _);
        }
    }
}
