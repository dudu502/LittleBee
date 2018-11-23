using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Mathematics;
namespace Components
{
    /// <summary>
    /// 定帧器组件
    /// 给定TotalCount 总数 
    ///     CountPerFrame  速度
    ///     在FrameClockSystem中计算CurrentCount>=TotalCount即达成
    /// </summary>
    public class FrameClockComponent : AbstractComponent
    {       
        /// <summary>
        /// 每一帧计数
        /// 类似于速度
        /// </summary>
        public int CountPerFrame;
        public int CurrentCount;
        public int TotalCount;
        public FrameClockComponent(int totalCount,int countPerFrame)
        {
            if (totalCount == 0 || countPerFrame==0)
                throw new ArgumentException();
            TotalCount = totalCount;          
            CountPerFrame = countPerFrame;
        }
        public void UpdateCount()
        {
            CurrentCount += CountPerFrame;
        }

        public bool IsOver() { return CurrentCount >= TotalCount; }

        public float GetRate() { return math.min(1, 1f * CurrentCount / TotalCount); }

        override public IComponent Clone()
        {
            FrameClockComponent com = new FrameClockComponent(TotalCount, CountPerFrame);
            com.CurrentCount = CurrentCount;
            com.Enable = Enable;
            com.EntityId = EntityId;
            return com;
        }
        public override string ToString()
        {
            return string.Format("[FrameClockComponent Id:{0} CurCount:{1} TotalCount:{2}]",EntityId,CurrentCount,TotalCount);
        }
        override public int GetCommand()
        {
            return 0;
        }
    }
}
