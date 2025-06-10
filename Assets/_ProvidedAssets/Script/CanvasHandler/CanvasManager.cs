using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LaundaryMan
{
    public class CanvasManager : MonoBehaviour
    {
        public CanvasStates prev;
        public CanvasStates currentState;
        public Text cashText;
        public MachineButton machineButton;

        public void Setting()
        {
            CanvasScriptSplash.instance.ChangeCanvas(CanvasStats.Setting);
        }
        private void OnEnable()
        {
            StartCoroutine(TextUpdater());
        }

        public void CanvasStateChanger(CanvasStates newState)
        {
            prev = currentState;
            currentState = newState;
            canvasStates[(int)prev].SetActive(false);
            canvasStates[(int)currentState].SetActive(true);
            switch (newState)
            {
                case CanvasStates.MainControls:
                    TssAdsManager._Instance?.ShowBanner("MainControl");
                    TssAdsManager._Instance?.admobInstance.TopShowBanner();
                    break;
                case CanvasStates.Hr:
                    if (SoundManager.instance)
                    {
                        SoundManager.instance.Play(SoundName.PopUp);
                    }
                    TssAdsManager._Instance?.ShowInterstitial(newState.ToString());
                    break;
                case CanvasStates.HrUpgrades:
                    if (SoundManager.instance)
                    {
                        SoundManager.instance.Play(SoundName.PopUp);
                    }
                    TssAdsManager._Instance?.ShowInterstitial(newState.ToString());
                    break;

                case CanvasStates.ObjectivePanel:
                    
                    break;
                case CanvasStates.UnlockCashier:
                    
                    break;
                case CanvasStates.DetergentPurchase:
                    
                    TssAdsManager._Instance?.ShowInterstitial(newState.ToString());

                    break;
                case CanvasStates.RefillDetergent:
                    TssAdsManager._Instance?.ShowInterstitial(newState.ToString());

                    break;
                case CanvasStates.RinPurchase:
                    TssAdsManager._Instance?.ShowInterstitial(newState.ToString());

                    break;
                case CanvasStates.RinRefill:
                    TssAdsManager._Instance?.ShowInterstitial(newState.ToString());

                    break;
                case CanvasStates.CandyBar:
                    TssAdsManager._Instance?.ShowInterstitial(newState.ToString());

                    break;
                case CanvasStates.TutorialObjective:
                    
                    break;
                case CanvasStates.Empty:
                    
                    break;
                case CanvasStates.CoffeePurchase:
                    TssAdsManager._Instance?.ShowInterstitial(newState.ToString());

                    break;
                case CanvasStates.CoffeeUse:
                    TssAdsManager._Instance?.ShowInterstitial(newState.ToString());
                    break;
            
            }

            if (TSS_AnalyticalManager.instance)
            {
                TSS_AnalyticalManager.instance.CustomScreenEvent(nameof(newState));
            }
        }

        public WaitForSeconds waitForTextUpdate = new WaitForSeconds(.01f);

        public IEnumerator TextUpdater()
        {
            while (ReferenceManager.Instance.isGameEnd)
            {
                cashText.text = ReferenceManager.Instance.GameData.playerCash + "";
                yield return waitForTextUpdate;
            }

            yield return waitForTextUpdate;
        }

        [SerializeField] private GameObject[] canvasStates;
    }
}

public enum CanvasStates
{
    MainControls,
    Hr,
    HrUpgrades,
    ObjectivePanel,
    UnlockCashier,
    DetergentPurchase,
    RefillDetergent,
    RinPurchase,
    RinRefill,
    CandyBar,
    TutorialObjective,
    Empty,
    CoffeePurchase,
    CoffeeUse,
    
}