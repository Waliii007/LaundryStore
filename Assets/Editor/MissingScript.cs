using UnityEditor;
using UnityEngine;

public class MissingScript : EditorWindow
{ 
    [MenuItem("Tools/Missing Script Cleaner")]
    public static void ShowWindow()
    {
        GetWindow<MissingScript>("Missing Script Cleaner");
    }

    private void OnGUI()
    {
        GUILayout.Label("Missing Script Cleaner", EditorStyles.boldLabel);
        
        if (GUILayout.Button("Clean All Missing Scripts in Scene"))
        {
            CleanAllMissingScripts();
        }
        
        if (GUILayout.Button("Clean Missing Scripts from Selected Objects"))
        {
            CleanMissingScriptsFromSelected();
        }
    }

    private static void CleanAllMissingScripts()
    {
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();
        int removedCount = 0;
        
        foreach (GameObject obj in allObjects)
        {
            removedCount += GameObjectUtility.RemoveMonoBehavioursWithMissingScript(obj);
        }
        
        Debug.Log($"Removed {removedCount} missing scripts from scene objects.");
    }

    private static void CleanMissingScriptsFromSelected()
    {
        int removedCount = 0;
        
        foreach (GameObject obj in Selection.gameObjects)
        {
            removedCount += GameObjectUtility.RemoveMonoBehavioursWithMissingScript(obj);
        }
        
        Debug.Log($"Removed {removedCount} missing scripts from selected objects.");
    }
}
