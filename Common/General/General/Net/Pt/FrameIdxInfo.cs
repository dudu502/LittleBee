
using System;

namespace Net.Pt
{
    /// <summary>
    /// 操作信息
    /// 用于网络通信
    /// </summary>
    public class FrameIdxInfo: IComparable<FrameIdxInfo>
    {
        public ushort Cmd { set; get; }
        public uint EntityId { set; get; }
        public byte[] ParamsContent { set; get; }
        
        public FrameIdxInfo(ushort cmd, uint eId, byte[] param)
        {
            Cmd = cmd;
            EntityId = eId;
            ParamsContent = param;
        }     
        public FrameIdxInfo() { }

        public override string ToString()
        {
            return string.Format("[CMD={0} EntityID={1} Content={2}]",Cmd,EntityId,string.Join(",",ParamsContent));
        }
        public bool EqualsInfo(FrameIdxInfo target)
        {
            return EntityId == target.EntityId && Cmd == target.Cmd;
        }

        public int CompareTo(FrameIdxInfo other)
        {
            if (EntityId == other.EntityId)
            {
                return Cmd.CompareTo(other.Cmd);
            }
            else
            {
                return EntityId.CompareTo(other.EntityId);
            }
        }
        public static byte[] Write(FrameIdxInfo info)
        {
            using (ByteBuffer buffer = new ByteBuffer())
            {
                buffer.WriteUInt16(info.Cmd);
                buffer.WriteUInt32(info.EntityId);
                buffer.WriteBytes(info.ParamsContent);
                return buffer.Getbuffer();
            }
             
        }

        public static FrameIdxInfo Read(byte[] bytes)
        {
            using (ByteBuffer buffer = new ByteBuffer(bytes))
            {
                FrameIdxInfo info = new FrameIdxInfo();
                info.Cmd = buffer.ReadUInt16();
                info.EntityId = buffer.ReadUInt32();
                info.ParamsContent = buffer.ReadBytes();
                return info;

            }
                
        }

    }
}
