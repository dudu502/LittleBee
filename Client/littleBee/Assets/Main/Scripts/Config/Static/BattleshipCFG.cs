using Synchronize.Game.Lockstep.Config.Static.Interface;

namespace Synchronize.Game.Lockstep.Config.Static
{
    public class BattleshipCFG: ICFG,IObjectCFG, ICombativeCFG
    {
        public int ConfigId { set; get; }
        public string Name { set; get; }
        public string Desc { set; get; }
        public int ResKey { set; get; }
        public int Mass { set; get; }
        public int Diameter { set; get; }
        public int HaloType { set; get; }
        public int HaloEffectRadius { set; get; }
        public int AttackType { set; get; }
        public int DefenceType { set; get; }
        public int Attack { set; get; }
        public int Defence { set; get; }

        #region Self
        public int Speed { set; get; }
        #endregion
    }
}
