
namespace LogicFrameSync.Src.LockStep.Behaviours
{
    public interface ISimulativeBehaviour
    {
        Simulation Sim { set; get; }
        void Start();
        void Update();
        void Quit();
    }
}
