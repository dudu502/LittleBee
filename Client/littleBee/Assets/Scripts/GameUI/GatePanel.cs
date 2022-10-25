using UnityEngine;
using System.Collections;
using Managers.UI;
using Localization;
using System.Collections.Generic;
using UnityEngine.UI;
using Misc;
using Net.Pt;
using NetServiceImpl;
using Proxy;
using Managers;

public class GatePanel : UIView, ILanguageApplicable
{

    public Button m_BtnBack;
    public Text m_TxtTitle;
    public Text m_TxtServer;
    public DynamicInfinityListRenderer m_DynRoomList;
    public Button m_BtnRefreshRoom;
    public Button m_BtnCreate;

    public override void OnInit()
    {
        base.OnInit();


        Evt.EventMgr<EvtGate, object>.AddListener(EvtGate.GateServerConnected, OnGateServerConnected);
        Evt.EventMgr<EvtGate, PtRoomList>.AddListener(EvtGate.UpdateRoomList, OnUpdateRoomList);

        Evt.EventMgr<EvtGate, object>.AddListener(EvtGate.OpenRoomPanel, OnOpenRoomPanel);
        m_BtnBack.onClick.AddListener(() =>
        {
            ModuleManager.GetModule<UIModule>().Pop(Layer.Bottom);
            GameClientNetwork.Instance.CloseClient();
        });


        m_BtnRefreshRoom.onClick.AddListener(OnClickRefreshRoom);
        m_BtnCreate.onClick.AddListener(OnClickCreate);
        m_DynRoomList.InitRendererList(OnSelectDynRoomListItem, null);
    }
    
    IEnumerator _RefreshGateInfo()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            OnClickRefreshRoom();
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
        m_BtnRefreshRoom.onClick.RemoveAllListeners();
        m_BtnBack.onClick.RemoveAllListeners();
        m_BtnCreate.onClick.RemoveAllListeners();
      

        Evt.EventMgr<EvtGate, object>.RemoveListener(EvtGate.GateServerConnected, OnGateServerConnected);
        Evt.EventMgr<EvtGate, PtRoomList>.RemoveListener(EvtGate.UpdateRoomList, OnUpdateRoomList);

        Evt.EventMgr<EvtGate, object>.RemoveListener(EvtGate.OpenRoomPanel, OnOpenRoomPanel);
    }
    void OnClickCreate()
    {
        ModuleManager.GetModule<UIModule>().Push(UITypes.RoomPanel, Layer.Bottom, null);
        ModuleManager.GetModule<NetworkGateModule>().RequestCreateRoom(1);
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
            DialogBox.Show(Language.GetText(22),Language.GetText(40), DialogBox.SelectType.All, selection =>
            {
                if (selection == DialogBox.SelectType.Confirm)
                {
                    ModuleManager.GetModule<NetworkGateModule>().RequestJoinRoom(ptRoom);
                }
            });
        }
        else
        {
            //在组队时刻加入房间
            ModuleManager.GetModule<NetworkGateModule>().RequestJoinRoom(ptRoom);
        }

    }
    #region NetMessageHandler
    void OnGateServerConnected(object obj)
    {
        ToastRoot.Instance.ShowToast("Connect Server Success!");
    }
    void OnUpdateRoomList(PtRoomList roomList)
    {
        m_DynRoomList.SetDataProvider(roomList.Rooms);
    }
   
    #endregion


    void ConnectToGateServer()
    {
        //Debug.LogWarning("ConnectTo GateServer");
        //ClientService.Get<GateService>().Connect2GateServer( );
    }
  
    void OnClickRefreshRoom()
    {
        //ClientService.Get<GateService>().RequestRoomList();
        ModuleManager.GetModule<NetworkGateModule>().RequestRoomList();
    }
   
    public override void OnShow(object paramObject)
    {
        base.OnShow(paramObject);
        ApplyLocalizedLanguage();
        ModuleManager.GetModule<NetworkGateModule>().ConnectToGateServer();
    }

    public void ApplyLocalizedLanguage()
    {
        m_BtnBack.SetButtonText(Language.GetText(5));
        m_BtnCreate.SetButtonText(Language.GetText(8));
        
        m_BtnRefreshRoom.SetButtonText(Language.GetText(6));

        m_TxtTitle.text = Language.GetText(1);
    }

}
