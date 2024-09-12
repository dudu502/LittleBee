/*
 * File: TipsExtends.cs
 *
 * Copyright(c) 2023 Lenovo, Inc. and/or its subsidiaries. All rights reserved.
 *
 * Confidential and Proprietary
 *
 */
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Synchronize.Game.Lockstep.Tips 
{
    public enum TipsSortingLayer
    {
        Default,
        Notification,
    }
    public static class TipsExtends
    {
        public static TipsTrigger SetToolTips(this MonoBehaviour behaviour, Func<string> textCallback, TipsSortingLayer tipsSortingLayer=TipsSortingLayer.Default,int sortOrder=1024)
        {
            TipsTrigger tipsTrigger = behaviour.GetComponent<TipsTrigger>();
            if (tipsTrigger == null)
            {
                tipsTrigger = behaviour.gameObject.AddComponent<TipsTrigger>();
            }
            tipsTrigger.SetToolTipsImpl(textCallback,tipsSortingLayer,sortOrder);
            return tipsTrigger;
        }
    }
}
