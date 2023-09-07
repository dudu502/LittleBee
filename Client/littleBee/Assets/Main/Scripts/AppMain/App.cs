using UnityEngine;
using Synchronize.Game.Lockstep.Misc;
using Synchronize.Game.Lockstep.Managers;
using Synchronize.Game.Lockstep;
using Synchronize.Game.Lockstep.Localization;
using Synchronize.Game.Lockstep.Managers.UI;
using Synchronize.Game.Lockstep.Notification;
using Synchronize.Game.Lockstep.Gate;
using Synchronize.Game.Lockstep.Room;

namespace Synchronize.Game.Lockstep
{
    public class App : MonoBehaviour
    {
        [SerializeField] UIModule m_UIModule;
        [SerializeField] PoolModule m_PoolModule;
        [SerializeField] EntitySpawnModule m_EntitySpawnModule;
        [SerializeField] GameContentRootModule m_GameContentRootModule;
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
        void InitServiceProxySettings()
        {
            Proxy.DataProxy.Init<GateServiceProxy>(new GateServiceProxy());
            Proxy.DataProxy.Init<RoomServiceProxy>(new RoomServiceProxy());
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
        }
        void InitLocalization()
        {
            Localization.Localization.Initialize();
            Localization.Localization.SetLanguage(Localization.Localization.Language.ChineseSimplified);
        }
        void Awake()
        {
            InitLocalization();
            InitApplicationSettings();
            InitProxySettings();
            InitServiceProxySettings();
            InitManagerSettings();
            Handler.Init();
        }

        void Start()
        {
            if (m_UIModule != null)
                m_UIModule.Push(UITypes.MainPanel, Layer.Bottom, null);
        }

        void Update()
        {
            Handler.Update();  
        }
    }
}