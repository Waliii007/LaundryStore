using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace LaundaryMan
{
    // public class QueueSystemEditor : EditorWindow
    // {
    //     private QueueSystem queueSystem;
    //     private SerializedObject serializedQueueSystem;
    //     private List<GameObject> customQueuePoints = new List<GameObject>();
    //
    //     [MenuItem("Window/Laundry Queue System")]
    //     public static void ShowWindow()
    //     {
    //         GetWindow<QueueSystemEditor>("Laundry Queue System");
    //     }
    //
    //     private void OnEnable()
    //     {
    //         queueSystem = FindObjectOfType<QueueSystem>();
    //         if (queueSystem != null)
    //             serializedQueueSystem = new SerializedObject(queueSystem);
    //     }
    //
    //     private void OnGUI()
    //     {
    //         if (queueSystem == null)
    //         {
    //             EditorGUILayout.HelpBox("QueueSystem not found in scene!", MessageType.Warning);
    //             if (GUILayout.Button("Refresh")) OnEnable();
    //             return;
    //         }
    //
    //         serializedQueueSystem.Update();
    //
    //         EditorGUILayout.LabelField("Queue System Settings", EditorStyles.boldLabel);
    //
    //         EditorGUILayout.PropertyField(serializedQueueSystem.FindProperty("dirtyQueuePoints"), true);
    //         EditorGUILayout.PropertyField(serializedQueueSystem.FindProperty("washedQueuePoints"), true);
    //         EditorGUILayout.PropertyField(serializedQueueSystem.FindProperty("aiPrefab"), true);
    //         EditorGUILayout.PropertyField(serializedQueueSystem.FindProperty("aiSpawnPoint"));
    //         EditorGUILayout.PropertyField(serializedQueueSystem.FindProperty("aiParent"));
    //
    //         GUILayout.Space(10);
    //
    //         EditorGUILayout.LabelField("Custom Queue Points", EditorStyles.boldLabel);
    //         for (int i = 0; i < customQueuePoints.Count; i++)
    //         {
    //             customQueuePoints[i] = (GameObject)EditorGUILayout.ObjectField($"Queue Point {i+1}", customQueuePoints[i], typeof(GameObject), true);
    //         }
    //         if (GUILayout.Button("Add Queue Point"))
    //         {
    //             customQueuePoints.Add(null);
    //         }
    //         if (GUILayout.Button("Apply to Dirty Queue"))
    //         {
    //             for (int i = 0; i < queueSystem.dirtyQueuePoints.Count && i < customQueuePoints.Count; i++)
    //             {
    //                 queueSystem.dirtyQueuePoints[i].queuePoint = customQueuePoints[i];
    //             }
    //         }
    //
    //         GUILayout.Space(10);
    //         if (GUILayout.Button("Spawn AI")) queueSystem.SpawnOnButton();
    //         if (GUILayout.Button("Dequeue AI")) queueSystem.DequeueOnButton();
    //
    //         serializedQueueSystem.ApplyModifiedProperties();
    //     }
    // }
}