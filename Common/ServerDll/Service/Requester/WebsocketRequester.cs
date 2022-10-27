using Net;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;

namespace ServerDll.Service.Requester
{
    public class WebsocketRequester : IRequester
    {
        private WebSocket socket;

        public Action OnConnected { set; get; }
        public Action OnDisconnected { set; get; }
        public Action OnError { set; get; }
        public Action<byte[]> OnMessage { set; get; }

        public void Connect(string ip,ushort port,string param)
        {
            socket = new WebSocket($"ws://{ip}:{port}/{param}");
            socket.OnOpen += (sender, e) => OnConnected?.Invoke();
            socket.OnMessage += (sender, item) => OnMessage?.Invoke(item.RawData);
            socket.OnClose += (sender,e)=> OnDisconnected?.Invoke();
            socket.OnError += (sender, e) => OnError?.Invoke();
            Task.Run(() => socket.Connect());// socket.Connect();
        }

        public void Disconnect()
        {
            socket.CloseAsync();
        }

        public void Send(byte[] raw)
        {
            Task.Run(()=>socket.Send(raw));
        }

        public void Send(PtMessagePackage package)
        {
            Send(PtMessagePackage.Write(package));
        }

        public void Tick()
        {
            
        }
    }
}
