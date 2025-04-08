using UnityEditor;
using UnityEngine;

namespace LaundaryMan
{
    [CustomEditor(typeof(SaveAndLoadSystem))]
    public class SaveAndLoadSystemEditor : Editor
    {
        
        // public override void OnInspectorGUI()
        // {
        //     DrawDefaultInspector(); // Default properties show kare
        //
        //     SaveAndLoadSystem saveSystem = (SaveAndLoadSystem)target;
        //
        //     if (GUILayout.Button("Save Game"))
        //     {
        //         saveSystem.SaveGame();
        //     }
        //
        //     if (GUILayout.Button("Load Game"))
        //     {
        //         saveSystem.LoadGame();
        //     }
        //
        //     if (GUILayout.Button("Open Save Data Editor"))
        //     {
        //         SaveDataEditorWindow.OpenWindow(SaveAndLoadSystem.Instance.GameData);
        //     }
        // }
    }
}