using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LaundaryMan
{
    public class TaskHandlers : MonoBehaviour
    {
        public PlayerTask[] machineObjects;
        public WorkerUnlockHandlers workerUnlockHandler;
        public PlayerTask[] hrObjects;
        public CashierAi cashierAi;
        public GameObject cashierAiObject;
        public GameObject hiringHrUnlocker;
        public GameObject hiringHrUpgradeUnlocker;

        //Cashier Object
        //Worker AI 

        public void HrHandler()
        {
            workerUnlockHandler.isHrUnlocked = SaveAndLoadSystem.Instance.saveData.isHrUnlocked;
            workerUnlockHandler.isCashierUnlocked = SaveAndLoadSystem.Instance.saveData.isCashierUnlocked;
            workerUnlockHandler.isHrofUpgradeisOn = SaveAndLoadSystem.Instance.saveData.isUpgradeHrUnlocked;
            workerUnlockHandler.cleanAIUnlocked = SaveAndLoadSystem.Instance.saveData.cleanAIUnlocked;
            workerUnlockHandler.dirtyBoxAiUnlocked = SaveAndLoadSystem.Instance.saveData.dirtyBoxAiUnlocked;
            hrObjects[(int)Hr.HiringTeam].tasksObjects.SetActive(workerUnlockHandler.isHrUnlocked);
            hiringHrUpgradeUnlocker.SetActive(
                !workerUnlockHandler.isHrofUpgradeisOn && workerUnlockHandler.isHrUnlocked);
            hrObjects[(int)Hr.UpgradesTeam].tasksObjects
                .SetActive(workerUnlockHandler.isHrofUpgradeisOn && workerUnlockHandler.isHrUnlocked);
            hiringHrUnlocker.SetActive(!workerUnlockHandler.isHrUnlocked);
            cashierAiObject.SetActive(!workerUnlockHandler.isCashierUnlocked &&
                                      ReferenceManager.Instance.GameData.isTutorialCompleted);
            cashierAi.gameObject.SetActive(workerUnlockHandler.isCashierUnlocked);
        }

        public Action OnTaskUpdatedAction;

        public void OnTaskCompleted()
        {
            OnTaskUpdatedAction?.Invoke();
        }

        void OnTaskUpdated()
        {
//            print("ddd" + " " + (SaveAndLoadSystem.Instance.saveData.taskCompleted < machineObjects.Length));
            if (
                ReferenceManager.Instance.GameData.taskCompleted < machineObjects.Length)
            {
                machineObjects[ReferenceManager.Instance.GameData.taskCompleted].tasksObjects.SetActive(true);
            }

            //   ReferenceManager.Instance.machineManager.Init();

            if (SaveAndLoadSystem.Instance.saveData.taskCompleted >= 1)
            {
                HrHandler();
            }

            if (ReferenceManager.Instance.GameData.dirtyBoxAiUnlocked >= 0)
            {
                ReferenceManager.Instance.dirtyBoxAiManager.UnlockAi(ReferenceManager.Instance.GameData
                    .dirtyBoxAiUnlocked);
            }

            if (ReferenceManager.Instance.GameData.cleanAIUnlocked >= 0)
            {
                ReferenceManager.Instance.cleanBoxAIManager.UnlockAi(ReferenceManager.Instance.GameData
                    .cleanAIUnlocked);
            }
        }

        private void Awake()
        {
            OnTaskUpdatedAction += OnTaskUpdated;
        }

        private void OnEnable()
        {
            OnTaskUpdated();
        }
    }
}

public enum Hr
{
    HiringTeam,
    UpgradesTeam,
}

[System.Serializable]
public class PlayerTask
{
    public GameObject tasksObjects;
}

[System.Serializable]
public class WorkerUnlockHandlers
{
    public bool isCashierUnlocked;
    public UpgradeCost[] cashierAICost;
    public bool isHrUnlocked;
    public int cleanAIUnlocked = -1;
    public bool isHrofUpgradeisOn;


    public UpgradeCost[] cleanAICost;
    public int dirtyBoxAiUnlocked = -1;
    public UpgradeCost[] dirtyAICost;
}

[System.Serializable]
public struct UpgradeCost
{
    public int cost;
}