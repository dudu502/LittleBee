﻿using UnityEngine;
using System.Collections;
using Managers.UI;
using Localization;
using Misc;
using UnityEngine.UI;
using Proxy;
using Net.Pt;
using Managers;
using Managers.Config;

public class RoomPanel : UIView, ILanguageApplicable
{
    public enum EventType
    {
        ClosePanel,
    }
    public Text m_TxtTitle;
    public Button m_BtnBack;
    public Button m_BtnLaunchGame;
    public DynamicInfinityListRenderer m_DlPlayerListRender;

    public Button m_BtnPrevMap;
    public Button m_BtnDone;
    public Button m_BtnNextMap;
    public Text m_TxtMapInfo;
  
    private ListIterator<Config.Static.MapIdCFG> m_IterMapIds = new ListIterator<Config.Static.MapIdCFG>(null);
    // Use this for initialization

    void Awake()
    {

    }
    void Start()
    {

    }
    public override void OnInit()
    {
        base.OnInit();
        m_BtnBack.onClick.AddListener(OnClickBack);
        m_BtnLaunchGame.onClick.AddListener(OnClickLaunchGame);
        m_DlPlayerListRender.InitRendererList(OnSelectPlayerItem, null);

        m_BtnPrevMap.onClick.AddListener(OnClickChangeMapPrev);
        m_BtnNextMap.onClick.AddListener(OnClickChangeMapNext);
        m_BtnDone.onClick.AddListener(OnClickDone);
        Evt.EventMgr<EvtGate, PtRoom>.AddListener(EvtGate.UpdateCreateRoom, OnCreateRoomSuccess);
        Evt.EventMgr<EvtGate, PtRoom>.AddListener(EvtGate.UpdateCurrentRoom, OnUpdateRoom);

        Evt.EventMgr<EventType, object>.AddListener(EventType.ClosePanel, OnClosePanel);
    }
    void OnClosePanel(object obj)
    {
        ModuleManager.GetModule<UIModule>().Pop(Layer.Bottom, this);
    }
    public override void OnClose()
    {
        base.OnClose();
        m_BtnBack.onClick.RemoveAllListeners();
        m_BtnLaunchGame.onClick.RemoveAllListeners();
        m_BtnPrevMap.onClick.RemoveAllListeners();
        m_BtnNextMap.onClick.RemoveAllListeners();
        m_BtnDone.onClick.RemoveAllListeners();
        Evt.EventMgr<EvtGate, PtRoom>.RemoveListener(EvtGate.UpdateCreateRoom, OnCreateRoomSuccess);
        Evt.EventMgr<EvtGate, PtRoom>.RemoveListener(EvtGate.UpdateCurrentRoom, OnUpdateRoom);
        Evt.EventMgr<EventType, object>.RemoveListener(EventType.ClosePanel, OnClosePanel);
    }
    void OnClickDone()
    {
        RequestUpdateRoomMap();
    }
    void RequestUpdateRoomMap()
    {
        if (ModuleManager.GetModule<NetworkGateModule>().SelfRoom != null)
        {
            ModuleManager.GetModule<NetworkGateModule>().RequestUpdateMap(ModuleManager.GetModule<NetworkGateModule>().SelfRoom.RoomId, (uint)m_IterMapIds.GetCurrent().ConfigId, m_IterMapIds.GetCurrent().PlayerCount);
        }
    }
    void OnClickChangeMapPrev()
    {
        if (m_IterMapIds.HasPrev())
        {
            var mapCfg = m_IterMapIds.GetPrev();
            RefreshMapPanel(mapCfg);
        }
        OnClickDone();
    }
    void OnClickChangeMapNext()
    {
        if (m_IterMapIds.HasNext())
        {
            var mapCfg = m_IterMapIds.GetNext();
            RefreshMapPanel(mapCfg);
        }
        OnClickDone();
    }
    void OnUpdateRoom(PtRoom room)
    {
        RefreshData();
        //Debug.LogError("OnUpdateRoom");
    }
    void OnCreateRoomSuccess(PtRoom room)
    {
        RequestUpdateRoomMap();
    }

    private void RefreshData()
    {
        var selfRoom = ModuleManager.GetModule<NetworkGateModule>().SelfRoom;
        if (selfRoom != null)
        {
            var mapCfg = ModuleManager.GetModule<ConfigModule>().GetConfig<Config.Static.MapIdCFG>((int)selfRoom.MapId);
            m_IterMapIds.SetCurrentIndex(m_IterMapIds.GetSource().IndexOf(mapCfg));
            RefreshPlayerList(selfRoom, mapCfg);
            RefreshMapPanel(mapCfg);
        }
    }

    void OnClickBack()
    {    
        RequestLeaveRoom();
        ModuleManager.GetModule<UIModule>().Pop(Layer.Bottom);    
    }
    void OnClickLaunchGame()
    {
        if (ModuleManager.GetModule<NetworkGateModule>().SelfRoom != null)
        {
            ModuleManager.GetModule<NetworkGateModule>().RequestLaunchGame();
        }
    }
    void OnSelectPlayerItem(DynamicInfinityItem selected)
    {
        ModuleManager.GetModule<NetworkGateModule>().RequestLaunchGame();
    }
    public override void OnShow(object paramObject)
    {
        base.OnShow(paramObject);
        m_IterMapIds.SetSource(ModuleManager.GetModule<ConfigModule>().GetConfigs<Config.Static.MapIdCFG>());
        ApplyLocalizedLanguage();
        RefreshData();
        RequestUpdateRoomMap();
    }

    private void RefreshMapPanel(Config.Static.MapIdCFG mapIdCFG)
    {
        m_TxtMapInfo.text = $"{mapIdCFG.ConfigId} {mapIdCFG.Name} {mapIdCFG.Desc}";
    }


    void RefreshPlayerList(PtRoom selfRoom, Config.Static.MapIdCFG mapIdCFG)
    {
        m_DlPlayerListRender.SetDataProvider(selfRoom.Players);
        bool isRoomOwner = selfRoom.RoomOwnerUserId == DataProxy.Get<UserDataProxy>().UserLoginInfo.results[0].GetId();
        m_BtnLaunchGame.gameObject.SetActive(isRoomOwner);
        m_BtnDone.gameObject.SetActive(isRoomOwner);
        m_BtnNextMap.gameObject.SetActive(isRoomOwner);
        m_BtnPrevMap.gameObject.SetActive(isRoomOwner);
    }
    void RequestLeaveRoom()
    {
        if (!DataProxy.Get<UserDataProxy>().UserLoginInfo.IsEmpty())
        {
            ModuleManager.GetModule<NetworkGateModule>().RequestLeaveRoom();
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
    public void ApplyLocalizedLanguage()
    {
        m_BtnBack.SetButtonText(Language.GetText(5));
        m_BtnLaunchGame.SetButtonText(Language.GetText(20));
        m_TxtTitle.text = Language.GetText(36);
    }
}
