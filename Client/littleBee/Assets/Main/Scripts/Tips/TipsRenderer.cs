/*
 * File: TipsRenderer.cs
 *
 * Copyright(c) 2023 Lenovo, Inc. and/or its subsidiaries. All rights reserved.
 *
 * Confidential and Proprietary
 *
 */

using System;
using TMPro;
using UnityEngine;

namespace Synchronize.Game.Lockstep.Tips
{
    public class TipsRenderer : MonoBehaviour
    {
        [SerializeField] GameObject background;
        [SerializeField] TMP_Text tipsText;
        [SerializeField] GameObject arrow;
        [SerializeField] RectTransform spaceTransform;
        CanvasGroup canvasGroup;
        Canvas canvas;
        private void Awake()
        {
            canvas = GetComponent<Canvas>();
            canvasGroup = GetComponent<CanvasGroup>();
        }
        public void SetContent(string value, TextAlignmentOptions alignmentOptions, TipsSortingLayer layer,  int sortingOrder)
        {
            canvas.sortingLayerName = Enum.GetName(typeof(TipsSortingLayer),layer);
            canvas.sortingOrder = sortingOrder;
            tipsText.text = value;
            tipsText.alignment = alignmentOptions;
        }
        public Bounds GetBackgroundBounds() 
        {
            return RectTransformUtility.CalculateRelativeRectTransformBounds(background.transform);
        }
        public void UpdateSpaceHeight(float value)
        {
            spaceTransform.sizeDelta = new Vector2(spaceTransform.sizeDelta.x, value);
        }
        public void SetCanvasAlpha(float alpha)
        {
            canvasGroup.alpha = alpha;
        }
        public float GetCanvasAlpha() { return canvasGroup.alpha; }
    }
}