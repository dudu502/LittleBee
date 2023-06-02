using Net.Pt;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Synchronize.Game.Lockstep.Replays
{
    /// <summary>
    /// 回放
    /// 存储所有的关键帧
    /// </summary>
    public class ReplayInfo
    {
        private static MD5 _MD5Inst = MD5.Create();

        public string Version;

        public uint MapId = 0;
        public byte[] MapVerificationCodes;

        public List<uint> InitEntityIds;

        public List<List<FrameIdxInfo>> Frames;
        private byte[] FrameIdxInfoVerificationCodes;

        public static byte[] ComputeHash(string value)
        {
            return _MD5Inst.ComputeHash(Encoding.UTF8.GetBytes(value));
        }
        public static byte[] ComputeHash(byte[] value)
        {
            return _MD5Inst.ComputeHash(value);
        }
        public override string ToString()
        {
            string mapHash = string.Join("-",MapVerificationCodes);
            string entityIds = string.Join("-",InitEntityIds);
            string frameHash = string.Join("-",FrameIdxInfoVerificationCodes);
            return $"[Version]:{Version} [MapId]:{MapId} [MapVerificationCodes]:{mapHash} [InitEntityIds]:{entityIds} [FrameIdxInfoVerificationCodes]:{frameHash}";
        }

        public static async Task<byte[]> Write(ReplayInfo info)
        {
            using (ByteBuffer buffer = new ByteBuffer())
            {
                buffer.WriteString(info.Version);
                buffer.WriteUInt32(info.MapId);
                buffer.WriteBytes(info.MapVerificationCodes);

                int initEntityIdsCount = info.InitEntityIds.Count;
                buffer.WriteInt32(initEntityIdsCount);
                for (int k = 0; k < initEntityIdsCount; ++k)
                    buffer.WriteUInt32(info.InitEntityIds[k]);

                using (ByteBuffer frameBuffer = new ByteBuffer())
                {
                    int count = info.Frames.Count;
                    frameBuffer.WriteInt32(count);
                    for(int i=0;i< count; ++i)
                    {
                        List<FrameIdxInfo> frames = info.Frames[i];
                        int frameCount = frames.Count;
                        frameBuffer.WriteInt32(frameCount);
                        for(int j = 0; j < frameCount; ++j)
                        {
                            FrameIdxInfo frameInfo = frames[j];
                            frameBuffer.WriteBytes(FrameIdxInfo.Write(frameInfo));
                        }
                    }
                    byte[] frameRawBuffer = frameBuffer.Getbuffer();
                    byte[] frameVerificationCodes = _MD5Inst.ComputeHash(frameRawBuffer);

                    buffer.WriteBytes(frameVerificationCodes);
                    buffer.WriteBytes(await SevenZip.Helper.CompressBytesAsync(frameRawBuffer));
                }


                return buffer.Getbuffer();
            }
        }
        public static async Task<ReplayInfo> Read(byte[] bytes)
        {
            using (ByteBuffer buffer = new ByteBuffer(bytes))
            {
                ReplayInfo info = new ReplayInfo();
                info.Version = buffer.ReadString();
                info.MapId = buffer.ReadUInt32();
                info.MapVerificationCodes = buffer.ReadBytes();
                int initEntityIdsCount = buffer.ReadInt32();
                info.InitEntityIds = new List<uint>();
                for (int k = 0; k < initEntityIdsCount; ++k)
                    info.InitEntityIds.Add(buffer.ReadUInt32());

                info.FrameIdxInfoVerificationCodes = buffer.ReadBytes();

                using (ByteBuffer frameBuffer = new ByteBuffer(await SevenZip.Helper.DecompressBytesAsync(buffer.ReadBytes())))
                {
                    int count = frameBuffer.ReadInt32();
                    info.Frames = new List<List<FrameIdxInfo>>();
                    for (int i = 0; i < count; ++i)
                    {
                        int frameCount = frameBuffer.ReadInt32();
                        List<FrameIdxInfo> frames = new List<FrameIdxInfo>();
                        info.Frames.Add(frames);
                        for (int j = 0; j < frameCount; ++j)
                        {
                            frames.Add(FrameIdxInfo.Read(frameBuffer.ReadBytes()));
                        }
                    }
                }
                return info;
            }
        }
    }
}
