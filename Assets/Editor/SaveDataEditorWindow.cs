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
            if (saveData == null)
            {
                EditorGUILayout.HelpBox("No save data found. Create new data or load existing.", MessageType.Warning);
                if (GUILayout.Button("Create New Save Data"))
                {
                    saveData = new SaveData();
                    SaveGameData();
                }
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
            saveData.dirtyBoxAiUnlocked = EditorGUILayout.IntField("Dirty Box AI Unlocked", saveData.dirtyBoxAiUnlocked);
            saveData.carryCapacityOfAI = EditorGUILayout.IntField("AI Carry Capacity", saveData.carryCapacityOfAI);
            saveData.playerMovingSpeed = EditorGUILayout.IntField("Player Moving Speed", saveData.playerMovingSpeed);
            saveData.speedOfAI = EditorGUILayout.IntField("AI Speed", saveData.speedOfAI);
            saveData.stackingSpeed = EditorGUILayout.IntField("Stacking Speed", saveData.stackingSpeed);
            saveData.playerCapacity = EditorGUILayout.IntField("Player Capacity", saveData.playerCapacity);
            
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Game Economy", EditorStyles.boldLabel);
            saveData.gameEconomy.cashierPrice = EditorGUILayout.IntField("Cashier Price", saveData.gameEconomy.cashierPrice);
            saveData.gameEconomy.machine1Price = EditorGUILayout.IntField("Machine 1 Price", saveData.gameEconomy.machine1Price);
            saveData.gameEconomy.machine2Price = EditorGUILayout.IntField("Machine 2 Price", saveData.gameEconomy.machine2Price);
            saveData.gameEconomy.machine3Price = EditorGUILayout.IntField("Machine 3 Price", saveData.gameEconomy.machine3Price);
            saveData.gameEconomy.hrPrice = EditorGUILayout.IntField("HR Price", saveData.gameEconomy.hrPrice);
            saveData.gameEconomy.hrUpgradePrice = EditorGUILayout.IntField("HR Upgrade Price", saveData.gameEconomy.hrUpgradePrice);
            
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Selected Players", EditorStyles.boldLabel);
            for (int i = 0; i < saveData.selectedPlayersList.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                saveData.selectedPlayersList[i].key = EditorGUILayout.TextField("Key", saveData.selectedPlayersList[i].key);
                saveData.selectedPlayersList[i].value = EditorGUILayout.IntField("Value", saveData.selectedPlayersList[i].value);
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
