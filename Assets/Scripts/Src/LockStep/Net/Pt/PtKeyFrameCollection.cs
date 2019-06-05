using LogicFrameSync.Src.LockStep.Frame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicFrameSync.Src.LockStep.Net.Pt
{
    /// <summary>
    /// 关键帧集合
    /// </summary>
    public class PtKeyFrameCollection
    {
        public List<FrameIdxInfo> KeyFrames;
        public int FrameIdx;
        public static PtKeyFrameCollection Read(byte[] bytes)
        {
            PtKeyFrameCollection info = new PtKeyFrameCollection();         
            info.KeyFrames = new List<FrameIdxInfo>();
            using (ByteBuffer buffer = new ByteBuffer(bytes))
            {
                info.FrameIdx = buffer.ReadInt32();
                int size = buffer.ReadInt32();
                for (int i = 0; i < size; ++i)
                {
                    info.KeyFrames.Add(FrameIdxInfo.Read(buffer.ReadBytes()));
                }
                return info;
            }               
        }
        public static byte[] Write(PtKeyFrameCollection info)
        {
            using (ByteBuffer buffer = new ByteBuffer())
            {
                buffer.WriteInt32(info.FrameIdx);
                buffer.WriteInt32(info.KeyFrames.Count);
                for (int i = 0; i < info.KeyFrames.Count; ++i)
                {
                    buffer.WriteBytes(FrameIdxInfo.Write(info.KeyFrames[i]));
                }
                return buffer.Getbuffer();
            }              
        }

        public void AddKeyFramesRange(PtKeyFrameCollection targetCollection)
        {
            KeyFrames.AddRange(targetCollection.KeyFrames);
        }
    }
}
