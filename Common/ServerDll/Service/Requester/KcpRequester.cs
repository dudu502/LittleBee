using kcp2k;
using Net;
using Service.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServerDll.Service.Requester
{
    public class KcpRequester : IRequester
    {
        private KcpClient kcpClient;
        public Action OnConnected { set; get; }
        public Action OnDisconnected { set; get; }
        public Action OnError { set; get; }
        public Action<byte[]> OnMessage { set; get; }
        public KcpRequester()
        {
            
        }
        public void Connect(string address,ushort port,string param)
        {
            Logger.LogInfo($"Connect {OnConnected!=null} {OnMessage!=null} {OnDisconnected!=null}");
            kcpClient = new KcpClient(_OnConnected, _OnMessage, _OnDisconnected, _OnError);
            kcpClient.Connect(address, port, true, 10);
        }
        void _OnConnected()
        {
            Logger.LogInfo("\n _OnConnected \n" + (OnConnected!=null));
            if(OnConnected!=null)
                OnConnected();
        }
        void _OnMessage(ArraySegment<byte> raw,KcpChannel channel)
        {
            Logger.LogInfo("\n _OnMessage offset" + (raw.Offset));
            
            byte[] result = new byte[raw.Count];
            Array.Copy(raw.Array, raw.Offset, result, 0, result.Length);

            
            if (OnMessage != null)
                OnMessage(result);
        }
        void _OnDisconnected()
        {
            Logger.LogInfo("\n _OnDisconnected \n" + (OnDisconnected != null));
            if (OnDisconnected != null)
                OnDisconnected();
        }
        void _OnError(ErrorCode code,string reason)
        {
            Logger.LogInfo("\n _OnError "+ code +"  "+ (reason));
            if (OnError != null)
                OnError();
        }
        public void Disconnect()
        {
            kcpClient.Disconnect();
        }

        public void Send(byte[] raw)
        {
            kcpClient.Send(new ArraySegment<byte>(raw), KcpChannel.Reliable);
        }

        public void Send(PtMessagePackage package)
        {
            Send(PtMessagePackage.Write(package));
        }

        public void Tick()
        {
            kcpClient.Tick();
        }
    }
}
