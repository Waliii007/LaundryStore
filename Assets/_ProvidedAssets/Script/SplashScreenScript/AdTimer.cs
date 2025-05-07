
using System;
using UnityEngine;

namespace LaundaryMan
{
    public class AdTimer : MonoBehaviour
    {
        public float adTimer = 120f; // 2 minutes
        public float timeElapsed = 0f;

        public void Init()
        {
            adTimer = GlobalConstant.adTimer;
        }

        void Update()
        {
            timeElapsed += Time.deltaTime;

            if (timeElapsed >= adTimer && !isCalled)
            {
                isCalled = true;
                ShowAd();
            }
        }

        public bool isCalled;

        void ShowAd()
        {
            CanvasScriptSplash.instance.ChangeCanvas(CanvasStats.AdFrequency);
        }
    }
}