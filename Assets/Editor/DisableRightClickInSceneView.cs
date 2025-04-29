using UnityEditor;
using UnityEngine;

 
    [InitializeOnLoad]

    public static class DisableRightClickInSceneView  
    {
        static DisableRightClickInSceneView()
        {
            SceneView.duringSceneGui += OnSceneGUI;
        }

        static void OnSceneGUI(SceneView sceneView)
        {
            Event e = Event.current;
            if (e != null && e.type == EventType.ContextClick)
            {
                e.Use(); // Consume the right-click event
            }
        }
    }
  