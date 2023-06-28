
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace Synchronize.Game.Lockstep.Notification
{
    public enum NotificationType
    {
        None,
        Info,
        Warning,
        Error,
    }

    public enum NotificationPriority
    {
        P1 = 1,P2 = 2,P3 = 3,P4 = 4
    }
    public enum AdditionalSpriteAnchor
    {
        LeftTop,
        RightTop,
        LeftBottom,
        RightBottom,
    }

    [CreateAssetMenu(fileName = "NotificationConfigAsset", menuName = "NotificationConfigAsset")]
    public class NotificationConfigAsset : ScriptableObject
    {
        [Serializable]
        public class NotificationItem
        {
            public NotificationType Type;
            public NotificationPriority Priority = NotificationPriority.P4;
            public bool NeedReEnqueue = true;
            [HideInInspector]
            public Sprite MainSprite;
            [HideInInspector]
            public Vector2 MainSpriteSize = new Vector2(80, 80);
            [HideInInspector]
            public Sprite AdditionSprite;
            [HideInInspector]
            public AdditionalSpriteAnchor AnchorType;
            public string TitleKey;
            [TextArea(5,10)]
            public string DescriptionKey;
            public TMPro.TextAlignmentOptions DescriptionTextAlignment = TMPro.TextAlignmentOptions.Top;
            public string CheckedOptionKey;
            public bool CheckedOptionDefaultOn;
            public NotificationOption[] Options;

            public NotificationItem Clone()
            {
                NotificationItem item = new NotificationItem();
                item.Type = Type;
                item.Priority = Priority;
                item.NeedReEnqueue = NeedReEnqueue;
                item.MainSprite = MainSprite;
                item.MainSpriteSize = MainSpriteSize;
                item.AdditionSprite = AdditionSprite;
                item.AnchorType = AnchorType;   
                item.TitleKey = TitleKey;
                item.DescriptionKey = DescriptionKey;
                item.DescriptionTextAlignment = DescriptionTextAlignment;
                item.Options = Options;
                item.CheckedOptionDefaultOn = CheckedOptionDefaultOn;
                item.CheckedOptionKey = CheckedOptionKey;
                return item;
            }
        }

        
        public List<NotificationItem> NotificationItems;
    }
}
