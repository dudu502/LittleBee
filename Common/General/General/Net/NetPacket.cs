using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Net
{
    /// <summary>
    /// 对于binary数据进行发送前封包，接收时拆包
    /// </summary>
    public class NetPacket
    {
        public const int ByteBufferLength = 256;
        public const int ByteContentLength = ByteBufferLength - 2;

        private byte[] m_ResidualData = new byte[0];
        private byte[] m_PackageData = new byte[0];
        public void ResetPackageData()
        {
            m_PackageData = new byte[0];
        }
        /// <summary>
        /// 拆包
        /// </summary>
        /// <param name="Source"></param>
        /// <returns></returns>
        public void UnPackageSegment(byte[] Source, Action<byte[]> callBack)
        {
            m_ResidualData = m_ResidualData.Concat(Source).ToArray();
            for (; ; )
            {
                byte idx = m_ResidualData[0];
                byte count = m_ResidualData[1];
                byte[] newResidual = new byte[m_ResidualData.Length - ByteBufferLength];

                byte[] seg = new byte[ByteContentLength];
                Buffer.BlockCopy(m_ResidualData, 2, seg, 0, ByteContentLength);
                Buffer.BlockCopy(m_ResidualData, ByteBufferLength, newResidual, 0, newResidual.Length);
                m_PackageData = m_PackageData.Concat(seg).ToArray();
                m_ResidualData = newResidual;

                if (idx == count - 1)
                {
                    callBack(m_PackageData);
                    ResetPackageData();
                }
                if (m_ResidualData.Length == 0)
                {
                    return;
                }
            }
        }
        /// <summary>
        /// 封包
        /// </summary>
        /// <param name="TotalBytes"></param>
        /// <returns></returns>
        public byte[] PackageSegment(byte[] TotalBytes)
        {
            int srcOffset = 0;
            int totalBytesLength = TotalBytes.Length;
            int maxSegmentCount = (int)Math.Ceiling(1f * totalBytesLength / ByteContentLength);
            if (maxSegmentCount > byte.MaxValue) throw new Exception("TotalBytes too Large.");
            byte[] result = new byte[0];
            for (; ; )
            {
                byte[] dstBytes = new byte[ByteBufferLength];
                dstBytes[0] = Convert.ToByte(srcOffset / ByteContentLength);
                dstBytes[1] = Convert.ToByte(maxSegmentCount);
                int copyLength = srcOffset + ByteContentLength > totalBytesLength ?
                    totalBytesLength % ByteContentLength : ByteContentLength;
                Buffer.BlockCopy(TotalBytes, srcOffset, dstBytes, 2, copyLength);
                srcOffset += ByteContentLength;
                result = result.Concat(dstBytes).ToArray();
                if (srcOffset >= totalBytesLength) break;
            }
            return result;
        }
    }
}
