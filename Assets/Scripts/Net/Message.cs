using System.Net.Sockets;

namespace Net
{
    public class Message
    {
        public TcpClient TcpClient { get; private set; }
        public byte[] Data { get; private set; }
        public Message(byte[] data,TcpClient client)
        {
            Data = data;
            TcpClient = client;
        }

        public void Reply(byte[] data)
        {
            NetworkStreamUtil.Write(TcpClient.GetStream(), data);
        }

        /// <summary>
        /// Server 端快速回复
        /// </summary>
        /// <param name="package"></param>
        public void Reply(PtMessagePackage package)
        {
            Reply(PtMessagePackage.Write(package));
        }
    }
}
