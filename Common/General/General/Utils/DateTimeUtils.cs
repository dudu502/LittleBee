using System;
using System.Collections.Generic;
using System.Text;

namespace General.Utils
{
    public class DateTimeUtils
    {
        public static DateTime DateTime1970 = new DateTime(1970, 1, 1).ToLocalTime();

        /// <summary>
        /// 获取从 1970-01-01 到现在的秒数。
        /// </summary>
        /// <returns></returns>
        public static long GetTimeStamp()
        {
            return (long)(DateTime.Now.ToLocalTime() - DateTime1970).TotalSeconds;
        }

        /// <summary>
        /// 计算 1970-01-01 到指定 <see cref="DateTime"/> 的秒数。
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static long GetTimeStamp(DateTime dateTime)
        {
            return (long)(dateTime.ToLocalTime() - DateTime1970).TotalSeconds;
        }
    }
}
