

namespace Synchronize.Game.Lockstep.Behaviours
{
    /// <summary>
    /// 模拟器行为接口
    /// </summary>
    public interface ISimulativeBehaviour
    {
        Simulation Sim { set; get; }
        void Start();
        void Update();
        void Quit();
    }
}
