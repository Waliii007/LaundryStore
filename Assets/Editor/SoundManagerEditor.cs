using UnityEditor;
using UnityEngine;
using System;
using System.Linq;

[CustomEditor(typeof(LaundaryMan.SoundManager))]
public class SoundManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        LaundaryMan.SoundManager soundManager = (LaundaryMan.SoundManager)target;

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Sound Enum Manager", EditorStyles.boldLabel);

        if (GUILayout.Button("Add New Sound Enum"))
        {
            AddNewSoundEnum();
        }
    }

    private void AddNewSoundEnum()
    {
        string path = "Assets/_ProvidedAssets/Script/SoundManager/SoundManager.cs"; // Adjust if needed
        string[] lines = System.IO.File.ReadAllLines(path);

        int lastIndex = lines.ToList().FindLastIndex(line => line.Contains("}"));
        if (lastIndex > 0)
        {
            string newSound = EditorUtility.DisplayDialogComplex("Add New Sound", "Enter new sound name:", "OK", "Cancel", "") == 0
                ? EditorUtility.DisplayDialog("Sound Name", "Enter new sound name:", "OK") ? "NewSound" : null
                : null;

            if (!string.IsNullOrEmpty(newSound))
            {
                lines[lastIndex] = $"    {newSound},\n" + lines[lastIndex];
                System.IO.File.WriteAllLines(path, lines);
                AssetDatabase.Refresh();
            }
        }
    }
}