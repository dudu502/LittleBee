using Net;
using System.Net;

namespace Synchronize.Game.Lockstep.Service.Event
{
    public class UnconnectedNetMessageEvt
    {
        public IPEndPoint RemoteEndPoint { private set; get; }
        public byte[] Content { private set; get; }
        public UnconnectedNetMessageEvt(IPEndPoint iPEndPoint,byte[] content)
        {
            RemoteEndPoint = iPEndPoint;
            Content = content;
        }

        public void Reply(LiteNetLib.NetManager manager,byte[] content)
        {
            if(manager!=null)
                manager.SendUnconnectedMessage(content, RemoteEndPoint);
        }
    }
}
