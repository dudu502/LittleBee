using System;
namespace Net
{
    /// <summary>
    /// 数据包格式
    /// </summary>
    public class PtMessagePackage:IDisposable
    {
        public ushort MessageId;
        public bool HasContent { private set; get; }
        byte[] m_Content;

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

        public object ExtraObj = null;
        public static PtMessagePackage Build(ushort messageId)
        {
            PtMessagePackage package = new PtMessagePackage();
            package.MessageId = messageId;
            return package;
        }
        public static PtMessagePackage Build(ushort messageId,byte[] content)
        {
            PtMessagePackage package = Build(messageId);
            package.Content = content;
            return package;
        }
        public static PtMessagePackage BuildParams(ushort messageId,params object[] pars)
        {
            using (ByteBuffer buffer = new ByteBuffer())
            {
                foreach (object i in pars)
                {
                    Type iType = i.GetType();
                    if (iType == typeof(int))
                    {
                        buffer.WriteInt32((int)i);
                    }
                    else if (iType == typeof(uint))
                    {
                        buffer.WriteUInt32((uint)i);
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
                        buffer.WriteInt64((long)i);
                    }
                    else if (iType == typeof(ulong))
                    {
                        buffer.WriteUInt64((ulong)i);
                    }
                    else if (iType == typeof(short))
                    {
                        buffer.WriteInt16((short)i);
                    }
                    else if (iType == typeof(ushort))
                    {
                        buffer.WriteUInt16((ushort)i);
                    }
                    else if (iType == typeof(byte))
                    {
                        buffer.WriteByte((byte)i);
                    }
                    else if (iType == typeof(string))
                    {
                        buffer.WriteString((string)i);
                    }
                    else if (iType == typeof(byte[]))
                    {
                        buffer.WriteBytes((byte[])i);
                    }
                    else
                    {
                        throw new Exception("BuildParams Type is not supported. "+iType.ToString());
                    }
                }
                return Build(messageId, buffer.Getbuffer());
            }

        }
        public static PtMessagePackage Read(byte[] bytes)
        {
            using (ByteBuffer buffer = new ByteBuffer(bytes))
            {
                PtMessagePackage info = new PtMessagePackage();
                info.MessageId = buffer.ReadUInt16();
                info.HasContent = buffer.ReadBool();
                if (info.HasContent)
                {
                    info.Content = buffer.ReadBytes();
                }
                return info;
            }
        }
        public static byte[] Write(PtMessagePackage info)
        {
            using (ByteBuffer buffer = new ByteBuffer())
            {
                buffer.WriteUInt16(info.MessageId);
                buffer.WriteBool(info.HasContent);
                if (info.HasContent)
                {
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
            ExtraObj = null;
        }
    }
}
