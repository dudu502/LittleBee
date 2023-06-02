using UnityEngine;
using NetServiceImpl;
using NetServiceImpl.OnlineMode.Gate;
using NetServiceImpl.OnlineMode.Room;
using UI.Data;

using Synchronize.Game.Lockstep.Misc;
using Synchronize.Game.Lockstep.Managers;
using Synchronize.Game.Lockstep;
using Synchronize.Game.Lockstep.Localization;
using Synchronize.Game.Lockstep.Managers.UI;
namespace Synchronize.Game.Lockstep
{
    public class App : MonoBehaviour
    {
        public LanguageType LanguageSetting = LanguageType.English;
        public static App Instance
        {
            get;
            private set;
        }
        [SerializeField] private UIModule m_UIModule;
        [SerializeField] private PoolModule m_PoolModule;
        [SerializeField] private EntitySpawnModule m_EntitySpawnModule;
        [SerializeField] private GameContentRootModule m_GameContentRootModule;
        void InitApplicationSettings()
        {
            BattleEntryPoint.PersistentDataPath = Application.persistentDataPath;
            UserSettingMgr.Init();
            Application.targetFrameRate = 40;
            Application.runInBackground = true;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
        }
        void InitProxySettings()
        {
            Proxy.DataProxy.Init<Proxy.PlayerPrefabsProxy>(new Proxy.PlayerPrefabsProxy());
            Proxy.DataProxy.Init<Proxy.UserDataProxy>(new Proxy.UserDataProxy());
        }
        void InitServiceSettings()
        {
            ClientService.Init(new GateService());
            ClientService.Init(new RoomServices());
        }
        void InitManagerSettings()
        {
            if (m_UIModule != null)
                ModuleManager.Add(m_UIModule);
            ModuleManager.Add(new ConfigModule());
            if (m_PoolModule != null)
                ModuleManager.Add(m_PoolModule);
            if (m_EntitySpawnModule != null)
                ModuleManager.Add(m_EntitySpawnModule);
            if (m_GameContentRootModule != null)
                ModuleManager.Add(m_GameContentRootModule);
            ObjectPool.Common.PoolMgr.Init();

        }
        void Awake()
        {
            Instance = this;
            InitApplicationSettings();
            InitProxySettings();
            InitServiceSettings();
            InitManagerSettings();

            DialogBox.Init();
            Handler.Init();
            Language.SetLanguge(LanguageSetting);
        }

        void Start()
        {
            if (m_UIModule != null)
                m_UIModule.Push(UITypes.MainPanel, Layer.Bottom, null);
        }

        void Update()
        {
            Handler.Update();
            ObjectPool.Common.PoolMgr.Update();
        }
    }
}