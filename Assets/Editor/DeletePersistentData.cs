using UnityEngine;
using UnityEditor;
using System.IO;

public class DeletePersistentData : EditorWindow
{
    [MenuItem("Tools/Delete Save Data")]
    public static void DeleteSaveData()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "MADARAUCHIHA.json");
        
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
            Debug.Log("Save data deleted: " + filePath);
        }
        else
        {
            Debug.LogWarning("No save file found at: " + filePath);
        }
    }
}