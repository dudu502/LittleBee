using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Net
{
    public class Client: IDisposable
    {
        public int ReadLoopIntervalMs { set; get; }
        private TcpClient m_TcpClient;
        public bool QueueStop { set; get; }
        private string m_Host;
        private int m_Port;
        public event EventHandler<Message> DataReceived;
        //public event Action Disconnected;
        public Client()
        {
            ReadLoopIntervalMs = 10;
        }
        public void Connect(string host,int port)
        {
            if (string.IsNullOrEmpty(host)) throw new Exception("host is null");
            m_Host = host;
            m_Port = port;
            StartClientThread();          
        }
        void StartClientThread()
        {
            ThreadPool.QueueUserWorkItem(ListenerLoop);
        }
        private void ListenerLoop(object state)
        {
            try
            {
                m_TcpClient = new TcpClient();
                m_TcpClient.Connect(m_Host, m_Port);
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
                return;
            }

            while (!QueueStop)
            {
                try
                {
                    //if (m_TcpClient.IsOnline())
                        RunLoopStep();
                    //else if(Disconnected!=null)
                    //    Disconnected();
                }
                catch { }
                Thread.Sleep(ReadLoopIntervalMs);
            }
        }

        private void RunLoopStep()
        {
            if (m_TcpClient == null) return;
            if (!m_TcpClient.Connected) return;
            int bytesAvailable = m_TcpClient.Available;
            if (bytesAvailable == 0)
            {
                Thread.Sleep(ReadLoopIntervalMs);
                return;
            }
            byte[] bytesReceived = new byte[bytesAvailable];
            m_TcpClient.Client.Receive(bytesReceived, 0, bytesAvailable, SocketFlags.None);
            
            if (DataReceived != null)
            {
                ByteBuffer buffer = new ByteBuffer(bytesReceived);
                while(buffer.GetPosition() < buffer.Getbuffer().Length && buffer.ReadInt32()==int.MaxValue)
                {                        
                    Message msg = new Message(buffer.ReadBytes(), m_TcpClient);
                    DataReceived(this, msg);
                }
                buffer.Dispose();  
            }           
        }
        public void Write(PtMessagePackage package)
        {
            Write(PtMessagePackage.Write(package));
        }
        public void Write(byte[] bytes)
        {
            if (m_TcpClient == null) throw new Exception("tcpclient is null");
            //m_TcpClient.GetStream().Write(bytes, 0, bytes.Length);
            NetworkStreamUtil.Write(m_TcpClient.GetStream(), bytes);
        }

        public void Dispose()
        {
            QueueStop = true;
            if(m_TcpClient != null)
            {
                try
                {
                    m_TcpClient.Close();
                }
                catch { }
                m_TcpClient = null;
            }
        }
    }
}
