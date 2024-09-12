/*
 * File: TipsTrigger.cs
 *
 * Copyright(c) 2023 Lenovo, Inc. and/or its subsidiaries. All rights reserved.
 *
 * Confidential and Proprietary
 *
 */

using Synchronize.Game.Lockstep.FSM;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Synchronize.Game.Lockstep.Tips
{
    public class TipsTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
    {
        enum State
        {
            Idle = 1,
            PointerEnter = 2,
            FadingIn = 4,
            Displaying = 8,
            PointerExit = 16,
        }

        private const float WAIT_BEFORE_FADEIN_SECS = 0.5f;
        private const float FADE_DELAY_SECS = 0.2f; //must be non-zero
        private const float DISPLAYING_DURATION_SECS = 4f;
        private const string TIPS_RENDERER_NAME = "TipsRenderer";
        private TipsRenderer _tipsRenderer;
        private RectTransform _tipsRectTransform;
        private int _currentPointerId = 0;
        private Dictionary<int, Vector3> _controllerPointerIds = new Dictionary<int, Vector3>();
        private Func<string> _textTipsContentFunc;
        private TipsSortingLayer _tipsSortingLayer;
        private int _tipsSortingOrder;
        private bool _isActive;
        private bool _isPointerHover = false;
        private IStateMachine<TipsTrigger> _tipsFsm;
        private Timer _timer;
        private RectTransform _rectTransform;
        [SerializeField]
        private string _staticContentKey = string.Empty;
        [SerializeField]
        private TMPro.TextAlignmentOptions _textAlignmentOptions = TMPro.TextAlignmentOptions.Top;
        [SerializeField]
        private TipsSortingLayer _staticContentSortingLayer = TipsSortingLayer.Default;
        [SerializeField]
        private int _staticContentSortingOrder = 1024;
        public bool TipsEnabled = true;
        public float AdditionalHeight;
        public float AdditionalScale = 1;
        public bool LookAtCamera = false;
        public bool UpdateContentDuringDisplaying = false;
        public float OverrideDisplayingDurationSecs = DISPLAYING_DURATION_SECS;
        static int CurrentHashCode;
        void Awake()
        {
            _timer = new Timer();
            _tipsFsm = new StateMachine<TipsTrigger>(this);
            _rectTransform = GetComponent<RectTransform>();
        }
        void Start()
        {
            _tipsFsm
                .State(State.Idle)
                    .Transition(so => so._isPointerHover).To(State.PointerEnter).End()
                .End()
                .State(State.PointerEnter)
                    .Enter(so => so._timer.Reset())
                    .Transition(so => !so._isPointerHover).To(State.Idle).End()
                    .Transition(so => so._timer >= WAIT_BEFORE_FADEIN_SECS).To(State.FadingIn).End()
                .End()
                .State(State.FadingIn)
                    .Enter(so => { so._timer.Reset(); so.ShowTip(); })
                    .Update(so =>
                    {
                        if (!so._tipsRectTransform.gameObject.activeSelf)
                            so._tipsRectTransform.gameObject.SetActive(true);
                        so._tipsRenderer.SetCanvasAlpha(so._timer / FADE_DELAY_SECS);
                    })
                    .Transition(so => so._timer > FADE_DELAY_SECS).To(State.Displaying).End()
                .End()
                .State(State.Displaying)
                    .Enter(so => so._timer.Reset())
                    .Update(so =>
                    {
                        if (so.UpdateContentDuringDisplaying)
                            so.ApplyContent();
                    })
                    .Transition(so => so.OverrideDisplayingDurationSecs > -1 ? so._timer > so.OverrideDisplayingDurationSecs : false).To(State.PointerExit).End()
                    .Transition(so => so.GetHashCode() != CurrentHashCode).To(State.PointerExit).End()
                .End()
                .State(State.PointerExit)
                    .Enter(so => { so._timer.Reset(); })
                    .Update(so => { so._tipsRenderer?.SetCanvasAlpha(1 - so._timer / FADE_DELAY_SECS); })
                    .Transition(so => so._timer > FADE_DELAY_SECS).Transfer(so => { so.HideTip(); so.OnPointerExit(null); }).To(State.Idle).End()
                .End()
                .Select(so => !_isPointerHover, State.FadingIn | State.Displaying, State.PointerExit, null)
                .Build().SetDefault(State.Idle);

            if (!string.IsNullOrEmpty(_staticContentKey))
                SetToolTipsImpl(() => Synchronize.Game.Lockstep.Localization.Localization.GetTranslation(_staticContentKey), _staticContentSortingLayer, _staticContentSortingOrder);
        }

        public void OnPointerMove(PointerEventData eventData)
        {
            _controllerPointerIds[eventData.pointerId] = eventData.position;
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            if(eventData != null)
                _controllerPointerIds[eventData.pointerId] = Vector3.zero;
            _isPointerHover = false;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _controllerPointerIds[eventData.pointerId] = eventData.position;
            _currentPointerId = eventData.pointerId;
            if (TipsEnabled)
                _isPointerHover = true;
        }

        public void SetToolTipsImpl(Func<string> func, TipsSortingLayer tipsSortingLayer,int sortOrder )
        {
            _textTipsContentFunc = func;
            _tipsSortingLayer = tipsSortingLayer;
            _tipsSortingOrder = sortOrder;

            if (_isActive)
                OnPointerEnter(null);
        }

        private void OnDisable()
        {
            HideTip();
        }

        private void OnDestroy()
        {
            if(_tipsRenderer!=null && _tipsRenderer.gameObject!=null)
                Destroy(_tipsRenderer.gameObject);
            _tipsRenderer = null;
        }
        
        private float GetTransformHalfHeight() {
            return _rectTransform.sizeDelta.y / 2f;
        }

        private void ShowTip()
        {
            if (_textTipsContentFunc!=null)
            {   
                GameObject tipContent = Instantiate(Resources.Load(TIPS_RENDERER_NAME) as GameObject, transform);
                tipContent.SetActive(false);
                tipContent.name = "[tip]"+ TIPS_RENDERER_NAME;
                _tipsRenderer = tipContent.GetComponent<TipsRenderer>();
                _tipsRectTransform = tipContent.GetComponent<RectTransform>();
                _tipsRenderer.UpdateSpaceHeight(GetTransformHalfHeight()+AdditionalHeight);
                ApplyContent();
                UpdateTipPose();
                _isActive = true;
                CurrentHashCode = GetHashCode();
            }
        }

        private void ApplyContent()
        {
            if (_tipsRenderer!=null&&_textTipsContentFunc != null)
            {
                _tipsRenderer.SetContent(_textTipsContentFunc(),_textAlignmentOptions, _tipsSortingLayer, _tipsSortingOrder);
            }
        }

        private void UpdateTipPose()
        {
            UpdateTipPosition();
            if(LookAtCamera)
                UpdateTipForward();
        }
        private void UpdateTipForward()
        {
            _tipsRectTransform.forward = (_tipsRectTransform.position - Camera.main.transform.position).normalized;
        }
        private void UpdateTipPosition()
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_rectTransform, _controllerPointerIds[_currentPointerId], Camera.main, out Vector2 uipos);

            _tipsRectTransform.localPosition = Vector3.zero+new Vector3(uipos.x,0,0);
            _tipsRectTransform.localScale = Vector3.one / (transform.localScale.x) * AdditionalScale;   
        }

        private void HideTip()
        {
            if (_tipsRenderer != null && _tipsRenderer.gameObject != null)
            {
                Destroy(_tipsRenderer.gameObject);
            }
            _tipsRenderer = null;
            _isActive = false;
        }

        private void Update()
        {
            _tipsFsm.Update();
        }
    }
}