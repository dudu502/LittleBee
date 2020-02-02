using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Net
{
    /// <summary>
    /// 数据包格式
    /// </summary>
    public class PtMessagePackage:IDisposable
    {
        public int MessageId;
        public bool HasContent = false;
        byte[] m_Content;

        public bool IsCompress = false;
        public byte[] Content {
            set {
                if (value != null)
                {
                    m_Content = value;
                    HasContent = true;
                }                  
            }

            get {
                return m_Content;
            }
        }

        public static PtMessagePackage Build(int messageId)
        {
            PtMessagePackage package = new PtMessagePackage();
            package.MessageId = messageId;
            return package;
        }
        public static PtMessagePackage Build(int messageId,byte[] content, bool compress = true)
        {
            PtMessagePackage package = Build(messageId);
            package.IsCompress = compress;
            package.Content = content;
            return package;
        }
        public static PtMessagePackage Build(int messageId,bool compress, params object[] pars)
        {
            using(ByteBuffer buffer = new ByteBuffer())
            {
                foreach (object i in pars)
                {
                    Type iType = i.GetType();
                    if (iType == typeof(int))
                    {
                        buffer.WriteInt32((int)i);
                    }
                    else if (iType == typeof(float))
                    {
                        buffer.WriteFloat((float)i);
                    }
                    else if (iType == typeof(bool))
                    {
                        buffer.WriteBool((bool)i);
                    }
                    else if (iType == typeof(long))
                    {
                        buffer.WriteLong((long)i);
                    }
                    else if (iType == typeof(short))
                    {
                        buffer.WriteShort((short)i);
                    }
                    else if (iType == typeof(byte))
                    {
                        buffer.WriteByte((byte)i);
                    }
                    else if(iType == typeof(string))
                    {
                        buffer.WriteString((string)i);
                    }
                    else if(iType == typeof(byte[]))
                    {
                        buffer.WriteBytes((byte[])i);
                    }
                    //todo
                }
                return Build(messageId,buffer.Getbuffer(), compress);
            }
      
        }
        public static PtMessagePackage Read(byte[] bytes)
        {
            using (ByteBuffer buffer = new ByteBuffer(bytes))
            {
                PtMessagePackage info = new PtMessagePackage();
                info.MessageId = buffer.ReadInt32();
                info.HasContent = buffer.ReadBool();
                if (info.HasContent)
                {
                    info.IsCompress = buffer.ReadBool();
                    if (info.IsCompress)
                        info.Content = ByteBuffer.Decompress(buffer.ReadBytes());
                    else
                        info.Content = buffer.ReadBytes();
                }
                return info;
            }
        }
        public static byte[] Write(PtMessagePackage info)
        {
            using (ByteBuffer buffer = new ByteBuffer())
            {
                buffer.WriteInt32(info.MessageId);
                buffer.WriteBool(info.HasContent);
                if (info.HasContent)
                {
                    buffer.WriteBool(info.IsCompress);
                    if (info.IsCompress)
                        buffer.WriteBytes(ByteBuffer.CompressBytes(info.Content));
                    else
                        buffer.WriteBytes(info.Content);
                }

                return buffer.Getbuffer();
            }         
        }


        public void Dispose()
        {
            m_Content = null;
            MessageId = 0;
            HasContent = false;
        }
    }
}
