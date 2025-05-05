using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace LaundaryMan
{
    public class HrPanelCaller : MonoBehaviour
    {
        public Image fillImage;
        public bool isPanelShown;
        public CanvasStates stateToCall = CanvasStates.Hr;

        private void OnTriggerStay(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            if (!isPanelShown)
            {
                fillImage.fillAmount += 0.5f * Time.deltaTime;

                if (fillImage.fillAmount >= 1)
                {
                    this.gameObject.SetActive(ReferenceManager.Instance.GameData.isTutorialCompleted);
                    isPanelShown = true;
                    ReferenceManager.Instance.canvasManager.CanvasStateChanger(stateToCall);
                }
            }
            
        }

        private void OnTriggerExit(Collider other)
        {
            fillImage.fillAmount = 0;
            isPanelShown = false;
        }
    }
}