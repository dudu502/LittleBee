using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using NetServiceImpl;
using NetServiceImpl.OnlineMode.Gate;
using Proxy;
using Localization;
using Misc;

public class RoomItemRenderer : DynamicInfinityItem, ILanguageApplicable
{
    public Image m_ImageMap;
    public Text m_TxtRoomName;
    public Button m_Btn;
    // Use this for initialization
    void Start()
    {
        ApplyLocalizedLanguage();
        m_Btn.onClick.AddListener(() =>
        {
            if (OnSelectHandler != null)
                OnSelectHandler(this);
        });
    }

    protected override void OnRenderer()
    {
        base.OnRenderer();
        var ptRoom = GetData<Net.Pt.PtRoom>();
        var ptPlayer = ptRoom.Players.Find((p) => ptRoom.RoomOwnerUserId == p.UserId);
        if (ptPlayer != null)
        {
            if(ptRoom.MaxPlayerCount>0)
                m_TxtRoomName.text = ptPlayer.NickName + $" ({ptRoom.Players.Count}/{ptRoom.MaxPlayerCount})";
            else
                m_TxtRoomName.text = ptPlayer.NickName + $" (...)";
        }

    }

    public void ApplyLocalizedLanguage()
    {
        m_Btn.SetButtonText(Language.GetText(10));
    }
}
