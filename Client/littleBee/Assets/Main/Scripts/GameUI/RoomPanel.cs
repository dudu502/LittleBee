
using UnityEngine.UI;
using Net.Pt;
using Synchronize.Game.Lockstep.Managers.UI;
using Synchronize.Game.Lockstep.Localization;
using Synchronize.Game.Lockstep.Managers;
using Synchronize.Game.Lockstep.Misc;
using System.Collections.Generic;
using Synchronize.Game.Lockstep.Proxy;
using Synchronize.Game.Lockstep.Gate;

namespace Synchronize.Game.Lockstep.UI
{
    public class RoomPanel : UIView
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
        public override void OnInit()
        {
            base.OnInit();
            m_BtnBack.onClick.AddListener(OnClickBack);
            m_BtnLaunchGame.onClick.AddListener(OnClickLaunchGame);
            m_DlPlayerListRender.InitRendererList(OnSelectPlayerItem, null);
            m_DlPlayerListRender.SetDataProvider(new List<PtRoomPlayer>());
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
            if (DataProxy.Get<GateServiceProxy>().SelfRoom != null)
            {
                DataProxy.Get<GateServiceProxy>().RequestUpdateMap(DataProxy.Get<GateServiceProxy>().SelfRoom.RoomId, (uint)m_IterMapIds.GetCurrent().ConfigId, m_IterMapIds.GetCurrent().PlayerCount);
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
        }
        void OnCreateRoomSuccess(PtRoom room)
        {
            RequestUpdateRoomMap();
        }

        private void RefreshData()
        {
            var selfRoom = DataProxy.Get<GateServiceProxy>().SelfRoom;
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
            if (DataProxy.Get<GateServiceProxy>().SelfRoom != null)
            {
                DataProxy.Get<GateServiceProxy>().RequestLaunchGame();
            }
        }
        void OnSelectPlayerItem(DynamicInfinityItem selected)
        {
            DataProxy.Get<GateServiceProxy>().RequestLaunchGame();
        }
        public override void OnShow(object paramObject)
        {
            base.OnShow(paramObject);
            m_IterMapIds.SetSource(ModuleManager.GetModule<ConfigModule>().GetConfigs<Config.Static.MapIdCFG>());
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
                DataProxy.Get<GateServiceProxy>().RequestLeaveRoom();
            }
        }
    }
}