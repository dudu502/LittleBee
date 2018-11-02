using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

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
            //TcpClient.GetStream().Write(data, 0, data.Length);
            NetworkStreamUtil.Write(TcpClient.GetStream(), data);
            Debug.Log("reply size:"+data.Length);
        }
        public void Reply(PtMessagePackage package)
        {
            Reply(PtMessagePackage.Write(package));
        }
    }
}
