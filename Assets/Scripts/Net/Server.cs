using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Net
{
    public class Server
    {
        private List<ServerListener> _listeners = new List<ServerListener>();
        public event EventHandler<TcpClient> ClientConnected;
        public event EventHandler<TcpClient> ClientDisconnected;
        public event EventHandler<Message> DataReceived;
        public Server()
        {

        }
        public void Start(int port)
        {
            var ipSorted = GetIPAddresses();
            foreach (var ipAddr in ipSorted)
            {
                try
                {
                    Start(ipAddr, port);
                }
                catch (SocketException ex)
                {
                    Debug.Log(ex.ToString());
                }
            }
        }
        public void Start(IPAddress ipAddress, int port)
        {
            ServerListener listener = new ServerListener(this, ipAddress, port);
            _listeners.Add(listener);       
        }
        public void Stop()
        {
            _listeners.All(l => l.QueueStop = true);
            while (_listeners.Any(l => l.Listener.Active))
            {
                Thread.Sleep(100);
            };
            _listeners.Clear();
        }
        public void Broadcast(byte[] data)
        {          
            foreach (var client in _listeners.SelectMany(x => x.ConnectedClients))
            {
                //client.GetStream().Write(data, 0, data.Length);

                NetworkStreamUtil.Write(client.GetStream(), data);
                  
                //client.GetStream().BeginWrite(data, 0, data.Length, (state) =>
                //{
                //    client.GetStream().EndWrite(state);
                //}, null);

                //IAsyncResult iar = client.GetStream().BeginWrite(data, 0, data.Length, null, null);
                //iar.AsyncWaitHandle.WaitOne();
                //client.GetStream().EndWrite(iar);            
            }
        }
        public void Broadcast(PtMessagePackage package)
        {
            Broadcast(PtMessagePackage.Write(package));
        }

        public List<IPAddress> GetListeningIPs()
        {
            List<IPAddress> listenIps = new List<IPAddress>();
            foreach (var l in _listeners)
            {
                if (!listenIps.Contains(l.IPAddress))
                {
                    listenIps.Add(l.IPAddress);
                }
            }
            return listenIps.OrderByDescending(ip => RankIpAddress(ip)).ToList();
        }
        public IEnumerable<IPAddress> GetIPAddresses()
        {
            List<IPAddress> ipAddresses = new List<IPAddress>();

            IEnumerable<NetworkInterface> enabledNetInterfaces = NetworkInterface.GetAllNetworkInterfaces()
                .Where(nic => nic.OperationalStatus == OperationalStatus.Up);
            foreach (NetworkInterface netInterface in enabledNetInterfaces)
            {
                IPInterfaceProperties ipProps = netInterface.GetIPProperties();
                foreach (UnicastIPAddressInformation addr in ipProps.UnicastAddresses)
                {
                    if (!ipAddresses.Contains(addr.Address))
                    {
                        ipAddresses.Add(addr.Address);
                    }
                }
            }

            var ipSorted = ipAddresses.OrderByDescending(ip => RankIpAddress(ip)).ToList();
            return ipSorted;
        }

        private int RankIpAddress(IPAddress addr)
        {
            int rankScore = 1000;

            if (IPAddress.IsLoopback(addr))
            {
                // rank loopback below others, even though their routing metrics may be better
                rankScore = 300;
            }
            else if (addr.AddressFamily == AddressFamily.InterNetwork)
            {
                rankScore += 100;
                // except...
                if (addr.GetAddressBytes().Take(2).SequenceEqual(new byte[] { 169, 254 }))
                {
                    // APIPA generated address - no router or DHCP server - to the bottom of the pile
                    rankScore = 0;
                }
            }

            if (rankScore > 500)
            {
                foreach (var nic in TryGetCurrentNetworkInterfaces())
                {
                    var ipProps = nic.GetIPProperties();
                    if (ipProps.GatewayAddresses.Any())
                    {
                        if (ipProps.UnicastAddresses.Any(u => u.Address.Equals(addr)))
                        {
                            // if the preferred NIC has multiple addresses, boost all equally
                            // (justifies not bothering to differentiate... IOW YAGNI)
                            rankScore += 1000;
                        }

                        // only considering the first NIC that is UP and has a gateway defined
                        break;
                    }
                }
            }

            return rankScore;
        }
        public void NotifyEndTransmissionRx(ServerListener listener, TcpClient client, byte[] msg)
        {
            if (DataReceived != null)
            {
                Message m = new Message(msg, client);
                DataReceived(this, m);
            }
        }

        public void NotifyClientConnected(ServerListener listener, TcpClient newClient)
        {
            if (ClientConnected != null)
            {
                ClientConnected(this, newClient);
            }
        }

        public void NotifyClientDisconnected(ServerListener listener, TcpClient disconnectedClient)
        {
            if (ClientDisconnected != null)
            {
                ClientDisconnected(this, disconnectedClient);
            }
        }

        public int ConnectedClientsCount
        {
            get
            {
                return _listeners.Sum(l => l.ConnectedClientsCount);
            }
        }
        private static IEnumerable<NetworkInterface> TryGetCurrentNetworkInterfaces()
        {
            try
            {
                return NetworkInterface.GetAllNetworkInterfaces().Where(ni => ni.OperationalStatus == OperationalStatus.Up);
            }
            catch (NetworkInformationException)
            {
                return Enumerable.Empty<NetworkInterface>();
            }
        }
    }
}
