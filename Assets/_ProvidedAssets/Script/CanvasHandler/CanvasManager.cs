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
                    break;
                case CanvasStates.Hr:
                    if (SoundManager.instance)
                    {
                        SoundManager.instance.Play(SoundName.PopUp);
                    }

                    break;
                case CanvasStates.HrUpgrades:
                    if (SoundManager.instance)
                    {
                        SoundManager.instance.Play(SoundName.PopUp);
                    }

                    break;
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
    UnlockCashier
}