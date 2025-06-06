using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Serialization;

namespace LaundaryMan
{
    public class SaveAndLoadSystem : MonoBehaviour
    {
        [FormerlySerializedAs("_saveData")] public SaveData saveData; // Assign the ScriptableObject in the Inspector
        public string savePath;
        public static SaveAndLoadSystem Instance;

        private void Awake()
        {
            GameData.selectedPlayers.Clear();

            if (Instance == null)
                Instance = this;
            else
                Destroy(this.gameObject);
            DontDestroyOnLoad(this);
            savePath = Path.Combine(Application.persistentDataPath, "MADARAUCHIHA.json");
            LoadGame();
        }

        public SaveData GameData
        {
            get => saveData;
            set => saveData = value;
        }

        public void SaveGame()
        {
            savePath = Path.Combine(Application.persistentDataPath, "MADARAUCHIHA.json");
            GameData.ConvertDictionaryToList();
            string json = JsonUtility.ToJson(GameData, true);
            File.WriteAllText(savePath, json);
//            print($"Game data saved at: {savePath}");
        }

        public void ResetData()
        {
            string savePath = Path.Combine(Application.persistentDataPath, "MADARAUCHIHA.json");

            // Delete the save file if it exists
            if (File.Exists(savePath))
            {
                File.Delete(savePath);
            }

            // Reset data in memory
            SaveAndLoadSystem.Instance.saveData = new SaveData();
            SaveAndLoadSystem.Instance.SaveGame();

            Debug.Log("Game data has been reset.");
        }

        public void LoadGame()
        {
            saveData = new SaveData();
            if (File.Exists(savePath))
            {
                string json = File.ReadAllText(savePath);
                SaveData loadedData = JsonUtility.FromJson<SaveData>(json);

                saveData.unlockedMachine = loadedData.unlockedMachine;
                saveData.taskCompleted = loadedData.taskCompleted;
                saveData.isCashierUnlocked = loadedData.isCashierUnlocked;
                saveData.isHrUnlocked = loadedData.isHrUnlocked;
                saveData.isTutorialCompleted = loadedData.isTutorialCompleted;
                saveData.isUpgradeHrUnlocked = loadedData.isUpgradeHrUnlocked;
                saveData.cleanAIUnlocked = loadedData.cleanAIUnlocked;
                saveData.dirtyBoxAiUnlocked = loadedData.dirtyBoxAiUnlocked;
                saveData.playerCash = loadedData.playerCash;
                saveData.speedOfAI = loadedData.speedOfAI;
                saveData.carryCapacityOfAI = loadedData.carryCapacityOfAI;
                saveData.playerMovingSpeed = loadedData.playerMovingSpeed;
                saveData.stackingSpeed = loadedData.stackingSpeed;
                saveData.playerCapacity = loadedData.playerCapacity;

                saveData.gameEconomy.rinMax = loadedData.gameEconomy.rinMax;
                saveData.gameEconomy.detergentMax = loadedData.gameEconomy.detergentMax;


                saveData.gameEconomy.machine1Price = loadedData.gameEconomy.machine1Price;
                saveData.gameEconomy.machine2Price = loadedData.gameEconomy.machine2Price;
                saveData.gameEconomy.machine3Price = loadedData.gameEconomy.machine3Price;
                saveData.gameEconomy.cashierPrice = loadedData.gameEconomy.cashierPrice;
                saveData.gameEconomy.hrPrice = loadedData.gameEconomy.hrPrice;
                saveData.gameEconomy.hrUpgradePrice = loadedData.gameEconomy.hrUpgradePrice;


                saveData.gameEconomy.machineUpgradeIndex = loadedData.gameEconomy.machineUpgradeIndex;
                saveData.gameEconomy.machine1UpgradeIndex = loadedData.gameEconomy.machine1UpgradeIndex;
                saveData.gameEconomy.machine2UpgradeIndex = loadedData.gameEconomy.machine2UpgradeIndex;
                saveData.gameEconomy.limitOnCleanBasket = loadedData.gameEconomy.limitOnCleanBasket;
                saveData.gameEconomy.limitBasket = loadedData.gameEconomy.limitBasket;


                saveData.gameEconomy.remainingDetergentBottleBlue = loadedData.gameEconomy.remainingDetergentBottleBlue;
                saveData.gameEconomy.remainingDetergentBottleRed = loadedData.gameEconomy.remainingDetergentBottleRed;
                saveData.gameEconomy.remainingDetergentBottleGreen =
                    loadedData.gameEconomy.remainingDetergentBottleGreen;


                saveData.gameEconomy.remainingRinseBottleBlue = loadedData.gameEconomy.remainingRinseBottleBlue;
                saveData.gameEconomy.remainingRinseBottleGreen = loadedData.gameEconomy.remainingRinseBottleGreen;
                saveData.gameEconomy.remainingRinseBottleBlue = loadedData.gameEconomy.remainingRinseBottleBlue;

                saveData.gameEconomy.detergentCapacity1 = loadedData.gameEconomy.detergentCapacity1;
                saveData.gameEconomy.detergentCapacity2 = loadedData.gameEconomy.detergentCapacity2;
                saveData.gameEconomy.detergentCapacity3 = loadedData.gameEconomy.detergentCapacity3;


                saveData.gameEconomy.gloriaPantsCount = loadedData.gameEconomy.gloriaPantsCount;
                saveData.gameEconomy.starPantsCount = loadedData.gameEconomy.starPantsCount;
                saveData.gameEconomy.simHortanCount = loadedData.gameEconomy.simHortanCount;


                saveData.selectedPlayersList = new List<SaveData.PlayerSelection>(loadedData.selectedPlayersList);
//                print(loadedData.gameEconomy.cashierPrice + saveData.gameEconomy.cashierPrice);
                saveData.ConvertListToDictionary();

                if (GlobalConstant.isLogger)
                    print("Game data loaded successfully.");

                // Notify all scripts that data has been updated
            }
            else
            {
                if (GlobalConstant.isLogger)
                    print("No save file found. Using default values.");
            }

            if (!saveData.isTutorialCompleted)
            {
                ResetData();
            }
        }

        public void SaveUnlockingData(string key, int value)
        {
            if (ReferenceManager.Instance.GameData.selectedPlayers == null)
            {
                ReferenceManager.Instance.GameData.selectedPlayers = new Dictionary<string, int>();
            }

            ReferenceManager.Instance.GameData.selectedPlayers["Value_" + key] = value;
            SaveGame();
        }
    }
}