using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using NetServiceImpl;
using Synchronize.Game.Lockstep.Localization;
using Synchronize.Game.Lockstep.Misc;
using TMPro;
using Net.Pt;
using Synchronize.Game.Lockstep.Proxy;
using Synchronize.Game.Lockstep.Gate;

namespace Synchronize.Game.Lockstep.UI
{
    public class RoomPlayerItem : DynamicInfinityItem
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
        public TMP_Text m_TxtName;
        public Button m_BtnColor;
        public Button m_BtnTeam;
        int teamId = 1;
        // Use this for initialization
        void Start()
        {
            m_BtnColor.onClick.AddListener(OnChangeColor);
            m_BtnTeam.onClick.AddListener(OnChangeTeam);
        }
        void OnChangeColor()
        {
            print("change color");
            var player = GetData<PtRoomPlayer>();

            DataProxy.Get<GateServiceProxy>().RequestUpdatePlayerColor(DataProxy.Get<GateServiceProxy>().SelfRoom.RoomId, player.UserId);
        }

        void OnChangeTeam()
        {
            print("change team");
            var player = GetData<PtRoomPlayer>();
            DataProxy.Get<GateServiceProxy>().RequestUpdatePlayerTeam(DataProxy.Get<GateServiceProxy>().SelfRoom.RoomId, player.UserId, (byte)++teamId);
        }

        protected override void OnRenderer()
        {
            base.OnRenderer();
            var player = GetData<PtRoomPlayer>();
            if (player.HasNickName())
                m_TxtName.text = player.NickName;
            else
                m_TxtName.text = "OPEN";
            if (player.HasColor())
                m_BtnColor.GetComponent<Image>().color = Colors[player.Color];
            else
                m_BtnColor.GetComponent<Image>().color = Color.black;
        }
    }
}