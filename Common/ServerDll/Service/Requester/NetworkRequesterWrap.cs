using Net;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServerDll.Service.Requester
{
    public interface IRequester
    {
        void Connect(string ip,ushort port,string param);
        void Send(byte[] raw);
        void Send(PtMessagePackage package);
        void Disconnect();
        void Tick();
        Action OnConnected { get;set; }
        Action OnDisconnected { get;set; }
        Action OnError { set; get; }
        Action<byte[]> OnMessage { set; get; }
    }
    public class NetworkRequesterWrap
    {
        public NetworkType Type { private set; get; }
        public IRequester Requester { private set; get; }
        public NetworkRequesterWrap(NetworkType type)
        {
            Type = type;
            if(type == NetworkType.WSS)
                Requester = new WebsocketRequester();
            else
                Requester = new KcpRequester();
        }
        public void Connect(string ip,ushort port,string param)
        {
            Requester.Connect(ip,port,param);
        }
        public void Send(byte[] raw)
        {
            Requester.Send(raw);
        }
        public void Send(PtMessagePackage package)
        {
            Requester.Send(package);
        }
        public void Disconnect()
        {
            Requester.Disconnect();
        }
        public void Tick()
        {
            Requester.Tick();
        }
    }
}
