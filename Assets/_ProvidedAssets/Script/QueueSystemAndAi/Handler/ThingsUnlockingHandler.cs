using System;
using System.Collections;
using System.Collections.Generic;
using GameAnalyticsSDK.Setup;
using UnityEngine;
using UnityEngine.UI;

namespace LaundaryMan
{
    public class ThingsUnlockingHandler : MonoBehaviour
    {
        #region Variables

        public int cashToUnlock;
        public Image fillerImage;
        private Coroutine _playerCoroutine;
        private bool isPlayerInside;
        public ThingsToUnlock thingsToUnlock;
        public GameObject arrowObject;
        public GameObject selfExplainationObject;
        #endregion

        #region TriggerEvents

        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            isPlayerInside = false;
            if (_playerCoroutine != null)
            {
                StopCoroutine(_playerCoroutine);
                _playerCoroutine = null;
            }
            selfExplainationObject.SetActive(true);
            arrowObject?.SetActive(!ReferenceManager.Instance.GameData.isTutorialCompleted);
            selfExplainationObject.SetActive(isPlayerInside);

        }

        public bool isthingUnlocked = false;
        public Text selfExplainationText;
        public void Init()
        {
            switch (thingsToUnlock)
            {
                case ThingsToUnlock.LaundryMachine1:
                    cashToUnlock = ReferenceManager.Instance.GameData.gameEconomy.machine1Price;
                    selfExplainationText.text = "Building First Machine";
                    break;
                case ThingsToUnlock.LaundryMachine2:
                    cashToUnlock = ReferenceManager.Instance.GameData.gameEconomy.machine2Price;
                    selfExplainationText.text = "Building Second Machine";

                    break;
                case ThingsToUnlock.LaundryMachine3:
                    cashToUnlock = ReferenceManager.Instance.GameData.gameEconomy.machine3Price;
                    selfExplainationText.text = "Building Third Machine";
                    break;
                case ThingsToUnlock.HrOfHiringTeam:
                    cashToUnlock = ReferenceManager.Instance.GameData.gameEconomy.hrPrice;
                    selfExplainationText.text = "Building HR Room";
                    break;
                case ThingsToUnlock.HrOfUpgradingTeam:
                    cashToUnlock = ReferenceManager.Instance.GameData.gameEconomy.hrUpgradePrice;
                    selfExplainationText.text = "Building Performance HR Room ";

                    break;
                case ThingsToUnlock.Cashier:
                    cashToUnlock = ReferenceManager.Instance.GameData.gameEconomy.cashierPrice;
                    selfExplainationText.text = "Unlocking Cashier";
                    break;
            }

            fillerImage.fillAmount = 1 - (float)cashToUnlock / initialCash;
            amountText.text = cashToUnlock.ToString();
        }

        private void Awake()
        {
            Init();
            isthingUnlocked = ReferenceManager.Instance.GetUnlockingData(thingsToUnlock.ToString()) == 1;
            if (isthingUnlocked)
            {
                this.gameObject.SetActive(false);
            }
            selfExplainationObject.SetActive(false);

        }
            
        private void OnTriggerStay(Collider other)
        {
            if (!other.CompareTag("Player") &&
                other.gameObject.layer == ReferenceManager.Instance.playerStackManager.gameObject.layer) return;

            isPlayerInside = true;

            if (_playerCoroutine == null)
                _playerCoroutine = StartCoroutine(HandlePlayerInteraction());
            selfExplainationObject.SetActive(isPlayerInside);

        }

        public int initialCash = 2000;
        public Text amountText;
        public WaitForSeconds waitForSecondsAiSpawn = new WaitForSeconds(.1f);

        private IEnumerator HandlePlayerInteraction()
        {
            int initialPlayerCash = ReferenceManager.Instance.GameData.playerCash;
            int spentAmount = 0;

            while (isPlayerInside && ReferenceManager.Instance.GameData.playerCash > 0 && cashToUnlock > 0)
            {
                int deduction = Mathf.Min(40, ReferenceManager.Instance.GameData.playerCash, cashToUnlock);
                cashToUnlock -= deduction;
                ReferenceManager.Instance.GameData.playerCash -= deduction;
                spentAmount += deduction;
                switch (thingsToUnlock)
                {
                    case ThingsToUnlock.LaundryMachine1:
                        ReferenceManager.Instance.GameData.gameEconomy.machine1Price = cashToUnlock;
                        ReferenceManager.Instance.SaveGameDataObserver();

                        break;
                    case ThingsToUnlock.LaundryMachine2:
                        ReferenceManager.Instance.GameData.gameEconomy.machine2Price = cashToUnlock;
                        ReferenceManager.Instance.SaveGameDataObserver();
                        break;
                    case ThingsToUnlock.LaundryMachine3:
                        ReferenceManager.Instance.GameData.gameEconomy.machine3Price = cashToUnlock;
                        ReferenceManager.Instance.SaveGameDataObserver();
                        break;
                    case ThingsToUnlock.HrOfHiringTeam:
                        ReferenceManager.Instance.GameData.gameEconomy.hrPrice = cashToUnlock;
                        ReferenceManager.Instance.SaveGameDataObserver();
                        break;
                    case ThingsToUnlock.HrOfUpgradingTeam:
                        ReferenceManager.Instance.GameData.gameEconomy.hrUpgradePrice = cashToUnlock;
                        ReferenceManager.Instance.SaveGameDataObserver();
                        break;
                    case ThingsToUnlock.Cashier:
                        ReferenceManager.Instance.GameData.gameEconomy.cashierPrice = cashToUnlock;
                        ReferenceManager.Instance.SaveGameDataObserver();
                        ReferenceManager.Instance.taskHandler.OnTaskCompleted();
                        break;
                }

                SaveAndLoadSystem.Instance.SaveGame();

                fillerImage.fillAmount = 1f - (float)cashToUnlock / initialCash;
                amountText.text = cashToUnlock.ToString();

                yield return waitForSecondsAiSpawn;
            }

            if (cashToUnlock <= 0)
            {
                UnlockBuilding();
            }
            else
            {
            }

            ReferenceManager.Instance.SaveGameDataObserver();

            _playerCoroutine = null;
        }

        private void UnlockBuilding()
        {
            switch (thingsToUnlock)
            {
                case ThingsToUnlock.LaundryMachine1:
                    ReferenceManager.Instance.SaveUnlockingData(thingsToUnlock.ToString(), 1);
                    ReferenceManager.Instance.GameData.taskCompleted++;
                    ReferenceManager.Instance.machineManager.MachineByIndex
                        (ReferenceManager.Instance.GameData.unlockedMachine);
                    ReferenceManager.Instance.GameData.unlockedMachine++;
                    ReferenceManager.Instance.taskHandler.OnTaskCompleted();
                    break;
                case ThingsToUnlock.LaundryMachine2:
                    ReferenceManager.Instance.SaveUnlockingData(thingsToUnlock.ToString(), 1);
                    ReferenceManager.Instance.GameData.taskCompleted++;
                    ReferenceManager.Instance.machineManager.MachineByIndex
                        (ReferenceManager.Instance.GameData.unlockedMachine);
                    ReferenceManager.Instance.GameData.unlockedMachine++;
                    ReferenceManager.Instance.taskHandler.OnTaskCompleted();

                    break;
                case ThingsToUnlock.LaundryMachine3:
                    ReferenceManager.Instance.SaveUnlockingData(thingsToUnlock.ToString(), 1);
                    ReferenceManager.Instance.GameData.taskCompleted++;
                    ReferenceManager.Instance.machineManager.MachineByIndex
                        (ReferenceManager.Instance.GameData.unlockedMachine);
                    ReferenceManager.Instance.GameData.unlockedMachine++;
                    ReferenceManager.Instance.taskHandler.OnTaskCompleted();

                    break;
                case ThingsToUnlock.HrOfHiringTeam:
                    ReferenceManager.Instance.SaveUnlockingData(thingsToUnlock.ToString(), 1);
                    if (GlobalConstant.isLogger)
                        print("isHrUnlocked");
                    ReferenceManager.Instance.GameData.isHrUnlocked = true;
                    ReferenceManager.Instance.taskHandler.OnTaskCompleted();
                    break;
                case ThingsToUnlock.HrOfUpgradingTeam:
                    ReferenceManager.Instance.SaveUnlockingData(thingsToUnlock.ToString(), 1);
                    ReferenceManager.Instance.GameData.isUpgradeHrUnlocked = true;
                    ReferenceManager.Instance.taskHandler.OnTaskCompleted();
                    break;
                case ThingsToUnlock.Cashier:
                    ReferenceManager.Instance.GameData.isCashierUnlocked = true;
                    ReferenceManager.Instance.taskHandler.OnTaskCompleted();
                    break;
            }

            gameObject.SetActive(false);
        }

        #endregion
    }

    public enum ThingsToUnlock
    {
        LaundryMachine1,
        LaundryMachine2,
        LaundryMachine3,
        HrOfHiringTeam,
        HrOfUpgradingTeam,
        Cashier
    }
}