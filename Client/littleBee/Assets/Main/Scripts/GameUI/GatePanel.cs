using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Net.Pt;
using Synchronize.Game.Lockstep.Managers.UI;
using Synchronize.Game.Lockstep.Localization;
using Synchronize.Game.Lockstep.Managers;
using Synchronize.Game.Lockstep.Misc;
using Synchronize.Game.Lockstep.Net;
using Synchronize.Game.Lockstep.Notification;
using TMPro;
using Synchronize.Game.Lockstep.Proxy;
using Synchronize.Game.Lockstep.Gate;

namespace Synchronize.Game.Lockstep.UI
{
    public class GatePanel : UIView
    {
        private List<GateAddressVO> m_Hosts;
        public Button m_BtnBack;
        public TMP_Text m_TxtTitle;
        public ToggleGroup m_ToggleGroup;
        public List<Toggle> m_Toggles;
        public DynamicInfinityListRenderer m_DynRoomList;
        public Button m_BtnRefreshGate;
        public Button m_BtnRefreshRoom;
        public Button m_BtnJoin;
        public Button m_BtnCreate;
        private Toggle currentSelectedToggle;
        public override void OnInit()
        {
            base.OnInit();

            foreach (Toggle toggle in m_Toggles)
            {
                toggle.isOn = false;
                toggle.onValueChanged.AddListener((select) => OnToggleSelect(toggle, select));
                toggle.gameObject.SetActive(false);
            }
            Evt.EventMgr<EvtGate, List<GateAddressVO>>.AddListener(EvtGate.UpdateWANGateServerList, OnWANGateServerList);
            Evt.EventMgr<EvtGate, List<GateAddressVO>>.AddListener(EvtGate.UpdateLANGateServerList, OnLANGateServerList);
            Evt.EventMgr<EvtGate, object>.AddListener(EvtGate.GateServerConnected, OnGateServerConnected);
            Evt.EventMgr<EvtGate, PtRoomList>.AddListener(EvtGate.UpdateRoomList, OnUpdateRoomList);

            Evt.EventMgr<EvtGate, object>.AddListener(EvtGate.OpenRoomPanel, OnOpenRoomPanel);
            m_BtnBack.onClick.AddListener(() =>
            {
                ModuleManager.GetModule<UIModule>().Pop(Layer.Bottom);
                foreach (Toggle toggle in m_Toggles)
                {
                    toggle.isOn = false;
                    toggle.gameObject.SetActive(false);
                }
                currentSelectedToggle = null;
                GameClientNetwork.Instance.CloseClient();
            });

            m_BtnRefreshGate.onClick.AddListener(OnClickRefreshGate);
            m_BtnRefreshRoom.onClick.AddListener(OnClickRefreshRoom);
            m_BtnCreate.onClick.AddListener(OnClickCreate);
            m_DynRoomList.InitRendererList(OnSelectDynRoomListItem, null);
            m_DynRoomList.SetDataProvider(new List<PtRoom>());
        }
        IEnumerator _RefreshGateInfo()
        {
            while (true)
            {
                OnClickRefreshRoom();
                yield return new WaitForSeconds(1);
            }
        }
        void OnEnable()
        {
            StartCoroutine(_RefreshGateInfo());
        }
        void OnDisable()
        {
            StopCoroutine(_RefreshGateInfo());
        }
        public override void OnResume()
        {
            base.OnResume();
        }
        public override void OnClose()
        {
            base.OnClose();
            m_BtnRefreshGate.onClick.RemoveAllListeners();
            m_BtnRefreshRoom.onClick.RemoveAllListeners();
            m_BtnBack.onClick.RemoveAllListeners();
            m_BtnCreate.onClick.RemoveAllListeners();
            foreach (Toggle toggle in m_Toggles)
            {
                toggle.onValueChanged.RemoveAllListeners();// ((select) => OnToggleSelect(toggle, select));            
            }
            Evt.EventMgr<EvtGate, List<GateAddressVO>>.RemoveListener(EvtGate.UpdateWANGateServerList, OnWANGateServerList);
            Evt.EventMgr<EvtGate, List<GateAddressVO>>.RemoveListener(EvtGate.UpdateLANGateServerList, OnLANGateServerList);
            Evt.EventMgr<EvtGate, object>.RemoveListener(EvtGate.GateServerConnected, OnGateServerConnected);
            Evt.EventMgr<EvtGate, PtRoomList>.RemoveListener(EvtGate.UpdateRoomList, OnUpdateRoomList);

            Evt.EventMgr<EvtGate, object>.RemoveListener(EvtGate.OpenRoomPanel, OnOpenRoomPanel);
        }
        void OnClickCreate()
        {
            ModuleManager.GetModule<UIModule>().Push(UITypes.RoomPanel, Layer.Bottom, null);
            DataProxy.Get<GateServiceProxy>().RequestCreateRoom(1);
        }
        void OnOpenRoomPanel(object obj)
        {
            ModuleManager.GetModule<UIModule>().Push(UITypes.RoomPanel, Layer.Bottom, null);
        }

        /// <summary>
        /// 房间加入按钮的事件
        /// </summary>
        /// <param name="selectedItem"></param>
        void OnSelectDynRoomListItem(DynamicInfinityItem selectedItem)
        {
            var ptRoom = selectedItem.GetData<PtRoom>();
            string selfName = DataProxy.Get<UserDataProxy>().GetUserName();
            if (ptRoom.Players.Exists(p => p.NickName == selfName))
            {
                //以断开 重新进入的方式加入房间，会以不同的方式进入游戏
                NotificationManager.Instance.Show(NotificationType.Warning, option =>
                {
                    if(option == NotificationOption.OK)
                        DataProxy.Get<GateServiceProxy>().RequestJoinRoom(ptRoom);
                }, item =>
                {
                    item.TitleKey = string.Format(item.TitleKey, Localization.Localization.GetTranslation("Tips"));
                    item.DescriptionKey = string.Format(item.DescriptionKey, Localization.Localization.GetTranslation("Do you need to synchronize?"));
                });
            }
            else
            {
                //在组队时刻加入房间
                DataProxy.Get<GateServiceProxy>().RequestJoinRoom(ptRoom);
            }
        }
        #region NetMessageHandler
        void OnGateServerConnected(object obj)
        {
            print("connected gate");
        }
        void OnUpdateRoomList(PtRoomList roomList)
        {
            m_DynRoomList.SetDataProvider(roomList.Rooms);
        }
        void OnWANGateServerList(List<GateAddressVO> hosts)
        {
            UpdateServerList(hosts);
        }
        void OnLANGateServerList(List<GateAddressVO> hosts)
        {
            UpdateServerList(hosts);
        }
        #endregion
        void UpdateServerList(List<GateAddressVO> hosts)
        {
            m_Hosts = hosts;
            for (int i = 0; i < m_Toggles.Count; ++i)
            {
                if (i < m_Hosts.Count)
                {
                    m_Toggles[i].gameObject.SetActive(true);
                    m_Toggles[i].SetToggleText(m_Hosts[i].ConnectKey);
                }
                else
                {
                    m_Toggles[i].gameObject.SetActive(false);
                }
            }
            if (m_Hosts.Count > 0)
            {
                m_Toggles[0].isOn = false;
                m_Toggles[0].isOn = true;
                //OnClickRefreshGate();
            }

        }

        void ConnectToGateServer()
        {
            Debug.LogWarning("ConnectTo GateServer");
            DataProxy.Get<GateServiceProxy>().Connect2GateServer();
        }
        void OnClickRefreshGate()
        {
            DataProxy.Get<GateServiceProxy>().RefreshLanGates(9030);
        }
        void OnClickRefreshRoom()
        {
            DataProxy.Get<GateServiceProxy>().RequestRoomList();
        }
        void OnToggleSelect(Toggle toggle, bool select)
        {
            Debug.LogWarning("OnToggleSelect " + select);
            if (select && currentSelectedToggle != toggle)
            {
                currentSelectedToggle = toggle;
                toggle.SetToggleTextColor(Color.black);
                foreach (var t in m_Toggles)
                {
                    if (t != toggle)
                    {
                        t.SetToggleTextColor(Color.white);
                    }
                }
                int selectIndex = m_Toggles.IndexOf(toggle);
                DataProxy.Get<GateServiceProxy>().UpdateCurrentGateAddress(m_Hosts[selectIndex]);
                ConnectToGateServer();
            }
        }
        public override void OnShow(object paramObject)
        {
            base.OnShow(paramObject);
            //wan mode
            GateAddressVO gateAddressVO = DataProxy.Get<UserDataProxy>().GetGateAddressVO();
            if (gateAddressVO != null)
            {
                Evt.EventMgr<EvtGate, List<GateAddressVO>>.TriggerEvent(
                   EvtGate.UpdateWANGateServerList, new List<GateAddressVO>() { gateAddressVO });
            }

        }
    }
}