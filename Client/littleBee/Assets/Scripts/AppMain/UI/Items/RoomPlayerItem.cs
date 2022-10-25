using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Localization;
using NetServiceImpl;
using Misc;
using Managers;

public class RoomPlayerItem : DynamicInfinityItem, ILanguageApplicable
{
    static Color[] Colors = new Color[] {
        Color.Lerp(Color.red,Color.green,0.4f),
        Color.Lerp(Color.red,Color.blue,0.4f),
        Color.Lerp(Color.red,Color.yellow,0.4f),
        Color.Lerp(Color.red,Color.grey,0.4f),
        Color.Lerp(Color.red,Color.cyan,0.4f),
        Color.Lerp(Color.blue,Color.green,0.4f),
        Color.Lerp(Color.yellow,Color.blue,0.4f),
        Color.Lerp(Color.blue,Color.yellow,0.4f),
        Color.Lerp(Color.red,Color.green,0.7f),
        Color.Lerp(Color.red,Color.blue,0.7f),
        Color.Lerp(Color.red,Color.yellow,0.7f),
        Color.Lerp(Color.red,Color.grey,0.7f),
        Color.Lerp(Color.red,Color.cyan,0.7f),
        Color.Lerp(Color.blue,Color.green,0.7f),
        Color.Lerp(Color.yellow,Color.blue,0.7f),
        Color.Lerp(Color.blue,Color.yellow,0.7f),
    };
    public Text m_TxtName;
    public Button m_BtnColor;
    public Button m_BtnTeam;
    int teamId=1;
    // Use this for initialization
    void Start()
    {
        m_BtnColor.onClick.AddListener(OnChangeColor);
        m_BtnTeam.onClick.AddListener(OnChangeTeam);
        ApplyLocalizedLanguage();
    }
    void OnChangeColor()
    {
        var player = GetData<Net.Pt.PtRoomPlayer>();
        
        ModuleManager.GetModule<NetworkGateModule>().RequestUpdatePlayerColor(ModuleManager.GetModule<NetworkGateModule>().SelfRoom.RoomId, player.UserId);
    }

    void OnChangeTeam()
    {
        var player = GetData<Net.Pt.PtRoomPlayer>();
        ModuleManager.GetModule<NetworkGateModule>().RequestUpdatePlayerTeam(ModuleManager.GetModule<NetworkGateModule>().SelfRoom.RoomId, player.UserId,(byte)++teamId);
    }

    protected override void OnRenderer()
    {
        base.OnRenderer();
        var player = GetData<Net.Pt.PtRoomPlayer>();
        if (player.HasNickName())
            m_TxtName.text = player.NickName;
        else
            m_TxtName.text = "OPEN";
        if (player.HasColor())
            m_BtnColor.GetComponent<Image>().color = Colors[player.Color];
        else
            m_BtnColor.GetComponent<Image>().color = Color.black;
    }
    // Update is called once per frame
    void Update()
    {

    }

    public void ApplyLocalizedLanguage()
    {
        m_BtnTeam.SetButtonText(Language.GetText(12));
    }
}
