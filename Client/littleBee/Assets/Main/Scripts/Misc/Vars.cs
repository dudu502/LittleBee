using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Synchronize.Game.Lockstep.Misc
{
    public enum ResourceType : byte
    {
        Prefab = 1,
        Texture = 2,
    }

    public enum FsmType
    {
        ComputerPlayer,
    }

    public enum AttackType
    { 
        Common,//普通
        Laser,//激光
        Acid,//酸
        Biochemical,//生化
    }
    public enum DefenceType
    {
        Lightweight,//轻甲
        Middleweight,//中甲
        Heavyweight,//重甲
        Urbanweight,//城甲
    }

}
