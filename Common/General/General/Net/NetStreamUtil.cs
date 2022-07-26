using System.Collections.Concurrent;
using System.Net;
using System.Threading;

namespace Net
{
    public class NetStreamUtil
    {
        public static bool Send(LiteNetLib.NetPeer peer, PtMessagePackage package)
        {
            if (peer != null)
            {
                peer.Send(PtMessagePackage.Write(package), LiteNetLib.DeliveryMethod.ReliableOrdered);
                return true;
            }
            return false;
        }
        public static bool Send(LiteNetLib.NetPeer peer, ushort messageId, byte[] content)
        {
            return Send(peer, PtMessagePackage.Build(messageId, content));
        }
        public static bool SendToAll(LiteNetLib.NetManager manager, PtMessagePackage package)
        {
            if (manager != null)
            {
                manager.SendToAll(PtMessagePackage.Write(package), LiteNetLib.DeliveryMethod.ReliableOrdered);
                return true;
            }
            return false;
        }
        public static bool SendToAll(LiteNetLib.NetManager manager,ushort messageId,byte[] content)
        {
            return SendToAll(manager, PtMessagePackage.Build(messageId,content));
        }

        public static bool SendUnconnnectedRequest(LiteNetLib.NetManager manager,ushort messageId,byte[] content,IPEndPoint endPoint)
        {
            return SendUnconnnectedRequest(manager,PtMessagePackage.Build(messageId,content),endPoint);
        }
        public static bool SendUnconnnectedRequest(LiteNetLib.NetManager manager, PtMessagePackage package, IPEndPoint endPoint)
        {
            if(manager!=null)
            {
                manager.SendUnconnectedMessage(PtMessagePackage.Write(package),endPoint);
                return true;
            }
            return false;
        }
    }
}
