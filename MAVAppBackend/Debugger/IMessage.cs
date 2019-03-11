using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAVAppBackend.Debugger
{
    public interface IMessage
    {
        int Id { get; }
        string ToResponseString();
    }
}
