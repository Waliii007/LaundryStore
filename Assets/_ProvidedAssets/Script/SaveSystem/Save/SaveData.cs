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
    }

    [System.Serializable]
    public class SaveData
    {
        public int taskCompleted = 0;
        public int unlockedMachine = 0;
        public bool isCashierUnlocked;
        public int cleanAIUnlocked = 0;
        public int dirtyBoxAiUnlocked = 0;
        public int playerCash = 2100;
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