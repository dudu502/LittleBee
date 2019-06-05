using LogicFrameSync.Src.LockStep.Frame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Src.Replays
{
    /// <summary>
    /// 战报信息
    /// 存储所有的关键帧
    /// </summary>
    public class ReplayInfo
    {
        public long OwnerId = 0;
        public string Version = "v1.0";
        public List<List<FrameIdxInfo>> Frames;

        public static byte[] Write(ReplayInfo info)
        {
            using (ByteBuffer buffer = new ByteBuffer())
            {
                buffer.WriteLong(info.OwnerId);
                buffer.WriteString(info.Version);

                int size = info.Frames.Count;
                buffer.WriteInt32(size);
                for (int i = 0; i < size; ++i)
                {
                    int infoCount = info.Frames[i].Count;
                    buffer.WriteInt32(infoCount);
                    for (int j = 0; j < infoCount; ++j)
                    {
                        FrameIdxInfo fInfo = info.Frames[i][j];
                        buffer.WriteBytes(FrameIdxInfo.Write(fInfo));
                    }
                }
                return ByteBuffer.CompressBytes(buffer.Getbuffer());
            }            
        }

        public static ReplayInfo Read(byte[] bytes)
        {
            using (ByteBuffer buffer = new ByteBuffer(ByteBuffer.Decompress(bytes)))
            {
                ReplayInfo info = new ReplayInfo();
                info.OwnerId = buffer.ReadLong();
                info.Version = buffer.ReadString();
                info.Frames = new List<List<FrameIdxInfo>>();

                int size = buffer.ReadInt32();
                for (int i = 0; i < size; ++i)
                {
                    int infoCount = buffer.ReadInt32();
                    List<FrameIdxInfo> list = new List<FrameIdxInfo>();
                    info.Frames.Add(list);
                    for (int j = 0; j < infoCount; ++j)
                    {
                        list.Add(FrameIdxInfo.Read(buffer.ReadBytes()));
                    }
                }
                return info;
            }              
        }
    }
}
