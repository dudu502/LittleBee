using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Synchronize.Game.Lockstep.Localization;
using Synchronize.Game.Lockstep.Misc;
using Net.Pt;

namespace Synchronize.Game.Lockstep.UI
{
    public class RoomItemRenderer : DynamicInfinityItem
    {
        public Image m_ImageMap;
        public TMPro.TMP_Text m_TxtRoomName;
        public Button m_Btn;
        // Use this for initialization
        void Start()
        {
            m_Btn.onClick.AddListener(() =>
            {
                if (OnSelectHandler != null)
                    OnSelectHandler(this);
            });
        }

        protected override void OnRenderer()
        {
            base.OnRenderer();
            var ptRoom = GetData<PtRoom>();
            var ptPlayer = ptRoom.Players.Find((p) => ptRoom.RoomOwnerUserId == p.UserId);
            if (ptPlayer != null)
            {
                if (ptRoom.MaxPlayerCount > 0)
                    m_TxtRoomName.text = ptPlayer.NickName + $" ({ptRoom.Players.Count}/{ptRoom.MaxPlayerCount})";
                else
                    m_TxtRoomName.text = ptPlayer.NickName + $" (...)";
            }
        }
    }
}