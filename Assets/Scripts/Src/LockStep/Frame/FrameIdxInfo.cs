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
        public System.Guid EntityId { set; get; }
        public string[] Params { set; get; }
        

        public FrameIdxInfo(int cmd, string eId,string[] param)
        {
            Cmd = cmd;
            EntityId = new System.Guid( eId);
            if (param == null)
                Params = new string[0];
            else
                Params = param;
        }
        public FrameIdxInfo(int idx,int cmd, string eId, string[] param)
        {
            Idx = idx;
            Cmd = cmd;
            EntityId = new System.Guid(eId);
            if (param == null)
                Params = new string[0];
            else
                Params = param;
        }
        public FrameIdxInfo(int cmd, string eId)
        {
            Cmd = cmd;
            EntityId = new System.Guid(eId);
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
            buffer.WriteString(info.EntityId.ToString());
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
            info.EntityId = new System.Guid(buffer.ReadString());
            int size = buffer.ReadInt32();
            info.Params = new string[size];
            for(int i=0;i<size;++i)
                info.Params[i] = buffer.ReadString();
            return info;
        }
    }
}
