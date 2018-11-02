using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetServiceImpl
{
    public enum S2CMessageId
    {
        ResponseClientConnected,
        ResponseEnterRoom,
        ResponseSyncKeyframes,
        ResponsePlayerReady,
        ResponseInitPlayer,
        ResponseAllPlayerReady,
    }

    public enum C2SMessageId
    {
        RequestEnterRoom,
        RequestSyncClientKeyframes,
        RequestInitPlayer,
        RequestPlayerReady,
    }
}
