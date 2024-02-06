
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Synchronize.Game.Lockstep.FSM;

namespace Synchronize.Game.Lockstep.Notification
{
    public enum NotificationOption
    {
        Cancel,
        OK,
        
    }
    public class NotificationManager : MonoBehaviour
    {
        public class NotificationCallback : IComparable<NotificationCallback>
        {
            public NotificationType Type;
            public Action<NotificationOption> Callback;
            public Action<NotificationOption, bool> CallbackWithBoolean;
            public Action<NotificationConfigAsset.NotificationItem> OverrideCallback;

            public NotificationConfigAsset.NotificationItem NotificationAssetItem;
            private DateTime TriggerTime;
            public NotificationCallback(NotificationType type, Action<NotificationOption> cb, Action<NotificationOption, bool> cbwb,
                Action<NotificationConfigAsset.NotificationItem> ocb , NotificationConfigAsset.NotificationItem item)
            {
                Type = type;
                Callback = cb;
                CallbackWithBoolean = cbwb;
                OverrideCallback = ocb;
                NotificationAssetItem = item;
                TriggerTime = DateTime.Now;
            }
            public int CompareTo(NotificationCallback other)
            {
                int result = other.NotificationAssetItem.Priority.CompareTo(NotificationAssetItem.Priority);
                if (result == 0)
                    return other.TriggerTime.CompareTo(TriggerTime);
                return result;
            }
        }
        public static NotificationManager Instance { get; private set; }
        private const string FADE_IN_ANI_NAME = "NotificationFadeInClip";
        private const string FADE_OUT_ANI_NAME = "NotificationFadeOutClip";
        private Animator _fadeAnimator;
        private List<NotificationCallback> _notificationTypesInQueue;
        private NotificationCallback _currentNotification = null;
     
        [SerializeField] NotificationConfigAsset _notificationConfigAsset;

        [SerializeField] Image _mainImage;
        [SerializeField] Image _additionalImageLT;
        [SerializeField] Image _additionalImageRT;
        [SerializeField] Image _additionalImageLB;
        [SerializeField] Image _additionalImageRB;
        [SerializeField] TMPro.TMP_Text _titleText;
        [SerializeField] TMPro.TMP_Text _descriptionText;
        [SerializeField] Toggle _toggleOption;
        [SerializeField] Button _optionButtonLeft;
        [SerializeField] Button _optionButtonRight;
        [SerializeField] Button _optionButtonBottom;
        enum NotificationLifeCycle
        {
            Displaying,
            Closing,
            Closed,
        }
        private NotificationLifeCycle _notificationLifeCycle = NotificationLifeCycle.Closed;
        enum State
        {
            Idle,
            Normal,
            Interrupting,
        }

        IStateMachine< NotificationManager> _notificationFSM;

        private void Awake()
        {
            _fadeAnimator = GetComponent<Animator>();
            _notificationTypesInQueue = new List<NotificationCallback>();
            Instance = this;

            _optionButtonLeft.onClick.AddListener(OnClickLeftOption);
            _optionButtonRight.onClick.AddListener(OnClickRightOption);
            _optionButtonBottom.onClick.AddListener(OnclickBottomOption);

            var leftNav = _optionButtonLeft.navigation;
            leftNav.mode = Navigation.Mode.None;
            _optionButtonLeft.navigation = leftNav;
            var rightNav = _optionButtonRight.navigation;
            rightNav.mode = Navigation.Mode.None;
            _optionButtonRight.navigation = rightNav;
            var bottomNav = _optionButtonBottom.navigation;
            bottomNav.mode = Navigation.Mode.None;
            _optionButtonBottom.navigation = bottomNav;
            _notificationFSM = new StateMachine< NotificationManager>(this);
        }
        private void Start()
        {
            _notificationFSM
                .State(State.Idle)
                    .Transition(so => so._notificationTypesInQueue.Count > 0).To(State.Normal).End()
                .End()
                .State(State.Normal)
                    .Enter(so =>
                    {
                        so.ShowImpl(so._notificationTypesInQueue.LastOrDefault());
                        so._notificationTypesInQueue.RemoveAt(so._notificationTypesInQueue.Count - 1);
                    })
                    .Transition(so => so._currentNotification == null).To(State.Idle).End()
                    .Transition(so =>
                    {
                        var last = so._notificationTypesInQueue.LastOrDefault();
                        if (last != null)
                            return so._currentNotification.CompareTo(last) < 0;
                        return false;
                    }).To(State.Interrupting).End()
                .End()
                .State(State.Interrupting)
                    .Enter(so =>
                    {
                        so.Continue(so._currentNotification.Type);
                        if (so._currentNotification.NotificationAssetItem.NeedReEnqueue)
                            so.EnqueueNotificationCallback(so._currentNotification);
                    })
                    .Transition(so => so._currentNotification == null).To(State.Idle).End()
                .End()
                .SetDefault(State.Idle).Build();
        }
        private NotificationConfigAsset.NotificationItem GetNotificationItem(NotificationType nType)
        {
            foreach (var item in _notificationConfigAsset.NotificationItems)
                if (item.Type == nType)
                    return item;
            return null;
        }
        public void Show(NotificationType nType,Action<NotificationOption> callback, 
            Action<NotificationConfigAsset.NotificationItem> overrideNotificationAction = null)
        {
            EnqueueNotificationCallback(new NotificationCallback(nType, callback, null, overrideNotificationAction, GetNotificationItem(nType)));
        }

        public void Show(NotificationType nType,Action<NotificationOption, bool> callback,
            Action<NotificationConfigAsset.NotificationItem> overrideNotificationAction = null)
        {
            EnqueueNotificationCallback(new NotificationCallback(nType, null, callback, overrideNotificationAction, GetNotificationItem(nType)));
        }

        void EnqueueNotificationCallback(NotificationCallback callback)
        {
            _notificationTypesInQueue.Add(callback);
            _notificationTypesInQueue.Sort();
        }

        private void Update()
        {
            _notificationFSM.Update();
        }

        void OnClickLeftOption()
        {
            OnOptionClick(0);
            CloseImpl();
        }
        void OnClickRightOption()
        {
            OnOptionClick(1);
            CloseImpl();
        }
        void OnclickBottomOption()
        {
            OnOptionClick(2);
            CloseImpl();
        }
        void OnOptionClick(int idx)
        {
            if (_notificationLifeCycle == NotificationLifeCycle.Displaying)
            {
                if (_currentNotification != null)
                {
                    var notificationItem = GetNotificationItem(_currentNotification.Type);
                    if (notificationItem != null)
                    {
                        _currentNotification.Callback?.Invoke(notificationItem.Options[idx]);
                        _currentNotification.CallbackWithBoolean?.Invoke(notificationItem.Options[idx], _toggleOption.isOn);
                    }
                }
            }
        }

        void ShowImpl(NotificationCallback callback)
        {
            var notificationItem = callback.NotificationAssetItem;
            if (notificationItem != null)
            {
                _notificationLifeCycle = NotificationLifeCycle.Displaying;
                _currentNotification = callback;
                _fadeAnimator.Play(FADE_IN_ANI_NAME);
                notificationItem = notificationItem.Clone();
                // Language
                notificationItem.TitleKey = Localization.Localization.GetTranslation(notificationItem.TitleKey);
                notificationItem.DescriptionKey = Localization.Localization.GetTranslation(notificationItem.DescriptionKey);
                callback.OverrideCallback?.Invoke(notificationItem);
                _mainImage.sprite = notificationItem.MainSprite;
                _mainImage.GetComponent<RectTransform>().sizeDelta = notificationItem.MainSpriteSize;
                _titleText.text = notificationItem.TitleKey;
                _descriptionText.text = notificationItem.DescriptionKey;
                _descriptionText.alignment = notificationItem.DescriptionTextAlignment;
                if (!string.IsNullOrEmpty(notificationItem.CheckedOptionKey))
                {
                    _toggleOption.gameObject.SetActive(true);
                    _toggleOption.isOn = notificationItem.CheckedOptionDefaultOn;
                    _toggleOption.GetComponentInChildren<TMPro.TMP_Text>().text = notificationItem.CheckedOptionKey;
                }
                else
                {
                    _toggleOption.gameObject.SetActive(false);
                    _toggleOption.isOn = false;
                    _toggleOption.GetComponentInChildren<TMPro.TMP_Text>().text = string.Empty;
                }

                if (notificationItem.AdditionSprite != null)
                {
                    _additionalImageLT.gameObject.SetActive(notificationItem.AnchorType == AdditionalSpriteAnchor.LeftTop);
                    _additionalImageRT.gameObject.SetActive(notificationItem.AnchorType == AdditionalSpriteAnchor.RightTop);
                    _additionalImageLB.gameObject.SetActive(notificationItem.AnchorType == AdditionalSpriteAnchor.LeftBottom);
                    _additionalImageRB.gameObject.SetActive(notificationItem.AnchorType == AdditionalSpriteAnchor.RightBottom);
                    _additionalImageLT.sprite = notificationItem.AdditionSprite;
                    _additionalImageRT.sprite = notificationItem.AdditionSprite;
                    _additionalImageLB.sprite = notificationItem.AdditionSprite;
                    _additionalImageRB.sprite = notificationItem.AdditionSprite;

                }
                else
                {
                    _additionalImageLT.gameObject.SetActive(false);
                    _additionalImageRT.gameObject.SetActive(false);
                    _additionalImageLB.gameObject.SetActive(false);
                    _additionalImageRB.gameObject.SetActive(false);
                }
                if (notificationItem.Options.Length == 0)
                {
                    _optionButtonLeft.gameObject.SetActive(false);
                    _optionButtonRight.gameObject.SetActive(false);
                    _optionButtonBottom.gameObject.SetActive(false);
                }
                else if(notificationItem.Options.Length == 1)
                {
                    _optionButtonLeft.gameObject.SetActive(true);
                    _optionButtonLeft.GetComponentInChildren<TMPro.TMP_Text>().text = Localization.Localization.GetTranslation( Enum.GetName(typeof(NotificationOption), notificationItem.Options[0]));
                    _optionButtonRight.gameObject.SetActive(false);
                    _optionButtonBottom.gameObject.SetActive(false);
                }
                else if(notificationItem.Options.Length == 2)
                {
                    _optionButtonLeft.gameObject.SetActive(true);
                    _optionButtonLeft.GetComponentInChildren<TMPro.TMP_Text>().text = Localization.Localization.GetTranslation(Enum.GetName(typeof(NotificationOption), notificationItem.Options[0]));
                    _optionButtonRight.gameObject.SetActive(true);
                    _optionButtonRight.GetComponentInChildren<TMPro.TMP_Text>().text = Localization.Localization.GetTranslation(Enum.GetName(typeof(NotificationOption), notificationItem.Options[1]));
                    _optionButtonBottom.gameObject.SetActive(false);
                }
                else if(notificationItem.Options.Length == 3)
                {
                    _optionButtonLeft.gameObject.SetActive(true);
                    _optionButtonLeft.GetComponentInChildren<TMPro.TMP_Text>().text = Localization.Localization.GetTranslation(Enum.GetName(typeof(NotificationOption), notificationItem.Options[0]));
                    _optionButtonRight.gameObject.SetActive(true);
                    _optionButtonRight.GetComponentInChildren<TMPro.TMP_Text>().text = Localization.Localization.GetTranslation(Enum.GetName(typeof(NotificationOption), notificationItem.Options[1]));
                    _optionButtonBottom.gameObject.SetActive(true);
                    _optionButtonBottom.GetComponentInChildren<TMPro.TMP_Text>().text = Localization.Localization.GetTranslation(Enum.GetName(typeof(NotificationOption), notificationItem.Options[2]));
                }
                else
                {
                    Debug.LogError("Too many options!");
                }
            }
        }
        public void FadeOutAnimationComplete()
        {
            _currentNotification = null;
            _notificationLifeCycle = NotificationLifeCycle.Closed;
        }
        void CloseImpl()
        {
            _fadeAnimator.Play(FADE_OUT_ANI_NAME);
            _notificationLifeCycle = NotificationLifeCycle.Closing;
        }
        public NotificationType Peek()
        {
            if (_currentNotification != null)
                return _currentNotification.Type;
            return NotificationType.None;
        }
        public void Continue()
        {
            if (_currentNotification != null)
            {
                CloseImpl();
            }
        }

        public void Continue(NotificationType nType)
        {
            if(_currentNotification != null && nType == _currentNotification.Type)
            {
                CloseImpl();
            }
        }
    }
}