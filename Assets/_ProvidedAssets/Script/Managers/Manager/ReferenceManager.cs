using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Invector.vCharacterController;
using LaundryMan;
using Unity.VisualScripting;
using UnityEngine;

namespace LaundaryMan
{
    public class ReferenceManager : MonoBehaviour
    {
        #region Singleton

        public static ReferenceManager Instance { get; private set; }

        //public GameObject environment;

        private void Awake()
        {
           // environment.SetActive(true);
            Instance = this;
        }

        #endregion

        #region Variable

        public DirtyBoxAiManager dirtyBoxAiManager;
        public TutorialHandler tutorialHandler;
        public bool isGameEnd = false;
        public PlayerStackManager playerStackManager;
        public vThirdPersonController controller;
        public QueueSystem queueSystem;
        public CheckoutHandler checkoutHandler;
        public DirtyBasketTrigger basketTrigger;
        public CleanBoxAIManager cleanBoxAIManager;
        public MachineManager machineManager;
        public TaskHandlers taskHandler;
        public CanvasManager canvasManager;
        public static List<Ai> activePickers;
        public static Action OnPLayerGotUpgrade;
        public ObjectivePanel objectivePanel;
        public static int CustomerAmountOfClothForTutorial;

        #endregion

        #region SaveSystemObserver

        public SaveData GameData => SaveAndLoadSystem.Instance.GameData;

        public static Action SaveDataAction;

        public void SaveGameDataObserver()
        {
            SaveDataAction.Invoke();
        }

        #endregion

        #region Function

        private void OnEnable()
        {
            Time.timeScale = 1;
            activePickers = new List<Ai>();
            SaveDataAction += SaveAndLoadSystem.Instance.SaveGame;
            TssAdsManager._Instance?.ShowBanner("MainScreen");
            TssAdsManager._Instance?.TopShowBanner("MainScreen");
        }

        public void SaveUnlockingData(string key, int value)
        {
            SaveAndLoadSystem.Instance.SaveUnlockingData(key, value);
        }

        // Load selected player dynamically
        public int GetUnlockingData(string key)
        {
            return SaveAndLoadSystem.Instance.GameData.selectedPlayers.ContainsKey("Value_" + key)
                ? SaveAndLoadSystem.Instance.GameData.selectedPlayers["Value_" + key]
                : -1;
        }

        #endregion
    }
}