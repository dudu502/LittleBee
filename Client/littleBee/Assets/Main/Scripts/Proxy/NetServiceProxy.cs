using Synchronize.Game.Lockstep.Logger;

namespace Synchronize.Game.Lockstep.Proxy
{
    public class NetServiceProxy:DataProxy
    {
        protected ILogger logger;
        protected override void OnInit()
        {
            base.OnInit();
            logger = new UnityEnvLogger("Client");
        }

    }
}
