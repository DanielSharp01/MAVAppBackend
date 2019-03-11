using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAVAppBackend.Debugger
{
    public interface IMessageClient<TMessage> where TMessage : IMessage
    {
        Guid Id { get; }
        int? LastMessageId { get; }
        Task SendMessageAsync(TMessage message);
    }
}
