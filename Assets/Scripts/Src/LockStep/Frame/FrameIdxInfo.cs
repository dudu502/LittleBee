using Components;


namespace LogicFrameSync.Src.LockStep.Frame
{
    /// <summary>
    /// 操作信息
    /// 用于网络通信
    /// </summary>
    public class FrameIdxInfo
    {
        public int Idx { set; get; }
        public int Cmd { set; get; }
        public int EntityId { set; get; }
        public string[] Params { set; get; }
        

        public FrameIdxInfo(int cmd,int eId,string[] param)
        {
            Cmd = cmd;
            EntityId = eId;
            Params = param;
        }
        public FrameIdxInfo(int cmd,int eId)
        {
            Cmd = cmd;
            EntityId = eId;
            Params = new string[0];
        }
        public FrameIdxInfo() { }

        public override string ToString()
        {
            return string.Format("[CMD={0} IDx={1} EntityID={2}]",Cmd,Idx,EntityId);
        }
        public bool EqualsInfo(FrameIdxInfo target)
        {
            return EntityId == target.EntityId && Cmd == target.Cmd && Idx == target.Idx;
        }

        public bool EqualsInfo(IComponent component)
        {
            return EntityId == component.EntityId && Cmd == component.GetCommand();
        }

        public static byte[] Write(FrameIdxInfo info)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteInt32(info.Idx);
            buffer.WriteInt32(info.Cmd);
            buffer.WriteInt32(info.EntityId);
            int size = info.Params.Length;
            buffer.WriteInt32(size);
            for (int i = 0; i < size; ++i)
                buffer.WriteString(info.Params[i]);
            return buffer.Getbuffer();
        }

        public static FrameIdxInfo Read(byte[] bytes)
        {
            ByteBuffer buffer = new ByteBuffer(bytes);
            FrameIdxInfo info = new FrameIdxInfo();
            info.Idx = buffer.ReadInt32();
            info.Cmd = buffer.ReadInt32();
            info.EntityId = buffer.ReadInt32();
            int size = buffer.ReadInt32();
            info.Params = new string[size];
            for(int i=0;i<size;++i)
            {
                info.Params[i] = buffer.ReadString();
            }
            return info;
        }
    }
}
