using System;
using System.Collections.Generic;
using UnityEngine;


namespace LaundaryMan
{
    [System.Serializable]
    public class GameEconomy
    {
        public int machine1Price = 2000;
        public int machine2Price = 2000;
        public int machine3Price = 2000;
        public int cashierPrice = 999;
        public int hrPrice = 2000;
        public int hrUpgradePrice = 2000;
        public int limitBasket = 10;
        public int limitOnCleanBasket = 10;
        public int machineUpgradeIndex = 0;
        public int machine1UpgradeIndex = 0;
        public int machine2UpgradeIndex = 0;
        public int rinMax = 250;
        public int detergentMax = 250;
        public int detergentCapacity1 = 250;
        public int detergentCapacity2 = 250;
        public int detergentCapacity3 = 250;
        public int rin1Capacity = 250;
        public int rin2Capacity = 250;
        public int rin3Capacity = 250;
        public int remainingDetergentBottleGreen = 1;
        public int remainingDetergentBottleBlue = 1;
        public int remainingDetergentBottleRed = 1;

        public int remainingRinseBottleGreen = 1;
        public int remainingRinseBottleBlue = 1;
        public int remainingRinseBottleRed = 1;
    }

    [System.Serializable]
    public class SaveData
    {
        public int taskCompleted = 0;
        public int unlockedMachine = 0;
        public bool isCashierUnlocked;
        public int cleanAIUnlocked = 0;
        public int dirtyBoxAiUnlocked = 0;
        public int playerCash = 5000;
        public bool isHrUnlocked = false;
        public bool isUpgradeHrUnlocked = false;
        public bool isTutorialCompleted = false;
        public int speedOfAI = 0;
        public int carryCapacityOfAI = 0;
        public int playerMovingSpeed = 0;
        public int stackingSpeed = 0;
        public int playerCapacity = 0;

        public GameEconomy gameEconomy = new GameEconomy();

        [Serializable]
        public class PlayerSelection
        {
            public string key;
            public int value;
        }

        public List<PlayerSelection> selectedPlayersList = new List<PlayerSelection>();

        // Convert List back to Dictionary at runtime
        public Dictionary<string, int> selectedPlayers = new Dictionary<string, int>();

        private void OnEnable()
        {
            ConvertListToDictionary();
        }

        public void ConvertListToDictionary()
        {
            selectedPlayers.Clear();
            foreach (var entry in selectedPlayersList)
            {
                selectedPlayers[entry.key] = entry.value;
            }
        }

        public void ConvertDictionaryToList()
        {
            selectedPlayersList.Clear();
            foreach (var kvp in selectedPlayers)
            {
                selectedPlayersList.Add(new PlayerSelection { key = kvp.Key, value = kvp.Value });
            }
        }
    }
}