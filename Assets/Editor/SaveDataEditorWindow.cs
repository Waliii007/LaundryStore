using UnityEditor;
using UnityEngine;
using System.IO;
using System.Collections.Generic;

namespace LaundaryMan
{
    public class SaveDataEditorWindow : EditorWindow
    {
        private SaveData saveData;
        private string savePath;
        private Vector2 scrollPos;

        [MenuItem("LaundaryMan/Save Data Editor")]
        public static void ShowWindow()
        {
            GetWindow<SaveDataEditorWindow>("Save Data Editor");
        }

        private void OnEnable()
        {
            savePath = Path.Combine(Application.persistentDataPath, "MADARAUCHIHA.json");
            LoadGameData();
        }

        private void OnGUI()
        {
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

            if (saveData == null)
            {
                EditorGUILayout.HelpBox("No save data found. Create new data or load existing.", MessageType.Warning);
                if (GUILayout.Button("Create New Save Data"))
                {
                    saveData = new SaveData();
                    SaveGameData();
                }

                EditorGUILayout.EndScrollView();
                return;
            }

            EditorGUILayout.LabelField("Save Data Editor", EditorStyles.boldLabel);

            saveData.playerCash = EditorGUILayout.IntField("Player Cash", saveData.playerCash);
            saveData.taskCompleted = EditorGUILayout.IntField("Tasks Completed", saveData.taskCompleted);
            saveData.unlockedMachine = EditorGUILayout.IntField("Unlocked Machine", saveData.unlockedMachine);
            saveData.isCashierUnlocked = EditorGUILayout.Toggle("Cashier Unlocked", saveData.isCashierUnlocked);
            saveData.isHrUnlocked = EditorGUILayout.Toggle("HR Unlocked", saveData.isHrUnlocked);
            saveData.isUpgradeHrUnlocked = EditorGUILayout.Toggle("HR Upgrade Unlocked", saveData.isUpgradeHrUnlocked);
            saveData.isTutorialCompleted = EditorGUILayout.Toggle("Tutorial Completed", saveData.isTutorialCompleted);
            saveData.cleanAIUnlocked = EditorGUILayout.IntField("Clean AI Unlocked", saveData.cleanAIUnlocked);
            saveData.dirtyBoxAiUnlocked =
                EditorGUILayout.IntField("Dirty Box AI Unlocked", saveData.dirtyBoxAiUnlocked);
            saveData.carryCapacityOfAI = EditorGUILayout.IntField("AI Carry Capacity", saveData.carryCapacityOfAI);
            saveData.playerMovingSpeed = EditorGUILayout.IntField("Player Moving Speed", saveData.playerMovingSpeed);
            saveData.speedOfAI = EditorGUILayout.IntField("AI Speed", saveData.speedOfAI);
            saveData.stackingSpeed = EditorGUILayout.IntField("Stacking Speed", saveData.stackingSpeed);
            saveData.playerCapacity = EditorGUILayout.IntField("Player Capacity", saveData.playerCapacity);

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Game Economy", EditorStyles.boldLabel);

            GameEconomy g = saveData.gameEconomy;

            g.cashierPrice = EditorGUILayout.IntField("Cashier Price", g.cashierPrice);
            g.machine1Price = EditorGUILayout.IntField("Machine 1 Price", g.machine1Price);
            g.machine2Price = EditorGUILayout.IntField("Machine 2 Price", g.machine2Price);
            g.machine3Price = EditorGUILayout.IntField("Machine 3 Price", g.machine3Price);
            g.hrPrice = EditorGUILayout.IntField("HR Price", g.hrPrice);
            g.hrUpgradePrice = EditorGUILayout.IntField("HR Upgrade Price", g.hrUpgradePrice);
            g.limitBasket = EditorGUILayout.IntField("Limit Basket", g.limitBasket);
            g.limitOnCleanBasket = EditorGUILayout.IntField("Limit Clean Basket", g.limitOnCleanBasket);
            g.machineUpgradeIndex = EditorGUILayout.IntField("Machine Upgrade Index", g.machineUpgradeIndex);
            g.machine1UpgradeIndex = EditorGUILayout.IntField("Machine 1 Upgrade Index", g.machine1UpgradeIndex);
            g.machine2UpgradeIndex = EditorGUILayout.IntField("Machine 2 Upgrade Index", g.machine2UpgradeIndex);
            g.rinMax = EditorGUILayout.IntField("Rinse Max", g.rinMax);
            g.detergentMax = EditorGUILayout.IntField("Detergent Max", g.detergentMax);

            g.detergentCapacity1 = EditorGUILayout.IntField("Detergent Capacity 1", g.detergentCapacity1);
            g.detergentCapacity2 = EditorGUILayout.IntField("Detergent Capacity 2", g.detergentCapacity2);
            g.detergentCapacity3 = EditorGUILayout.IntField("Detergent Capacity 3", g.detergentCapacity3);
            g.rin1Capacity = EditorGUILayout.IntField("Rinse Capacity 1", g.rin1Capacity);
            g.rin2Capacity = EditorGUILayout.IntField("Rinse Capacity 2", g.rin2Capacity);
            g.rin3Capacity = EditorGUILayout.IntField("Rinse Capacity 3", g.rin3Capacity);

            g.remainingDetergentBottleGreen =
                EditorGUILayout.IntField("Detergent Bottle Green", g.remainingDetergentBottleGreen);
            g.remainingDetergentBottleBlue =
                EditorGUILayout.IntField("Detergent Bottle Blue", g.remainingDetergentBottleBlue);
            g.remainingDetergentBottleRed =
                EditorGUILayout.IntField("Detergent Bottle Red", g.remainingDetergentBottleRed);

            g.remainingRinseBottleGreen = EditorGUILayout.IntField("Rinse Bottle Green", g.remainingRinseBottleGreen);
            g.remainingRinseBottleBlue = EditorGUILayout.IntField("Rinse Bottle Blue", g.remainingRinseBottleBlue);
            g.remainingRinseBottleRed = EditorGUILayout.IntField("Rinse Bottle Red", g.remainingRinseBottleRed);

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Selected Players", EditorStyles.boldLabel);
            for (int i = 0; i < saveData.selectedPlayersList.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                saveData.selectedPlayersList[i].key =
                    EditorGUILayout.TextField("Key", saveData.selectedPlayersList[i].key);
                saveData.selectedPlayersList[i].value =
                    EditorGUILayout.IntField("Value", saveData.selectedPlayersList[i].value);
                if (GUILayout.Button("Remove"))
                {
                    saveData.selectedPlayersList.RemoveAt(i);
                    break;
                }

                EditorGUILayout.EndHorizontal();
            }

            if (GUILayout.Button("Add New Player Selection"))
            {
                saveData.selectedPlayersList.Add(new SaveData.PlayerSelection { key = "New Key", value = 0 });
            }

            EditorGUILayout.Space();
            if (GUILayout.Button("Save Data"))
            {
                saveData.ConvertDictionaryToList();
                SaveGameData();
            }

            if (GUILayout.Button("Reload Data"))
            {
                LoadGameData();
            }

            EditorGUILayout.EndScrollView();
        }

        private void LoadGameData()
        {
            if (File.Exists(savePath))
            {
                string json = File.ReadAllText(savePath);
                saveData = JsonUtility.FromJson<SaveData>(json);
                saveData.ConvertListToDictionary();
            }
            else
            {
                saveData = new SaveData();
            }
        }

        private void SaveGameData()
        {
            saveData.ConvertDictionaryToList();
            string json = JsonUtility.ToJson(saveData, true);
            File.WriteAllText(savePath, json);
            Debug.Log("Save Data Updated.");
        }
    }
}