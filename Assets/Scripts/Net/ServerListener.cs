using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Net
{
    public class ServerListener
    {
        private TcpListenerEx _listener = null;
        private List<TcpClient> _connectedClients = new List<TcpClient>();
        private List<TcpClient> _disconnectedClients = new List<TcpClient>();
        private Server _parent = null;
        public int ConnectedClientsCount
        {
            get { return _connectedClients.Count; }
        }
        public IEnumerable<TcpClient> ConnectedClients { get { return _connectedClients; } }

        internal TcpListenerEx Listener { get { return _listener; } }
        internal bool QueueStop { get; set; }
        internal IPAddress IPAddress { get; private set; }
        internal int Port { get; private set; }
        internal int ReadLoopIntervalMs { get; set; }

        public ServerListener(Server server,IPAddress ipaddress,int port)
        {
            QueueStop = false;
            _parent = server;
            IPAddress = ipaddress;
            Port = port;
            ReadLoopIntervalMs = 10;
            _listener = new TcpListenerEx(ipaddress, port);
            _listener.Start();
            ThreadPool.QueueUserWorkItem(ListenerLoop);
        }

        private void ListenerLoop(object state)
        {
            while (!QueueStop)
            {
                try
                {
                    RunLoopStep();
                }
                catch { }
                Thread.Sleep(ReadLoopIntervalMs);
            }
            _listener.Stop();
        }

        private void RunLoopStep()
        {
            if (_disconnectedClients.Count > 0)
            {
                var disconnectedClients = _disconnectedClients.ToArray();
                _disconnectedClients.Clear();

                foreach (var disC in disconnectedClients)
                {
                    _connectedClients.Remove(disC);
                    _parent.NotifyClientDisconnected(this, disC);
                }
            }

            if (_listener.Pending())
            {
                var newClient = _listener.AcceptTcpClient();
                _connectedClients.Add(newClient);
                _parent.NotifyClientConnected(this, newClient);
            }

            foreach(var c in _connectedClients)
            {
                if (!IsSocketConnected(c.Client))
                {
                    _disconnectedClients.Add(c);
                }

                int bytesAvailable = c.Available;
                if (bytesAvailable == 0)
                {
                    continue;
                }
                
                byte[] bytesReceived = new byte[bytesAvailable];
                c.Client.Receive(bytesReceived, 0, bytesAvailable, SocketFlags.None);
               
                ByteBuffer buffer = new ByteBuffer(bytesReceived);
                while(buffer.GetPosition()<buffer.Getbuffer().Length&&buffer.ReadInt32()==int.MaxValue)
                    _parent.NotifyEndTransmissionRx(this, c, buffer.ReadBytes());
                buffer.Dispose();
            }
        }

        bool IsSocketConnected(Socket s)
        {
            // https://stackoverflow.com/questions/2661764/how-to-check-if-a-socket-is-connected-disconnected-in-c
            bool part1 = s.Poll(1000, SelectMode.SelectRead);
            bool part2 = (s.Available == 0);
            if ((part1 && part2) || !s.Connected)
                return false;
            else
                return true;
        }
    }
}
