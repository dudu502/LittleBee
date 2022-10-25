using UnityEngine;
using NetServiceImpl;
using UI.Data;
using Managers;
using Managers.UI;
using Managers.Config;
using System.IO;
using LogicFrameSync.Src.LockStep;
using System;
using Src.Replays;
using Localization;
using Misc;

public class App : MonoBehaviour
{
    public LanguageType LanguageSetting = LanguageType.English;
    public static App Instance {
        get;
        private set;
    }
    [SerializeField] private UIModule m_UIModule;
    [SerializeField] private PoolModule m_PoolModule;
    [SerializeField] private EntitySpawnModule m_EntitySpawnModule;
    [SerializeField] private GameContentRootModule m_GameContentRootModule;
    void InitApplicationSettings()
    {
        LogicFrameSync.Src.LockStep.BattleEntryPoint.PersistentDataPath = Application.persistentDataPath;
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
        ModuleManager.Add(new NetworkGateModule());
        ModuleManager.Add(new NetworkRoomModule());
    }
    void InitManagerSettings()
    {
        if(m_UIModule!=null)
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
        Misc.Handler.Init();
        Language.SetLanguge(LanguageSetting);
    }

    void Start()
    {
        if(m_UIModule!=null)
            m_UIModule.Push(UITypes.MainPanel, Layer.Bottom, null);
    }

    void Update()
    {
        Misc.Handler.Update();
        ObjectPool.Common.PoolMgr.Update();
    }
}
