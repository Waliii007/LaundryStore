using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LaundaryMan
{
    public class AddFrequency : MonoBehaviour
    {
        private void OnEnable()
        {
            GC.Collect();
        }

        public void Accept()
        {
            ReferenceManager.Instance.GameData.playerCash += 100;
            TssAdsManager._Instance?.ShowInterstitial("AdsFrequency");
            CanvasScriptSplash.instance.adFrequency.timeElapsed = 0;
            CanvasScriptSplash.instance.adFrequency.isCalled = false;
            CanvasScriptSplash.instance.ChangeCanvas(CanvasStats.MainScreen);
        }
    }
}