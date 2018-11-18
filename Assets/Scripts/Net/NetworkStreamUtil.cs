using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Net
{
    internal class AsyncWriteStateObject
    {
        public ManualResetEvent eventDone;
        public NetworkStream stream;
        public Exception exception;
    }

    public class NetworkStreamUtil
    {
        public static void Write(NetworkStream stream,byte[] bytes)
        {
            AsyncWriteStateObject state = new AsyncWriteStateObject();
            state.eventDone = new ManualResetEvent(false);
            state.stream = stream;
            state.exception = null;

            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteInt32(int.MaxValue);
            buffer.WriteBytes(bytes);

            stream.BeginWrite(buffer.Getbuffer(), 0, buffer.Getbuffer().Length, AsyncWriteCallback, state);
        }
        public static void AsyncWriteCallback(IAsyncResult iar)
        {
            AsyncWriteStateObject asyncState = iar.AsyncState as AsyncWriteStateObject;
            try
            {
                asyncState.stream.EndWrite(iar);
                asyncState.stream.Flush();
            }
            catch (Exception e)
            {
                asyncState.exception = e;
            }
            finally
            {
                asyncState.eventDone.Set();
            }
        }       
    }
}
