using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace LaundaryMan
{
    public class CashierUnlockTrigger : MonoBehaviour
    {
        #region Variables

        public Image fillerImage;
        private bool isPlayerInside;
        public ThingsToUnlock thingsToUnlock;

        #endregion

        private void OnTriggerExit(Collider other)
        {
            progress = 0;
            fillerImage.fillAmount = 0;
            if (!other.CompareTag("Player")) return;
            isPlayerInside = false;
            isNotCalled = false;
        }

        public bool isthingUnlocked = false;

        private void Awake()
        {
            isthingUnlocked = ReferenceManager.Instance.GetUnlockingData(thingsToUnlock.ToString()) == 1;
            if (isthingUnlocked)
            {
                this.gameObject.SetActive(false);
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (!other.CompareTag("Player") &&
                other.gameObject.layer ==LayerMask.GetMask("Player")) return;

            isPlayerInside = true;

            // Start filling UI
            StartCoroutine(FillProgress());
        }

        float progress;

        private IEnumerator FillProgress()
        {
            progress += 0.01f;
            fillerImage.fillAmount = progress;
            yield return new WaitForSeconds(0.1f);


            if (progress >= 1f && !isNotCalled)
            {
                isNotCalled = true;
                ReferenceManager.Instance.canvasManager.CanvasStateChanger(CanvasStates.UnlockCashier);

            }
        }

        private void HandleAdWatched()
        {
            UnlockBuilding();
        }

        private bool isNotCalled;

        private void UnlockBuilding()
        {
            ReferenceManager.Instance.SaveUnlockingData(thingsToUnlock.ToString(), 1);
            switch (thingsToUnlock)
            {
                case ThingsToUnlock.LaundryMachine1:
                    break;
                case ThingsToUnlock.LaundryMachine2:
                    break;
                case ThingsToUnlock.LaundryMachine3:
                    break;
                case ThingsToUnlock.HrOfHiringTeam:
                    break;
                case ThingsToUnlock.HrOfUpgradingTeam:
                    break;
                case ThingsToUnlock.Cashier:
                    ReferenceManager.Instance.GameData.isCashierUnlocked = true;
                    ReferenceManager.Instance.SaveGameDataObserver();
                    ReferenceManager.Instance.taskHandler.OnTaskCompleted();
                    break;
            }

            gameObject.SetActive(false);
        }
    }
}